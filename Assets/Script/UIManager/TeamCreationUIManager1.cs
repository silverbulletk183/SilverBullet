using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeamCreationUIManager1 : MonoBehaviour
{
    
   // public TextMeshProUGUI statusText; 
    public Button readyButton;
    public Button btnBack;
    public TextMeshProUGUI txtIDRoom;
    public TextMeshProUGUI txtRoomType;

    public static TeamCreationUIManager1 Instance { get; private set; }


    public void Awake()
    {
        Instance = this;
        btnBack.onClick.AddListener(() =>
        {
           SilverBulletMultiplayer.Instance.LeaveLobby();
          // NetworkManager.Singleton.Shutdown();
        });
        readyButton.onClick.AddListener(() => 
        { 
            CharacterSelectReady.Instance.SetPlayerReady();
        });
        Lobby lobby = SilverBulletGameLobby.Instance.GetLobby();
        txtIDRoom.text = "ID Room: "+lobby.LobbyCode;
        txtRoomType.text = lobby.Data["ROOMTYPE"].Value + "_" + lobby.Data["GAMEMODE"].Value.Replace("Mode", "");

    }
    public void SetIDRoom(string _IDRoom)
    {
        txtIDRoom.text = _IDRoom;
    }
    public void SetRoomType(string _RoomType)
    {
        txtRoomType.text = _RoomType;
    }
    
}
