using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject UILogin;
    [SerializeField] private GameObject UIStartScreen;
    [SerializeField] private GameObject UIMenuScreen;
    [SerializeField] private GameObject UISetting;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void OnDisable()
    {
        UnSubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        GameEvent.LoginSuccessful += OnLoginSuccess;
        GameEvent.OptionButtonOnClick += OnOptionClicked;
        GameEvent.SettingsClosed += OnSettingsClosed;
    }
    private void UnSubscribeFromEvents()
    {
        GameEvent.LoginSuccessful -= OnLoginSuccess;
        GameEvent.OptionButtonOnClick -= OnOptionClicked;
        GameEvent.SettingsClosed -= OnSettingsClosed;
    }
    public void OnLoginSuccess()
    {
        UIStartScreen.SetActive(true);
        UILogin.SetActive(false);
    }
    private void OnOptionClicked()
    {
        UISetting.SetActive(true);
    }
    private void OnSettingsClosed()
    {
        UIStartScreen.SetActive(true);
        UISetting.SetActive(false);
    }


}
