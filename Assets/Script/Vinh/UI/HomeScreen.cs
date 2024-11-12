using Photon.Pun.Demo.Cockpit;
using Photon.Chat;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using ExitGames.Client.Photon;
using Photon.Pun;

[System.Serializable]
public class HomeScreen : UIScreen, IChatClientListener
{
    private Button playButton, settingsButton, storeButton, okButton, homeButton, sendButton;
    private TextField nameTextfield, messageTextfield;
    private VisualElement logoImage, detailContainer, logoDetailImage, chatContainer;
    private TemplateContainer shopScreenTemplate, usernameContainer, rankingListView;
    private Label nameDetailLabel, sdtDetailLabel;

    private ChatClient chatClient;
    private bool isConnected;
    private ScrollView messageScrollview;

    public HomeScreen(VisualElement root) : base(root) { }

    public override void SetVisualElements()
    {
        logoImage = root.Q<VisualElement>("visualelement__logoimage");
        shopScreenTemplate = root.Q<TemplateContainer>("ShopScreen");
        usernameContainer = root.Q<TemplateContainer>("namescreen__container");
        okButton = root.Q<Button>("button__ok");
        detailContainer = root.Q<VisualElement>("visualelement__detailcontainer");
        nameTextfield = root.Q<TextField>("textfield__name");

        nameDetailLabel = root.Q<Label>("label__name-detail");
        sdtDetailLabel = root.Q<Label>("label__sdt-detail");
        logoDetailImage = root.Q<VisualElement>("visualelement__logoimage-detail");

        sendButton = root.Q<Button>("button__send");
        messageTextfield = root.Q<TextField>("textfield__message");
        chatContainer = root.Q<VisualElement>("chat__container");
        messageScrollview = root.Q<ScrollView>("scrollview__message");

        HomeEvent.IsUserActive += ShowUsernameContainer;
    }

    public override void RegisterButtonCallbacks()
    {
        okButton.RegisterCallback<ClickEvent>(ClickOkButton);
        sendButton.RegisterCallback<ClickEvent>(evt => SubmitChatMessage());
        messageTextfield.RegisterCallback<FocusInEvent>(evt => ChatConnect());
        messageTextfield.RegisterCallback<KeyDownEvent>(evt =>
        {
            if (evt.keyCode == KeyCode.Return) SubmitChatMessage();
        });
    }

    private void ChatConnect()
    {
        if (isConnected) return;

        string username = GameDataManager.Instance.UserSO.username;
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(username));
        Debug.Log("Connecting to chat...");
    }

    private void SubmitChatMessage()
    {
        if (!isConnected)
        {
            Debug.Log("Cannot send message: Not connected to the chat server.");
            return;
        }

        if (string.IsNullOrEmpty(messageTextfield.value)) return;

        chatClient.PublishMessage("RegionChannel", messageTextfield.value);
        messageTextfield.value = string.Empty;
    }


    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        Debug.Log($"[Chat - {level}] {message}");
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log($"Chat state changed: {state}");
        if (state == ChatState.Uninitialized) isConnected = false;
    }

    public void OnConnected()
    {
        Debug.Log("Chat connected");
        isConnected = true;
        chatClient.Subscribe(new string[] { "RegionChannel" });
    }

    public void OnDisconnected()
    {
        isConnected = false;
        Debug.Log("Disconnected from chat");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            Label messageLabel = new Label($"{senders[i]}: {messages[i]}");
            messageLabel.AddToClassList("chat-message");  // Optional: Add a class for styling
            messageScrollview.Add(messageLabel);
        }

        // Scroll to the bottom to show the latest message
        messageScrollview.ScrollTo(messageScrollview.contentContainer);
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log($"Private message from {sender}: {message}");
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log($"Subscribed to channels: {string.Join(", ", channels)}");
    }

    public void OnUnsubscribed(string[] channels) { }
    public void OnUserSubscribed(string channel, string user) { }
    public void OnUserUnsubscribed(string channel, string user) { }

    private void ClickOkButton(ClickEvent evt)
    {
        HomeEvent.NameInputed?.Invoke(nameTextfield.value);
        nameDetailLabel.text = GameDataManager.Instance.UserSO.username;
        sdtDetailLabel.text = GameDataManager.Instance.UserSO.sdt;
        logoDetailImage.style.backgroundImage = new StyleBackground(GameDataManager.Instance.UserSO.logo);
        AuthManager.Instance.StartUpdateUser(nameTextfield.value);
        HideUsernameContainer();
    }

    public void HideUsernameContainer() => usernameContainer.style.display = DisplayStyle.None;

    public void ShowUsernameContainer(bool isUserActive) => SetVisibility(usernameContainer, !isUserActive);

    private void SetVisibility(VisualElement element, bool isVisible)
    {
        element.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log($"Status update: {user} is now {status}");
    }
}
