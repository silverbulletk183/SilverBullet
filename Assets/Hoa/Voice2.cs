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
        Debug.Log("?ã k?t n?i ??n Master!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("?ã tham gia Lobby!");
        // B?n có th? t?o ho?c tham gia phòng ? ?ây
    }
}