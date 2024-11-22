using Unity.Netcode;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPoints; // C�c ?i?m spawn ???c g�n qua Inspector
    [SerializeField] private GameObject playerPrefab; // Prefab c?a Player

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                SpawnPlayer(clientId);
            }
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        // Ch?n ng?u nhi�n m?t ?i?m spawn
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // T?o player t?i v? tr� c?a ?i?m spawn
        GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        // G�n NetworkObject v� spawn tr�n m?ng
        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }
}
