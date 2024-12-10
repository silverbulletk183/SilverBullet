using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageInGameUI : MonoBehaviour
{
    public static MessageInGameUI instance {  get; private set; }
    [SerializeField] private TextMeshProUGUI txt;
    private void Awake()
    {
        instance = this;
      
        
    }
    private void Start()
    {
        SilverBulletManager.Instance.OnPlayerReady += SilverBulletManager_OnPlayerReady;
        SilverBulletManager.Instance.OnGamePlaying += SilverBulletManager_OnGamePlaying;
        SilverBulletManager.Instance.OnHostDisconnected += SilverBulletManager_OnHostDisconnected;
    }

    private void SilverBulletManager_OnHostDisconnected(object sender, EventArgs e)
    {
        showMessage("The host has disconnected. Back to Home");
    }

    private void SilverBulletManager_OnGamePlaying(object sender, EventArgs e)
    {
        hideMessage();
    }

    private void SilverBulletManager_OnPlayerReady(object sender, System.EventArgs e)
    {
        showMessage("Wait for other players to be ready");
    }

    public void showMessage(string txtMess)
    {
        gameObject.SetActive(true);
        txt.text = txtMess;
    }
    public void hideMessage()
    {
        gameObject.SetActive(false);
    }
    
}
