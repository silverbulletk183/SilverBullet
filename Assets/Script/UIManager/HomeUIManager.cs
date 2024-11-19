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
      
        btnJoinWithIDRoom.onClick.AddListener(() =>
        {
            SilverBulletGameLobby.Instance.JoinWithIDRoom(edtIDRoom.text);
       });
    }
}
