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
    private NetworkList<PlayerData> playerDataNetworkList = new NetworkList<PlayerData>();
    public event EventHandler OnPlayerDataNetworkListChanged;

    private string playerName;
    private string userID ;

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

       // playerDataNetworkList = new NetworkList<PlayerData>();
        playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;

        playerName = UserData.Instance.nameAcc;
        userID = UserData.Instance.userId;
    }
   /* private void OnEnable()
    {
        // Đảm bảo sự kiện được đăng ký khi kích hoạt
        if (playerDataNetworkList != null)
        {
            playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;
        }
    }
    private void OnDisable()
    {
        // Hủy đăng ký sự kiện khi vô hiệu hóa
        if (playerDataNetworkList != null)
        {
            playerDataNetworkList.OnListChanged -= PlayerDataNetworkList_OnListChanged;
        }
    }*/
    void Start()
    {
        // Bật chế độ phát hiện rò rỉ bộ nhớ
       // NativeLeakDetection.Mode = NativeLeakDetectionMode.EnabledWithStackTrace;
    }

    private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        Debug.Log("playerdatalist"+playerDataNetworkList.Count);
        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartHost()
    {
        if (NetworkManager.Singleton != null)
        {

            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectedCallback;
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
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
           //NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectedCallback;
            NetworkManager.Singleton.StartClient();
        }
        else
        {
            Debug.LogError("NetworkManager.Singleton is null!");
        }
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
        if (!IsServer) return;

        playerDataNetworkList.Add(new PlayerData
        {
            clientId = clientId,

        });
        SetPlayerNameServerRpc(GetPlayerName());
        SetUserIDServerRpc(GetUserID());
       // hideCam();
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

    private void NetworkManager_OnClientDisconnectedCallback(ulong clientId)
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
        Debug.Log(playerDataNetworkList.Count+ "playerDataNetworkList");
    }

    public bool IsPlayerIndexConneted(int playerIndex)
    {
        return playerIndex < playerDataNetworkList.Count;
    }

    public void LeaveLobby()
    {
        try
        {
            if (SilverBulletGameLobby.Instance != null)
            {
                SilverBulletGameLobby.Instance.LeaveLobby();
            }

            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= NetworkManager_OnClientConnectedCallback;
                NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectedCallback;

                if (IsServer)
                {
                    // Clear the list only on the server
                    playerDataNetworkList.Clear();
                }

                NetworkManager.Singleton.Shutdown();
            }

            Loader.Load(Loader.Scene.mainHomecp);
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


    /* public override void OnDestroy()
     {
         // Hủy đăng ký sự kiện
         if (NetworkManager.Singleton != null)
         {
             NetworkManager.Singleton.OnClientConnectedCallback -= NetworkManager_OnClientConnectedCallback;
             NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectedCallback;
         }

         // Xóa sự kiện của NetworkList
         if (playerDataNetworkList != null)
         {
             playerDataNetworkList.OnListChanged -= PlayerDataNetworkList_OnListChanged;

             // Thay vì Dispose, hãy Clear
             playerDataNetworkList.Clear();
         }

         // Đặt lại singleton nếu đúng là instance hiện tại
         if (Instance == this)
         {
             Instance = null;
         }

         base.OnDestroy();
     }
     public void Dispose()
     {
         // Kiểm tra và giải phóng NetworkList
         if (playerDataNetworkList != null)
         {
             playerDataNetworkList.Clear();
             playerDataNetworkList.Dispose();
             playerDataNetworkList = null;
         }
     }*/
    /* private void OnDestroy()
     {
         if (NetworkManager.Singleton != null)
         {
             NetworkManager.Singleton.OnClientConnectedCallback -= NetworkManager_OnClientConnectedCallback;
             NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectedCallback;
         }

         if (playerDataNetworkList != null)
         {
             playerDataNetworkList.OnListChanged -= PlayerDataNetworkList_OnListChanged;

             if (IsServer)
             {
                 playerDataNetworkList.Clear(); // Ensure the list is cleared on the server
             }
         }

         if (Instance == this)
         {
             Instance = null;
         }
     }*/
}