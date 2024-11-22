using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtMessage;
    [SerializeField] private Button btnOk;

    private void Awake()
    {
        btnOk.onClick.AddListener(Hide);
    }

    private void Start()
    {
        SilverBulletGameLobby.Instance.OnCreateLobbyStarted += SilverBulletGameLobby_OnCreateLobbyStarted;
        SilverBulletGameLobby.Instance.OnCreateLobbyFailed += SilverBulletGameLobby_OnCreateLobbyFailed;
        SilverBulletGameLobby.Instance.OnJoinStated += SilverBulletGameLobby_OnJoinStated;
        SilverBulletGameLobby.Instance.OnJoinFailed += SilverBulletGameLobby_OnJoinFailed;
        SilverBulletGameLobby.Instance.OnQuickJoinFailed += SilverBulletGameLobby_OnQuickJoinFailed;
        SilverBulletGameLobby.Instance.OnMaxCCU += SilverBulletGameLobby_OnMaxCCU;
        Hide();
    }

    private void SilverBulletGameLobby_OnMaxCCU(object sender, EventArgs e)
    {
        ShowMessage("Total CCU MAX");
    }

    private void SilverBulletGameLobby_OnQuickJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Could not find the room to Join!");
    }

    private void SilverBulletGameLobby_OnJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to join room!");
    }

    private void SilverBulletGameLobby_OnJoinStated(object sender, EventArgs e)
    {
        ShowMessage("Joining room...");
    }

    private void SilverBulletGameLobby_OnCreateLobbyFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to create room!");
    }

    private void SilverBulletGameLobby_OnCreateLobbyStarted(object sender, EventArgs e)
    {
        ShowMessage("Creating room...");
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void ShowMessage(string message)
    {
        Show();
        txtMessage.text = message;
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void OnDestroy()
    {

        SilverBulletGameLobby.Instance.OnCreateLobbyStarted -= SilverBulletGameLobby_OnCreateLobbyStarted;
        SilverBulletGameLobby.Instance.OnCreateLobbyFailed -= SilverBulletGameLobby_OnCreateLobbyFailed;
        SilverBulletGameLobby.Instance.OnJoinStated -= SilverBulletGameLobby_OnJoinStated;
        SilverBulletGameLobby.Instance.OnJoinFailed -= SilverBulletGameLobby_OnJoinFailed;
        SilverBulletGameLobby.Instance.OnQuickJoinFailed -= SilverBulletGameLobby_OnQuickJoinFailed;
        SilverBulletGameLobby.Instance.OnMaxCCU -= SilverBulletGameLobby_OnMaxCCU;
    }
}
