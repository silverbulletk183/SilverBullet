using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btnClinent;
    public Button btnHost;
    public Button btnOptional;
    public TMP_InputField edtIDRoom;
    public Button btnJoinWithIDRoom;

     void Start()
    {
        if (SilverBulletGameLobby.Instance == null)
        {
            Debug.LogError("SilverBulletGameLobby.Instance is not initialized!");
            return;
        }
        btnClinent.onClick.AddListener(() =>
         {
            SilverBulletGameLobby.Instance.QuickJoin();
         });
        btnHost.onClick.AddListener(() =>
        {
            
            SilverBulletGameLobby.Instance.CreateLobby("SilverBullet "+Random.Range(0,100),5,false,SilverBulletGameLobby.RoomType.TuChien,SilverBulletGameLobby.GameMode.Mode5v5);
        });
        btnJoinWithIDRoom.onClick.AddListener(() =>
        {
            SilverBulletGameLobby.Instance.JoinWithIDRoom(edtIDRoom.text);
       });
    }
}
