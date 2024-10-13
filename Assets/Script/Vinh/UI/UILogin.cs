using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UILogin : MonoBehaviour
{
    private Button registerSwitchBtn, loginSwitchBtn, loginBtn, registerBtn;
    private VisualElement registerFrame, loginFrame;
    private TextField logUsername, logPassword, regUsername, regPassword, regRepassword, regEmail;

    private AuthManager authManager;
    private void Awake()
    {
        authManager = FindAnyObjectByType<AuthManager>();
    }
    private void OnEnable()
    {

        //reference
        var root = GetComponent<UIDocument>().rootVisualElement;

        //Button
        registerSwitchBtn = root.Q<Button>("Register_Switch_btn");
        loginSwitchBtn = root.Q<Button>("Login_Switch_btn");
        loginBtn = root.Q<Button>("Login_Btn");
        registerBtn = root.Q<Button>("Register_Btn");


        //Frame to use class list
        registerFrame = root.Q<VisualElement>("Register_Frame");
        loginFrame = root.Q<VisualElement>("Login_Frame");

        //Text Field
        logUsername = root.Q<TextField>("Log_Username_textfield");
        logPassword = root.Q<TextField>("Log_Password_textfield");
        regUsername = root.Q<TextField>("Reg_Username_textfield");
        regPassword = root.Q<TextField>("Reg_Password_textfield");
        regRepassword = root.Q<TextField>("Reg_Repassword_textfield");
        regEmail = root.Q<TextField>("Reg_Email_textfield");
        

        registerSwitchBtn.clicked += OnRegisterSwitchButtonClick;
        loginBtn.clicked += OnLoginButtonClick;
        registerBtn.clicked += OnRegisterButtonClick;
        loginSwitchBtn.clicked += OnLoginSwitchButtonClick;

        //observer
        GameEvent.RegisterSuccessful += OnLoginSwitchButtonClick;

    }

    void OnRegisterSwitchButtonClick()
    {
        loginFrame.AddToClassList("Hidden");
        registerFrame.RemoveFromClassList("Hidden");
    }
    void OnLoginSwitchButtonClick()
    {
        registerFrame.AddToClassList("Hidden");
        loginFrame.RemoveFromClassList("Hidden");
    }
    void OnLoginButtonClick()
    {
        Debug.Log(logUsername.text);
        Debug.Log(logPassword.text);
        StartCoroutine(authManager.LoginUser(logUsername.text, logPassword.text));
    }
    void OnRegisterButtonClick()
    {
        if (regUsername == null || regPassword == null || regRepassword == null || regEmail == null)
        {
            Debug.LogError("One or more TextFields are not assigned!");
            return;
        }

        if (authManager == null)
        {
            Debug.LogError("AuthManager is not assigned!");
            return;
        }
        StartCoroutine(authManager.RegisterUser(regUsername.text, regPassword.text, regRepassword.text, regEmail.text));
    }
}
