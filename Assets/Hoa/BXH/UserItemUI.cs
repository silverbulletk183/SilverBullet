using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtScore;
    
    [SerializeField] private RawImage img;
    [SerializeField] private TextMeshProUGUI txtSelected;
    private User user;


    public static UserItemUI Instance {  get; private set; }
    private void Awake()
    {   
        Instance = this;
    }
    public void SetupUserData(User user)
    {
        this.user = user;
        txtScore.text = user.score.ToString();
       
        StartCoroutine(UploadAndDisplayImage.Instance.LoadImage(APIURL.UserImage+user._id, img));
        
    }
   
   

}
