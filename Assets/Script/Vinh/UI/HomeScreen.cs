using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[System.Serializable]
public class HomeScreen : UIScreen
{
    private Button playButton, settingsButton, storeButton, okButton, homeButton;
    private TextField nameTextfield;
    private VisualElement logoImage, detailContainer, logoDetailImage;
    private TemplateContainer shopScreenTemplate;
    private TemplateContainer usernameContainer;
    private Label nameLabel, nameDetailLabel, sdtDetailLabel;
    private TemplateContainer rankingListView;

    public HomeScreen(VisualElement root) : base(root)
    {
    }

    public override void SetVisualElements()
    {
        //playButton = root.Q<Button>("button__play");
        //homeButton = root.Q<Button>("button__home");
        //storeButton = root.Q<Button>("button__store");
        //settingsButton = root.Q<Button>("button__settings");

        logoImage = root.Q<VisualElement>("visualelement__logoimage");
        shopScreenTemplate = root.Q<TemplateContainer>("ShopScreen");
        usernameContainer = root.Q<TemplateContainer>("namescreen__container");
        okButton = root.Q<Button>("button__ok");
        detailContainer = root.Q<VisualElement>("visualelement__detailcontainer");
        nameTextfield = root.Q<TextField>("textfield__name");

        nameDetailLabel = root.Q<Label>("label__name-detail");
        sdtDetailLabel = root.Q<Label>("label__sdt-detail");
        logoDetailImage = root.Q<VisualElement>("visualelement__logoimage-detail");

        HomeEvent.IsUserActive += ShowUsernameContainer;
    }

    public override void RegisterButtonCallbacks()
    {
        //playButton.RegisterCallback<ClickEvent>(ClickPlayButton);
        //settingsButton.RegisterCallback<ClickEvent>(ClickSettingsButton);
        //storeButton.RegisterCallback<ClickEvent>(ClickStoreButton);
        //homeButton.RegisterCallback<ClickEvent>(ReturnToHomeScreen);

        okButton.RegisterCallback<ClickEvent>(ClickOkButton);

        logoImage.RegisterCallback<MouseEnterEvent>(evt => ShowDetail());
        logoImage.RegisterCallback<MouseLeaveEvent>(evt => HideDetail());
    }

    public void HideUsernameContainer()
    {
        usernameContainer.style.display = DisplayStyle.None;
    }

    public void ShowUsernameContainer(bool isUserActive)
    {
        SetVisibility(usernameContainer, !isUserActive);
    }
    void ClickStoreButton(ClickEvent evt)
    {
        GameEvent.StoreScreenShown?.Invoke();
    } 
    public void ShowDetail()
    {
        SetVisibility(detailContainer, true);
    }

    public void HideDetail()
    {
        SetVisibility(detailContainer, false);
    }

    private void SetVisibility(VisualElement element, bool isVisible)
    {
        element.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
    }

    void ClickOkButton(ClickEvent evt)
    {
        HomeEvent.NameInputed?.Invoke(nameTextfield.value);
        nameDetailLabel.text = GameDataManager.Instance.UserSO.username;
        sdtDetailLabel.text = GameDataManager.Instance.UserSO.sdt;
        logoDetailImage.style.backgroundImage = new StyleBackground(GameDataManager.Instance.UserSO.logo);
        AuthManager.Instance.StartUpdateUser(nameTextfield.value);
        HideUsernameContainer();
    }

    void ReturnToHomeScreen(ClickEvent evt)
    {
        GameEvent.HomeScreenShown?.Invoke();
    }

    void ClickPlayButton(ClickEvent evt)
    {
        SilverBulletGameLobby.Instance.CreateLobby("vinh", 10, true, SilverBulletGameLobby.RoomType.GiaiCuu, SilverBulletGameLobby.GameMode.Mode5v5);
        GameEvent.PlayButtonOnClick?.Invoke();
        GameEvent.LobbyScreenShown?.Invoke();
    }

    void ClickSettingsButton(ClickEvent evt)
    {
        GameEvent.SettingScreenShown?.Invoke();
    }
}
