using Photon.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    #region Setup

    [SerializeField] GameObject joinChatButton;
    ChatClient chatClient;
    bool isConnected;
    [SerializeField] string username;

    public void UsernameOnValueChange(string valueIn)
    {
        username = valueIn;
    }

    public void ChatConnectOnClick()
    {
        isConnected = true;
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(username));
        Debug.Log("Connecting");
    }

    #endregion Setup

    #region General

    [SerializeField] GameObject chatPanel;
    string privateReceiver = "";
    string currentChat;
    [SerializeField] InputField chatField;

    [SerializeField] GameObject chatMessagePrefab;
    [SerializeField] Transform chatContent; // ?ây là n?i ch?a các tin nh?n trong ScrollView

    void Start() { }

    void Update()
    {
        if (isConnected)
        {
            chatClient.Service();
        }

        if (!string.IsNullOrEmpty(chatField.text) && Input.GetKey(KeyCode.Return))
        {
            SubmitPublicChatOnClick();
            //SubmitPrivateChatOnClick();
        }
    }

    #endregion General

    #region PublicChat

    public void SubmitPublicChatOnClick()
    {
        if (privateReceiver == "")
        {
            // G?i tin nh?n công khai
            chatClient.PublishMessage("RegionChannel", currentChat);

            // T?o m?t instance m?i t? prefab
           // GameObject messageObject = Instantiate(chatMessagePrefab, chatContent);
           // Text messageText = messageObject.GetComponent<Text>();

            // C?p nh?t n?i dung tin nh?n
            //messageText.text = $"{username}: {currentChat}";

            chatField.text = "";  // Xóa n?i dung input
            currentChat = "";     // ??t l?i bi?n tin nh?n hi?n t?i

            // Cu?n ??n tin nh?n m?i nh?t
            Canvas.ForceUpdateCanvases(); // C?p nh?t Canvas tr??c
            ScrollRect scrollRect = chatContent.GetComponentInParent<ScrollRect>(); // L?y ScrollRect t? ScrollView
            scrollRect.verticalNormalizedPosition = 0f; // Cu?n xu?ng ?áy
        }
    }

    public void TypeChatOnValueChange(string valueIn)
    {
        currentChat = valueIn;
    }

    #endregion PublicChat

    #region PrivateChat

    public void ReceiverOnValueChange(string valueIn)
    {
        privateReceiver = valueIn;
    }

    public void SubmitPrivateChatOnClick()
    {
        if (privateReceiver != "")
        {
            chatClient.SendPrivateMessage(privateReceiver, currentChat);
            chatField.text = "";
            currentChat = "";
        }
    }

    #endregion PrivateChat

    #region Callbacks

    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        Debug.Log($"[{level}] {message}");
    }

    public void OnChatStateChange(ChatState state)
    {
        if (state == ChatState.Uninitialized)
        {
            isConnected = false;
            joinChatButton.SetActive(true);
            chatPanel.SetActive(false);
        }
    }

    public void OnConnected()
    {
        Debug.Log("Connected");
        joinChatButton.SetActive(false);
        chatClient.Subscribe(new string[] { "RegionChannel" });
    }

    public void OnDisconnected()
    {
        isConnected = false;
        joinChatButton.SetActive(true);
        chatPanel.SetActive(false);
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            // T?o m?t instance m?i t? prefab
            GameObject messageObject = Instantiate(chatMessagePrefab, chatContent);
            Text messageText = messageObject.GetComponent<Text>();

            // C?p nh?t n?i dung tin nh?n
            string msg = string.Format("{0}: {1}", senders[i], messages[i]);
            messageText.text = msg;

            Debug.Log(msg);
        }

        // Cu?n ??n tin nh?n m?i nh?t
        Canvas.ForceUpdateCanvases(); // C?p nh?t Canvas tr??c
        ScrollRect scrollRect = chatContent.GetComponentInParent<ScrollRect>(); // L?y ScrollRect t? ScrollView
        scrollRect.verticalNormalizedPosition = 0f; // Cu?n xu?ng ?áy
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        string msgs = string.Format("(Private) {0}: {1}", sender, message);
        Debug.Log(msgs);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message) { }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        chatPanel.SetActive(true);
    }

    public void OnUnsubscribed(string[] channels) { }

    public void OnUserSubscribed(string channel, string user) { }

    public void OnUserUnsubscribed(string channel, string user) { }

    #endregion Callbacks
}
