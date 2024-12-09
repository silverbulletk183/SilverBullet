using Photon.Pun.UtilityScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class SilverBulletManager : NetworkBehaviour
{
    public static SilverBulletManager Instance { get; private set; }
    private Lobby lobby;


    private enum State
    {
        WaitingToStart,
        GamePlaying,
        GameOver,
    }

    [SerializeField] private NetworkObject playerPrefab;
    int index = 1;
    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    private Dictionary<ulong,int> playerHP = new Dictionary<ulong,int>();
    private Dictionary<ulong, bool> playerReadyDictionary;

    private void Awake()
    {
        Instance = this;
        if (IsServer)
        {
            // Đăng ký prefab với NetworkManager
            NetworkManager.Singleton.AddNetworkPrefab(playerPrefab.gameObject);
        }
        playerReadyDictionary = new Dictionary<ulong, bool>();
        lobby = SilverBulletGameLobby.Instance.GetLobby();
        
        string voiceChatID = (NetworkManager.Singleton.LocalClientId % 2 == 0) ? lobby.LobbyCode + "A" : lobby.LobbyCode + "B";
       ConnectAndJoinRandom.Instance.SetRoomID(voiceChatID);
    }
  

    public override void OnNetworkSpawn()
    {
        // Only the server should handle spawning and managing players
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }

        // Add this to debug connection status
        Debug.Log($"Network Spawn - IsServer: {IsServer}, IsClient: {IsClient}, IsHost: {IsHost}");
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        Debug.Log($"Client disconnected: {clientId}");
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {

        if (!IsServer) return; // Double check we're on the server

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            SpawnPlayer(clientId);
            
        }
        
    }

    private void SpawnPlayer(ulong clientId)
    {
        Vector3 spawnPosition = (index % 2 == 0)
            ? new Vector3(UnityEngine.Random.Range(11f, 21.5f), 2.8f, -31.5f)
            : new Vector3(UnityEngine.Random.Range(-11.5f, -2f), 2.8f, 48.0f);

        Transform playerTransform = Instantiate(playerPrefab.transform, spawnPosition, Quaternion.identity);
        NetworkObject networkObject = playerTransform.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.SpawnAsPlayerObject(clientId, true);
            
            Debug.Log($"Spawned player for client {clientId} at position {spawnPosition}");
        }
        else
        {
            Debug.LogError("PlayerPrefab is missing NetworkObject component!");
        }

        index++;
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                // This player is NOT ready
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
           // all ready
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerHPServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerHP[serverRpcParams.Receive.SenderClientId] = 100;
    }

    public override void OnDestroy()
    {
        if (IsServer && NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
            if (NetworkManager.Singleton.SceneManager != null)
            {
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= SceneManager_OnLoadEventCompleted;
            }
        }
        base.OnDestroy();
    }
}