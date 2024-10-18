using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;

public class SilverBulletGameLobby : MonoBehaviour
{
    private const string KEY_RELAY_JOIN_CODE = "RelayJoinCode";

    public static SilverBulletGameLobby Instance { get; private set; }

    private Lobby joinedLobby;

    public event EventHandler OnCreateLobbyStarted;
    public event EventHandler OnCreateLobbyFailed;
    public event EventHandler OnJoinStated;
    public event EventHandler OnQuickJoinFailed;
    public event EventHandler OnJoinFailed;
    public event EventHandler OnMaxCCU;
    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;

    public enum RoomType
    {
        TuChien,
        GiaiCuu
    }
    public enum GameMode
    {
        Mode5v5,
        Mode3v3,
        Mode1v1
    }

    public bool MAX_CCU =false;

    public class OnLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeUnityAuthentication();
    }

    private async void InitializeUnityAuthentication()
    {
        if(UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(UnityEngine.Random.Range(0,10000).ToString());
            

            await UnityServices.InitializeAsync(initializationOptions);
            
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter>
            {
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)
            }
            };
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs
            {
                lobbyList = queryResponse.Results
            });
        }catch(LobbyServiceException ex) {
            Debug.Log(ex);
        }

    }
    private async Task<Allocation> AllocateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4 - 1);
            return allocation;
        }catch(RelayServiceException  ex) { Debug.Log(ex);
            return default;
        }
    }

    private async Task<string> GetRelayJoinCode(Allocation allocation)
    {
        try
        {
            String relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            return relayJoinCode;
        }catch(RelayServiceException ex)
        {
            Debug.Log(ex);
            return default;
        }
    }
    private async Task<JoinAllocation> JoinRelay(string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            return joinAllocation;
        }catch(RelayServiceException e)
        {
            Debug.Log(e);
            return default;
        }
    }
    public async void CreateLobby(string lobbyName,int maxPlayer, bool isPrivate,RoomType roomType,GameMode gameMode)
    {
        OnCreateLobbyStarted?.Invoke(this, EventArgs.Empty);
        try
        {
            if (await CheckTotalCCU(maxPlayer))
            {
                OnMaxCCU?.Invoke(this, EventArgs.Empty);
                return;
            }
            
                joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,maxPlayer, new CreateLobbyOptions
                {
                    IsPrivate = isPrivate,

                });
                Allocation allocation = await AllocateRelay();

                string relayJoinCode = await GetRelayJoinCode(allocation);

                await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject> {
                     {
                        KEY_RELAY_JOIN_CODE , new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode)

                     },
                    {
                        "ROOM_TYPE",new DataObject(DataObject.VisibilityOptions.Member,roomType.ToString())
                    },
                    {
                        "GAME_MODE",new DataObject(DataObject.VisibilityOptions.Member,gameMode.ToString())
                    }
                 }
                });
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
                NetworkManager.Singleton.StartHost();
                Loader.LoadNetwork(Loader.Scene.TeamCreationUI);
            
           
           
        }
        catch (LobbyServiceException ex) { 
            Debug.Log(ex);
            OnCreateLobbyFailed?.Invoke(this, EventArgs.Empty);
        }
    }  
    public async void QuickJoin()
    {
        OnJoinStated?.Invoke(this, EventArgs.Empty);
        try{
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            string relayJoinCode= joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
            
            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

            NetworkManager.Singleton.StartClient();
            Loader.Load(Loader.Scene.TeamCreationUI);
        }
        catch(LobbyServiceException ex)
        {
            Debug.Log(ex);
            OnQuickJoinFailed?.Invoke(this, EventArgs.Empty);
        }
    }
    public async void JoinWithIDRoom(string idRoom)
    {
        OnJoinStated?.Invoke(this, EventArgs.Empty);
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(idRoom);

            string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;

            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

            NetworkManager.Singleton.StartClient();
            Loader.Load(Loader.Scene.TeamCreationUI);
        } catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
            OnJoinFailed?.Invoke(this, EventArgs.Empty);

        }
    }
    public async void DeleteLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
                joinedLobby = null;
            }
            catch (LobbyServiceException ex)
            {
                Debug.Log(ex);
            }
        }
    }
    public async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                joinedLobby = null;
            }
            catch (LobbyServiceException ex)
            {
                Debug.LogError(ex);
            }
        }
    }
    public Lobby GetLobby()
    {
        return joinedLobby;
    }
    public async Task<bool> CheckTotalCCU(int addPlayer)
    {
        try {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions();
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            int totalCCU = 0;
            foreach (var item in queryResponse.Results)
            {
                totalCCU += item.MaxPlayers;
            }
            return totalCCU+addPlayer>40?true:false;
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError(ex);
            return default;
        }
    }
}