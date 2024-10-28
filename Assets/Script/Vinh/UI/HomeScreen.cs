using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[System.Serializable]
public class HomeScreen : UIScreen
{


    private Button playButton, settingsButton;
    private VisualElement logoImage;


    public override void Initialize()
    {
        playButton = root.Q<Button>("play-btn");
        settingsButton = root.Q<Button>("setting-btn");
        logoImage = root.Q<VisualElement>("logo-image");



        playButton.clicked += OnPlayClicked;
        settingsButton.clicked += OnSettingsClicked;
        logoImage.style.backgroundImage = new StyleBackground(GameDataManager.Instance.UserData().thumbnail);
    }
    void OnSelectedMultipleElements()
    {

    }

    private void OnPlayClicked()
    {
        UIManager.Instance.ShowUI("LobbyScreen");
        SceneManager.LoadScene(1);
    }

    private void OnSettingsClicked()
    {
        UIManager.Instance.ShowUI("SettingScreen");
    }
}
