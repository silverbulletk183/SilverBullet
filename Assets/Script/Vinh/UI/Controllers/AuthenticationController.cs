using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthenticationController : MonoBehaviour
{
    private void OnEnable()
    {
        AuthenticationEvent.LoginButtonClicked += OnLoginButtonClick;
        AuthenticationEvent.RegisterButtonClicked += OnRegisterButtonClick;
    }
    void OnLoginButtonClick(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            AuthenticationEvent.LoginFail("Username and password cannot be empty.");
            return;
        }
        StartCoroutine(AuthManager.Instance.LoginUser(username, password, (success, message) =>
        {
            if (success)
            {
                GameEvent.HomeScreenShown?.Invoke();
            }
            else
            {
                AuthenticationEvent.LoginFail?.Invoke(message);
            }
        }));
    }
    void OnRegisterButtonClick(string _username, string _password, string _rePassword, string _email)
    {
        StartCoroutine(AuthManager.Instance.RegisterUser(_username, _password, _rePassword, _email, (success, message) =>
        {
            if (success)
            {
                AuthenticationEvent.RegisterSuccess?.Invoke();  // Switch to login screen on successful registration
            }
            else
            {
                AuthenticationEvent.RegisterFail?.Invoke(message);
            }
        }));
    }
}
