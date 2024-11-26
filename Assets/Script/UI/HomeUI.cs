using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button btnOptional;
    [SerializeField] private Button btnStart;
    [SerializeField] private Button btnShop;
    [SerializeField] private Button btnAVT;
    [SerializeField] private RawImage avt;
    private void Start()
    {
        btnOptional.onClick.AddListener(() =>
        {
            OptionalUI.Instance.Show();
        });
        btnStart.onClick.AddListener(()=> {
            SilverBulletGameLobby.Instance.JoinFirstMatchingLobby(10, 0, SilverBulletGameLobby.RoomType.TuChien, SilverBulletGameLobby.GameMode.Mode5v5);
            });
        btnShop.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.Shop);
        });
        btnAVT.onClick.AddListener(() =>
        {
            UploadFile.instance.OnUploadButtonClick();
           
        });
    }

}
