using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Voice2 : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("?� k?t n?i ??n Master!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("?� tham gia Lobby!");
        // B?n c� th? t?o ho?c tham gia ph�ng ? ?�y
    }
}