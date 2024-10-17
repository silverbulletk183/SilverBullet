using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuScreen : UIScreen
{
    private Button playButton;
    private Button settingsButton;

    public override void Initialize()
    {
        playButton = root.Q<Button>("play-btn");
        settingsButton = root.Q<Button>("setting-btn");

        playButton.clicked += OnPlayClicked;
        settingsButton.clicked += OnSettingsClicked;
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
