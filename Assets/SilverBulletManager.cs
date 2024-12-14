using Photon.Pun.UtilityScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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
    private const float roundTime = 120f;
    private bool isLocalPlayerReady = false;
    private int totalRound = 1;
    public int maxPlayer;
    public string myTeam;
    private bool localGameOver = false;

    public NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    private Dictionary<ulong, bool> playerReadyDictionary;

    private NetworkVariable<FixedString32Bytes> teamWinTheMatch = new NetworkVariable<FixedString32Bytes>("A", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
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
            Application.quitting += OnHostQuitting;
            foreach (var prefab in playerPrefabs)
            {
                NetworkManager.Singleton.AddNetworkPrefab(prefab.gameObject);
            }
        }
        playerReadyDictionary = new Dictionary<ulong, bool>();
        playerSpawnPositions = new Dictionary<ulong, Vector3>();

        lobby = SilverBulletGameLobby.Instance.GetLobby();

        myTeam = (NetworkManager.Singleton.LocalClientId % 2 == 0) ? "A" : "B";
        maxPlayer = lobby.MaxPlayers;
        ConnectAndJoinRandom.Instance.SetRoomID(lobby.LobbyCode + myTeam);
        ConnectAndJoinRandom.Instance.SetMaxPlayer(maxPlayer);
    }

    private void Start()
    {
        teamWinTheMatch.OnValueChanged += SilverBulletManager_ShowGameOverUI;
        countdown.OnValueChanged += InGameUI.Instance.SetTimer;
        teamAWins.OnValueChanged += InGameUI.Instance.UpdateScoreUI;
        teamBWins.OnValueChanged += InGameUI.Instance.UpdateScoreUI;
    }

    private void SilverBulletManager_ShowGameOverUI(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        if (myTeam == teamWinTheMatch.Value)
        {
            WinUI.Instance.Show();
        }
        else
        {
            LoseUI.Instance.Show();
        }

        if (teamWinTheMatch.Value == "A")
        {
            ShowUIClientRpc();
        }
    }

    [ClientRpc]
    private void ShowUIClientRpc()
    {
        if (myTeam == teamWinTheMatch.Value)
        {
            WinUI.Instance.Show();
        }
        else
        {
            LoseUI.Instance.Show();
        }
    }

    private void Update()
    {
        if (!isLocalPlayerReady && Input.GetKeyDown(KeyCode.Space))
        {
            isLocalPlayerReady = true;
            OnPlayerReady?.Invoke(this, EventArgs.Empty);
            SetPlayerReadyServerRpc();
        }

        if (IsServer && state.Value == State.GamePlaying)
        {
            UpdateTimer();
        }

        if (state.Value == State.GameOver && !localGameOver)
        {
            localGameOver = true;
            StartCoroutine(BackToHome());
        }
    }

    private void UpdateTimer()
    {
        countdown.Value -= Time.deltaTime;

        if (countdown.Value <= 0)
        {
            EndRoundServerRpc("D");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void EndRoundServerRpc(string winningTeam)
    {
        state.Value = State.EndRound;

        if (winningTeam == "A")
        {
            teamAWins.Value++;
        }
        else if (winningTeam == "B")
        {
            teamBWins.Value++;
        }
        else
        {
            teamAWins.Value++;
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

        foreach (ulong clientId in playerSpawnPositions.Keys)
        {
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
            {
                var playerObject = client.PlayerObject;
                if (playerObject != null)
                {
                    Vector3 spawnPosition = playerSpawnPositions[clientId];
                    playerObject.transform.position = spawnPosition;

                    NotifyPositionResetClientRpc(clientId, spawnPosition);

                    var healthManager = playerObject.GetComponent<HealthManager>();
                    if (healthManager != null)
                    {
                        healthManager.ResetHealthServerRpc();
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
            teamWinTheMatch.Value = "A";
        }
        else if (teamBWins.Value > teamAWins.Value)
        {
            teamWinTheMatch.Value = "B";
        }
        else
        {
            teamWinTheMatch.Value = UnityEngine.Random.Range(0, 2) == 0 ? "A" : "B";
        }

        SilverBulletManager_ShowGameOverUI(default, teamWinTheMatch.Value);
    }

    private void OnHostQuitting()
    {
        if (IsServer)
        {
            NotifyClientsHostDisconnectedClientRpc();
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == 0)
        {
            NotifyClientsHostDisconnectedClientRpc();
        }
    }

    [ClientRpc]
    private void NotifyClientsHostDisconnectedClientRpc()
    {
        OnHostDisconnected?.Invoke(this, EventArgs.Empty);
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (!IsServer) return;

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            SpawnPlayer(clientId);
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        Vector3 spawnPosition = (clientId % 2 == 0)
            ? new Vector3(UnityEngine.Random.Range(11f, 21.5f), 2.8f, -31.5f)
            : new Vector3(UnityEngine.Random.Range(-11.5f, -2f), 2.8f, 48.0f);

        Transform playerTransform = Instantiate(playerPrefabs[UserData.Instance.userCharacter].transform, spawnPosition, Quaternion.identity);
        NetworkObject networkObject = playerTransform.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.SpawnAsPlayerObject(clientId, true);

            if (!playerSpawnPositions.ContainsKey(clientId))
            {
                playerSpawnPositions[clientId] = spawnPosition;
            }
        }
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
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            state.Value = State.GamePlaying;
            OnGamePlaying?.Invoke(this, EventArgs.Empty);

            NotifyClientsGamePlayingClientRpc();
        }
    }

    [ClientRpc]
    private void NotifyClientsGamePlayingClientRpc()
    {
        MessageInGameUI.instance.hideMessage();
    }

    public IEnumerator BackToHome()
    {
        if (IsServer)
        {
            SilverBulletGameLobby.Instance.DeleteLobby();
        }
        else
        {
            SilverBulletGameLobby.Instance.LeaveLobby();
        }

        yield return new WaitForSeconds(5f);
        NetworkManager.Singleton.Shutdown();
        Loader.LoadNetwork(Loader.Scene.Summary);
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
