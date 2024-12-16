using Photon.Voice.PUN.UtilityScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class SilverBulletMultiplayer : NetworkBehaviour
{
    public static SilverBulletMultiplayer Instance { get; private set; }

    // Khởi tạo NetworkList ngay khi khai báo
    public NetworkList<PlayerData> playerDataNetworkList = new NetworkList<PlayerData>();
    public event EventHandler OnPlayerDataNetworkListChanged;

    private string playerName;
    private string userID;

    private void Awake()
    {
        // Kiểm tra và quản lý singleton
        if (Instance != null)
        {
            Debug.LogWarning("Multiple instances of SilverBulletMultiplayer detected.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Khởi tạo NetworkList không cần Allocator
      //  playerDataNetworkList.Clear();

       // playerDataNetworkList = new NetworkList<PlayerData>();
        playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;
        Debug.Log("playeredata" + playerDataNetworkList.Count);
        playerName = UserData.Instance.nameAcc;
        userID = UserData.Instance.userId;
    }

    private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartHost()
    {
        if (NetworkManager.Singleton != null)
        {
      
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            Debug.LogError("NetworkManager.Singleton is null!");
        }
    }

    public void StartClient()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Client_OnClientDisconnectCallback;
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback;
            NetworkManager.Singleton.StartClient();
        }
        else
        {
            Debug.LogError("NetworkManager.Singleton is null!");
        }
    }

    private void NetworkManager_Client_OnClientConnectedCallback(ulong obj)
    {
        SetPlayerNameServerRpc(GetPlayerName());
        SetUserIDServerRpc(GetUserID());
    }

    private void NetworkManager_Client_OnClientDisconnectCallback(ulong obj)
    {
        Debug.Log("clineedd");
    }

    public string GetPlayerName()
    {
        return playerName;
    }
    public string GetUserID()
    {
        return userID;
    }
    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        Debug.Log("playerdata"+playerDataNetworkList.Count);
        playerDataNetworkList.Add(new PlayerData
        {
            clientId = clientId,

        });
     
        SetPlayerNameServerRpc(GetPlayerName());
        SetUserIDServerRpc(GetUserID());
        // hideCam();
    }
    private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientId)
    {
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            PlayerData playerData = playerDataNetworkList[i];
            if (playerData.clientId == clientId)
            {
                // Disconnected!
                playerDataNetworkList.RemoveAt(i);
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerNameServerRpc(string playerName, ServerRpcParams serverRpcParams = default)
    {
        
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = playerDataNetworkList[playerDataIndex];

        playerData.playerName = playerName;

        playerDataNetworkList[playerDataIndex] = playerData;

    }
    [ServerRpc(RequireOwnership = false)]
    private void SetUserIDServerRpc(string userID, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = playerDataNetworkList[playerDataIndex];

        playerData.userId = userID;

        playerDataNetworkList[playerDataIndex] = playerData;
    }


    public bool IsPlayerIndexConneted(int playerIndex)
    {
        return playerIndex < playerDataNetworkList.Count;
    }

    public void LeaveLobby(bool backToHome)
    {
        try
        {
           

            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= NetworkManager_OnClientConnectedCallback;
                NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_Server_OnClientDisconnectCallback;
                NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_Client_OnClientDisconnectCallback;
                NetworkManager.Singleton.OnClientConnectedCallback -= NetworkManager_Client_OnClientConnectedCallback;

                if (IsServer)
                {
                    // Clear the list only on the server

                    playerDataNetworkList.Clear();
                    Debug.Log("playerdata" + playerDataNetworkList.Count);
                }

                NetworkManager.Singleton.Shutdown();
            }
            if (SilverBulletGameLobby.Instance != null)
            {

                if (!IsServer)
                {
                    Debug.Log("is delete");
                    SilverBulletGameLobby.Instance.LeaveLobby();
                }
                else
                {
                    SilverBulletGameLobby.Instance.DeleteLobby();
                }
            }
            if (backToHome)
            {
                Loader.Load(Loader.Scene.mainHomecp);
            }
            
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in LeaveLobby: {ex.Message}");
        }
    }


    public PlayerData GetPlayerDataFromPlayerIndex(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < playerDataNetworkList.Count)
        {
            return playerDataNetworkList[playerIndex];
        }
        throw new ArgumentOutOfRangeException(nameof(playerIndex), "Player index is out of range");
    }
    public int GetPlayerDataIndexFromClientId(ulong clientId)
    {
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            if (playerDataNetworkList[i].clientId == clientId)
            {
                return i;
            }
        }
        return -1;
    }


   
}