using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;

public class TestLobby : MonoBehaviour
{
    // Start is called before the first frame update
    private Lobby hostLobby;
    private float heartBeatTimer;
    public TextMeshProUGUI log;
    public TMP_InputField edtCode;
    private string playerName;
   private async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerName = "DevMonkey" + UnityEngine.Random.Range(1, 99);
        Debug.Log(" " + playerName);
    }
    private void Update()
    {
        HandleLobbyHeartBeat();
    }
    private async void HandleLobbyHeartBeat()
    {
        if (hostLobby != null)
        {
            heartBeatTimer-= Time.deltaTime;
            if (heartBeatTimer < 0f)
            {
                float heartBeatTimerMax = 15f;
                heartBeatTimer = heartBeatTimerMax;
                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }
    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "My Lobby";
            int maxPlayer = 4;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions { IsPrivate = false,
                Player = GetPlayer()
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer,createLobbyOptions);
            log.SetText("create lobby " + lobbyName + "  " + maxPlayer+ " "+ lobby.Id+ " "+lobby.LobbyCode);
            PrintPlayers(lobby);
        }
        catch(LobbyServiceException ex)
        {
            Debug.LogError(ex.Message);
        }
    }
    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder>
                {
                    new QueryOrder(false,QueryOrder.FieldOptions.Created)
                }
            };
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            log.SetText("lobbies found" + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                log.SetText(lobby.Name + "  " + lobby.MaxPlayers);
            }
        }
        catch(LobbyServiceException ex)
        {
            Debug.LogError(ex.Message);
        }
    }
    public async void JoinLobby()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError(ex.Message);
        }
    }
    public async void JoinLobbyWithCode()
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };
            string code = edtCode.text.ToString();
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
           Lobby joinedLobby= await Lobbies.Instance.JoinLobbyByCodeAsync(code, joinLobbyByCodeOptions);
            log.SetText("join code "+"  "+ code);
            PrintPlayers(joinedLobby);
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError(ex.Message);
        }
    }
    public async void QuickJoin()
    {
        try
        {
            await LobbyService.Instance.QuickJoinLobbyAsync();
            log.SetText("QuickJoin " + "  ");
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError(ex.Message);
        }
    }
    public void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Player in lobby"+ lobby.Name);
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + "  " + player.Data["PlayerName"].Value);
        }
    }
    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
                    {
                        {"PlayerName",new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
                    }
        };
    }
}
