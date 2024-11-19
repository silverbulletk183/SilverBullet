using Photon.Pun.Demo.Cockpit;
using Photon.Chat;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using ExitGames.Client.Photon;
using Photon.Pun;

[System.Serializable]
public class HomeScreen : UIScreen
{
    private Button playButton, settingsButton, storeButton, okButton, homeButton, sendButton;
    private TextField nameTextfield, messageTextfield;
    private VisualElement logoImage, detailContainer, logoDetailImage, chatContainer;
    private TemplateContainer shopScreenTemplate, usernameContainer, rankingListView;
    private Label nameDetailLabel, sdtDetailLabel;
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
        ChatEvent.OnMessageReceived += ShowMessageReceived;
        ChatEvent.ChatMessageSubmited += OnChatMessageSubmited;
    }

    public override void RegisterButtonCallbacks()
    {
        okButton.RegisterCallback<ClickEvent>(ClickOkButton);
        sendButton.RegisterCallback<ClickEvent>(ClickSendButton);
        messageTextfield.RegisterCallback<FocusInEvent>(OnMessageTextfied);
        messageTextfield.RegisterCallback<KeyDownEvent>(evt =>
        {
            if (evt.keyCode == KeyCode.Return) PhotonChatManager.Instance.SubmitPublicChatOnClick();
        });
    }
    void OnMessageTextfied(FocusInEvent evt)
    {
        PhotonChatManager.Instance.ChatConnectOnClick();
        messageScrollview.AddToClassList("scrollview__message-show");
    }
    void ClickSendButton(ClickEvent evt)
    {
        PhotonChatManager.Instance.messages = messageTextfield.value;
        PhotonChatManager.Instance.SubmitPublicChatOnClick();
    }
    void ShowMessageReceived(string sender, object messages)
    {
        Label messageLabel = new Label($"{sender}: {messages}");
        messageScrollview.Add(messageLabel);
    }
    private void OnChatMessageSubmited()
    {
        messageTextfield.value = string.Empty;
    }
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
}