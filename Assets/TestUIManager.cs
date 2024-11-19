using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button Create;
    public Button Join;
    void Start()
    {
        Create.onClick.AddListener(() =>
        {
            SilverBulletGameLobby.Instance.CreateLobby("SilverBullet " + Random.Range(0, 100), 10, false, SilverBulletGameLobby.RoomType.TuChien, SilverBulletGameLobby.GameMode.Mode5v5);
        });
        Join.onClick.AddListener(() =>
        {
            SilverBulletGameLobby.Instance.JoinFirstMatchingLobby(10,2,SilverBulletGameLobby.RoomType.TuChien, SilverBulletGameLobby.GameMode.Mode5v5);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
