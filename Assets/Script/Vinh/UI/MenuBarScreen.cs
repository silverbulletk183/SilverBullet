using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuBarScreen : UIScreen
{
    private Button playButton, settingsButton, storeButton, homeButton;
    public MenuBarScreen(VisualElement root) : base(root)
    {
    }
    public override void SetVisualElements()
    {
        base.SetVisualElements();
        playButton = root.Q<Button>("button__play");
        homeButton = root.Q<Button>("button__home");
        storeButton = root.Q<Button>("button__store");
        settingsButton = root.Q<Button>("button__settings");
    }
    public override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();
        playButton.RegisterCallback<ClickEvent>(ClickPlayButton);
        settingsButton.RegisterCallback<ClickEvent>(ClickSettingsButton);
        storeButton.RegisterCallback<ClickEvent>(ClickStoreButton);
        homeButton.RegisterCallback<ClickEvent>(ReturnToHomeScreen);
    }
    void ClickStoreButton(ClickEvent evt)
    {
        Debug.Log("Store Button");
        GameEvent.StoreScreenShown?.Invoke();
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
    void ReturnToHomeScreen(ClickEvent evt)
    {
        GameEvent.HomeScreenShown?.Invoke();
    }
}
