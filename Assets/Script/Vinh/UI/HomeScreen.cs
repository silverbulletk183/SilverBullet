using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[System.Serializable]
public class HomeScreen : UIScreen
{
    private Button playButton, settingsButton, storeButton, okButton;
    private TextField nameTextfield;
    private VisualElement logoImage, detailContainer, logoDetailImage;
    private TemplateContainer shopScreenTemplate;
    private TemplateContainer usernameContainer;
    private Label nameLabel, nameDetailLabel, sdtDetailLabel;

    public HomeScreen(VisualElement root) : base(root)
    {
    }
    public override void SetVisualElements()
    {
        playButton = root.Q<Button>("play-btn");
        settingsButton = root.Q<Button>("setting-btn");
        logoImage = root.Q<VisualElement>("visualelement__logoimage");
        storeButton = root.Q<Button>("store-btn");
        shopScreenTemplate = root.Q<TemplateContainer>("ShopScreen");
        usernameContainer = root.Q<TemplateContainer>(("namescreen__container"));
        okButton = root.Q<Button>("button__ok");
        detailContainer = root.Q<VisualElement>("visualelement__detailcontainer");
        nameTextfield = root.Q<TextField>("textfield__name");

        nameDetailLabel = root.Q<Label>("label__name-detail");
        sdtDetailLabel = root.Q<Label>("label__sdt-detail");
        logoDetailImage = root.Q<VisualElement>("visualelement__logoimage-detail");
    }
    public override void RegisterButtonCallbacks()
    {
        playButton.RegisterCallback<ClickEvent>(ClickPlayButton);
        settingsButton.RegisterCallback<ClickEvent>(ClickSettingsButton);
        storeButton.RegisterCallback<ClickEvent>(ClickStoreButton);
        okButton.RegisterCallback<ClickEvent>(ClickOkButton);
        logoImage.RegisterCallback<MouseEnterEvent>(evt => ShowDetail());
        logoImage.RegisterCallback<MouseLeaveEvent>(evt => HideDetail());
    }
    public void ShowDetail()
    {
        detailContainer.style.display = DisplayStyle.Flex;
    }
    public void HideDetail()
    {
        detailContainer.style.display = DisplayStyle.None;
    }
    void ClickOkButton(ClickEvent evt)
    {
        HomeEvent.NameInputed?.Invoke(nameTextfield.value);
        nameDetailLabel.text = GameDataManager.Instance.UserSO.username;
        sdtDetailLabel.text = GameDataManager.Instance.UserSO.sdt;
        logoDetailImage.style.backgroundImage = new StyleBackground(GameDataManager.Instance.UserSO.logo);
        usernameContainer.style.display = DisplayStyle.None;
    }
    void ClickStoreButton(ClickEvent evt)
    {
        shopScreenTemplate.RemoveFromClassList("Hidden");
    }
    void ClickPlayButton(ClickEvent evt)
    {
        GameEvent.PlayButtonOnClick?.Invoke();
        GameEvent.LobbyScreenShown?.Invoke();
    }
    void ClickSettingsButton(ClickEvent evt)
    {
        GameEvent.SettingScreenShown?.Invoke();
    }
}
