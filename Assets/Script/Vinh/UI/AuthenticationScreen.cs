using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class AuthenticationScreen : UIScreen
{
    private Button registerSwitchBtn, loginSwitchBtn, loginBtn, registerBtn;
    private VisualElement registerFrame, loginFrame;
    private TextField logUsername, logPassword, regUsername, regPassword, regRepassword, regEmail;
    private Toggle rememberToggle;
    private Label errorText;

    public override void Initialize()
    {

        registerSwitchBtn = root.Q<Button>("Register_Switch_btn");
        loginSwitchBtn = root.Q<Button>("Login_Switch_btn");
        loginBtn = root.Q<Button>("Login_Btn");
        registerBtn = root.Q<Button>("Register_Btn");

        registerFrame = root.Q<VisualElement>("Register_Frame");
        loginFrame = root.Q<VisualElement>("Login_Frame");

        rememberToggle = root.Q<Toggle>("Toggle_RememberPass");

        logUsername = root.Q<TextField>("Log_Username_textfield");
        logPassword = root.Q<TextField>("Log_Password_textfield");
        regUsername = root.Q<TextField>("Reg_Username_textfield");
        regPassword = root.Q<TextField>("Reg_Password_textfield");
        regRepassword = root.Q<TextField>("Reg_Repassword_textfield");
        regEmail = root.Q<TextField>("Reg_Email_textfield");
        errorText = root.Q<Label>("Log_ErrorMessage_label");


        registerSwitchBtn.clicked += OnRegisterSwitchButtonClick;
        loginBtn.clicked += OnLoginButtonClick;
        registerBtn.clicked += OnRegisterButtonClick;
        loginSwitchBtn.clicked += OnLoginSwitchButtonClick;

        GameEvent.RegisterSuccessful += OnLoginSwitchButtonClick;

        string savedPassword = SaveManager.Instance.ReadLocalFile("password");
        if (!string.IsNullOrEmpty(savedPassword))
        {
            logPassword.value = savedPassword;
        }
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
    void HandleLoginSuccessful()
    {
        UIManager.Instance.ShowUI("HomeScreen");
    }
    private void OnLoginButtonClick()
    {
        Debug.Log(logUsername.value + logPassword.value);
        if (string.IsNullOrEmpty(logUsername.value) || string.IsNullOrEmpty(logPassword.value))
        {
            ShowErrorMessage("Username and password cannot be empty.");
            return;
        }
        HandleRememberPassword();

        StartCoroutine(AuthManager.Instance.LoginUser(logUsername.value, logPassword.value, (success, message) =>
        {
            if (success)
            {
                HandleLoginSuccessful(); // Switch to main menu on successful login
            }
            else
            {

                ShowErrorMessage(message); // Display error message
            }
        }));
    }

    void OnRegisterButtonClick()
    {
        StartCoroutine(AuthManager.Instance.RegisterUser(regUsername.text, regPassword.text, regRepassword.text, regEmail.text, (success, message) =>
        {
            if (success)
            {
                OnLoginSwitchButtonClick();  // Switch to login screen on successful registration
            }
            else
            {
                ShowErrorMessage(message);  // Display error message
            }
        }));
    }

    void HandleRememberPassword()
    {
        if (rememberToggle.value)
        {
            SaveManager.Instance.SaveLocalFile("password", logPassword.text);  // Ideally should save token instead of password
        }
    }

    void ShowErrorMessage(string message)
    {
        Debug.Log(message);
        errorText.text = message;
        errorText.style.display = DisplayStyle.Flex;
        // var errorMessageElement = root.Q<Label>("Error_Message_Label");
        //errorMessageElement.text = message;
        //errorMessageElement.style.display = DisplayStyle.Flex; // Hiện thông báo lỗi
    }
}
