using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSelectReady : NetworkBehaviour
{


    public static CharacterSelectReady Instance { get; private set; }


    public event EventHandler OnReadyChanged;


    private Dictionary<ulong, bool> playerReadyDictionary;


    private void Awake()
    {
        Instance = this;

        playerReadyDictionary = new Dictionary<ulong, bool>();
    }


    public void SetPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);

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
           
            Loader.LoadNetwork(Loader.Scene.Start);
        }
    }

    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientId)
    {
        // Nếu clientId đã tồn tại trong từ điển, chuyển đổi trạng thái ready
        if (playerReadyDictionary.ContainsKey(clientId))
        {
            playerReadyDictionary[clientId] = !playerReadyDictionary[clientId];
        }
        else
        {
            // Nếu chưa tồn tại, mặc định là true
            playerReadyDictionary[clientId] = true;
        }

        // Kích hoạt sự kiện OnReadyChanged
        OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }


    public bool IsPlayerReady(ulong clientId)
    {
        return playerReadyDictionary.ContainsKey(clientId) && playerReadyDictionary[clientId];
    }

}