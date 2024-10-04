using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class LoginUI : MonoBehaviour
{
    private Button registerSwitchBtn, loginSwitchBtn, loginBtn, registerBtn;
    private VisualElement registerFrame, loginFrame;
    private TextField logUsername, logPassword;

    public AuthManager authManager;
    private void Awake()
    {
  
    }
    private void OnEnable()
    {

        //reference
        var root = GetComponent<UIDocument>().rootVisualElement;
        registerSwitchBtn = root.Q<Button>("Register_Switch_btn");
        loginSwitchBtn = root.Q<Button>("Login_Switch_btn");
        loginBtn = root.Q<Button>("Login_Btn");
        registerBtn = root.Q<Button>("Register_Btn");
        registerFrame = root.Q<VisualElement>("Register_Frame");
        loginFrame = root.Q<VisualElement>("Login_Frame");
        logUsername = root.Q<TextField>("Log_Username_textfield");
        logPassword = root.Q<TextField>("Log_Password_textfield");

        registerSwitchBtn.clicked += OnRegisterSwitchButtonClick;
        loginBtn.clicked += OnLoginButtonClick;
    }

    void OnRegisterSwitchButtonClick()
    {
        loginFrame.AddToClassList("Hidden");
        registerFrame.RemoveFromClassList("Hidden");
    }
    void OnLoginButtonClick()
    {
        StartCoroutine(authManager.LoginUser(logUsername.text, logPassword.text));
    }
}
