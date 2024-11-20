using Unity.Netcode;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPoints; // Các ?i?m spawn ???c gán qua Inspector
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
        // Ch?n ng?u nhiên m?t ?i?m spawn
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // T?o player t?i v? trí c?a ?i?m spawn
        GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        // Gán NetworkObject và spawn trên m?ng
        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }
}
