﻿using Photon.Pun.UtilityScripts;
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
    public event EventHandler OnPlayerReady;
    public event EventHandler OnGamePlaying;
    public event EventHandler OnHostDisconnected;
    

    public enum State
    {
        WaitingToStart,
        GamePlaying,
        EndRound,
        GameOver,
    }

    [SerializeField] private List<NetworkObject> playerPrefabs;
    int index = 1;
    private const float roundTime = 120f; // 2 minutes
    private bool isLocalPlayerReady = false;
    private int totalRound = 5;
    public int maxPlayer;

    public NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    private Dictionary<ulong, bool> playerReadyDictionary;
    public NetworkVariable<int> teamAWins = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> teamBWins = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<float> countdown = new NetworkVariable<float>(roundTime, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> currentRound = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private Dictionary<ulong, Vector3> playerSpawnPositions;



    private void Awake()
    {
        Instance = this;
        if (IsServer)
        {
            // Đăng ký prefab với NetworkManager
            Application.quitting += OnHostQuitting;
            foreach (var prefab in playerPrefabs)
            {
                NetworkManager.Singleton.AddNetworkPrefab(prefab.gameObject);
            }
           // NetworkManager.Singleton.AddNetworkPrefab(playerPrefabs[UserData.Instance.userCharacter].gameObject);
        }
        playerReadyDictionary = new Dictionary<ulong, bool>();
        playerSpawnPositions = new Dictionary<ulong, Vector3>();

        lobby = SilverBulletGameLobby.Instance.GetLobby();
        
        string voiceChatID = (NetworkManager.Singleton.LocalClientId % 2 == 0) ? lobby.LobbyCode + "A" : lobby.LobbyCode + "B";
        maxPlayer = lobby.MaxPlayers;
        ConnectAndJoinRandom.Instance.SetRoomID(voiceChatID);
        ConnectAndJoinRandom.Instance.SetMaxPlayer(maxPlayer);
        
    }
    private void Start()
    {
        countdown.OnValueChanged += InGameUI.Instance.SetTimer;
        teamAWins.OnValueChanged += InGameUI.Instance.UpdateScoreUI;
        teamBWins.OnValueChanged += InGameUI.Instance.UpdateScoreUI;
    }


    private void Update()
    {
        if (!isLocalPlayerReady && Input.GetKeyDown("space"))
        {
            isLocalPlayerReady = true;
            OnPlayerReady?.Invoke(this, EventArgs.Empty);
            SetPlayerReadyServerRpc();
        }
        if(IsServer && state.Value == State.GamePlaying)
        {
            UpdateTimer();
        }
    }
    private void UpdateTimer()
    {
        countdown.Value -= Time.deltaTime;

        if (countdown.Value <= 0)
        {
           // EndRound(Random.Range(0, 2) == 0 ? "A" : "B"); // Simulating team wins for now
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void EndRoundServerRpc(string winningTeam)
    {
        state.Value= State.EndRound;

        if (winningTeam == "A")
        {
            teamAWins.Value++;
        }
        else if (winningTeam == "B")
        {
            teamBWins.Value++;
        }

        currentRound.Value++;

        if (currentRound.Value >= totalRound)
        {
            EndMatch();
        }
        else
        {
            ResetRound();
            StartRound();
        }
    }
    private void ResetRound()
    {
        countdown.Value = roundTime;
        TeamDeathManager.Instance.ResetTeamDeath();

        // Reset player positions
        foreach (ulong clientId in playerSpawnPositions.Keys)
        {
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
            {
                var playerObject = client.PlayerObject;
                if (playerObject != null)
                {
                    // Reset position
                    Vector3 spawnPosition = playerSpawnPositions[clientId];
                    playerObject.transform.position = spawnPosition;

                    // Notify clients to update position explicitly if needed
                    NotifyPositionResetClientRpc(clientId, spawnPosition);

                    Debug.Log($"Player {clientId} moved back to spawn position {spawnPosition}");

                    // Reset health
                    var healthManager = playerObject.GetComponent<HealthManager>();
                    if (healthManager != null)
                    {
                        healthManager.ResetHealthServerRpc(); // Reset health for the player.
                    }
                    else
                    {
                        Debug.LogWarning($"HealthManager not found for player {clientId}");
                    }
                }
            }
        }
    }
    [ClientRpc]
    private void NotifyPositionResetClientRpc(ulong clientId, Vector3 position)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            var playerObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);
            if (playerObject != null)
            {
                playerObject.transform.position = position;
            }
        }
    }




    private void StartRound()
    {
        state.Value = State.GamePlaying;
    }

    private void EndMatch()
    {
        state.Value = State.GameOver;

        if (teamAWins.Value > teamBWins.Value)
        {
           // resultText.text = "Team A Wins!";
        }
        else if (teamBWins.Value > teamAWins.Value)
        {
           // resultText.text = "Team B Wins!";
        }
        else
        {
          //  resultText.text = "It's a Tie!";
        }
    }

    private void OnHostQuitting()
    {
        if (IsServer)
        {
            NotifyClientsHostDisconnectedClientRpc();
            Debug.Log("Host is quitting. Notifying clients.");
        }
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
       // Debug.Log($"Network Spawn - IsServer: {IsServer}, IsClient: {IsClient}, IsHost: {IsHost}");
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        Debug.Log($"Client disconnected: {clientId}");
        if (clientId == 0) // ID của host là 0
        {
            Debug.Log("Host disconnected.");
            NotifyClientsHostDisconnectedClientRpc();
            
        }
    }
    [ClientRpc]
    private void NotifyClientsHostDisconnectedClientRpc()
    {
        OnHostDisconnected?.Invoke(this, EventArgs.Empty);
        Debug.Log("The host has disconnected. Returning to home screen.");
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
        Debug.Log("character" + UserData.Instance.userCharacter);

        Transform playerTransform = Instantiate(playerPrefabs[UserData.Instance.userCharacter].transform, spawnPosition, Quaternion.identity);
        NetworkObject networkObject = playerTransform.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.SpawnAsPlayerObject(clientId, true);

            // Save spawn position
            if (!playerSpawnPositions.ContainsKey(clientId))
            {
                playerSpawnPositions[clientId] = spawnPosition;
            }

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
            Debug.Log("All players are ready");
            state.Value = State.GamePlaying;
            OnGamePlaying?.Invoke(this, EventArgs.Empty);

            // Gửi thông báo đến tất cả client để ẩn message
            NotifyClientsGamePlayingClientRpc();
        }
    }
    

    [ClientRpc]
    private void NotifyClientsGamePlayingClientRpc()
    {
        // Ẩn message trên tất cả các client
        MessageInGameUI.instance.hideMessage();
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