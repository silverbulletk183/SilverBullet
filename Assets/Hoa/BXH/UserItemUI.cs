using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private RawImage img;
    private User user;


    public static UserItemUI Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public void SetupUserData(User user)
    {
        this.user = user;
        txtLevel.text = user.level;
       

        StartCoroutine(UploadAndDisplayImage.Instance.LoadImage(APIURL.UserImage + user._id, img));

    }
}
