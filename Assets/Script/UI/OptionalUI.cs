using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionalUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button btnBack;
    [SerializeField] private Button btn5v5;
    [SerializeField] private Button btn3v3;
    [SerializeField] private Button btn1v1;
    [SerializeField] private Button btnTuChien;
    [SerializeField] private Button btnGiaiCuu;
    private SilverBulletGameLobby.RoomType roomType;

    public static OptionalUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        roomType= SilverBulletGameLobby.RoomType.TuChien;
        btnBack.onClick.AddListener(Hide);
        btn5v5.onClick.AddListener(() =>
        {
            SilverBulletGameLobby.Instance.JoinFirstMatchingLobby(10, 0, roomType, SilverBulletGameLobby.GameMode.Mode5v5);
        });
        btn3v3.onClick.AddListener(() =>
        {
            SilverBulletGameLobby.Instance.JoinFirstMatchingLobby(6, 0, roomType, SilverBulletGameLobby.GameMode.Mode3v3);
        });
        btn1v1.onClick.AddListener(() =>
        {
            SilverBulletGameLobby.Instance.JoinFirstMatchingLobby(2, 0, roomType, SilverBulletGameLobby.GameMode.Mode1v1);
        });
        btnGiaiCuu.onClick.AddListener(() =>
        {
            roomType = SilverBulletGameLobby.RoomType.GiaiCuu;
        });
        btnTuChien.onClick.AddListener(() =>
        {
            roomType = SilverBulletGameLobby.RoomType.TuChien;
        });
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    
}
