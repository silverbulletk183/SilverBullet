using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[System.Serializable]
public class HomeScreen : UIScreen
{
    private Button playButton, settingsButton, storeButton;
    private VisualElement logoImage;
    private TemplateContainer shopScreenTemplate;

    public override void Initialize()
    {
        playButton = root.Q<Button>("play-btn");
        settingsButton = root.Q<Button>("setting-btn");
        logoImage = root.Q<VisualElement>("logo-image");
        storeButton = root.Q<Button>("store-btn");
        shopScreenTemplate = root.Q<TemplateContainer>("ShopScreen");

        playButton.clicked += OnPlayClicked;
        settingsButton.clicked += OnSettingsClicked;
        storeButton.clicked += OnShopClicked;
    }

    private void OnShopClicked()
    {
        shopScreenTemplate.RemoveFromClassList("Hidden");
    }

    private void OnPlayClicked()
    {
        UIManager.Instance.ShowUI("LobbyScreen");
        SceneManager.LoadScene(4);
    }

    private void OnSettingsClicked()
    {
        UIManager.Instance.ShowUI("SettingScreen");
    }
}
