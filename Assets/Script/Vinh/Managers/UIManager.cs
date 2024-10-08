using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject UILogin;
    [SerializeField] private GameObject UIStartScreen;
    [SerializeField] private GameObject UIMenuScreen;
    [SerializeField] private GameObject UISetting;

    private void Start()
    {
        GameEvent.LoginSuccessful += OnLoginSuccess;
        GameEvent.OptionButtonOnClick += OnOptionClicked;
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
}
