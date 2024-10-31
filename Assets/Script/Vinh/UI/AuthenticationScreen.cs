using UnityEngine;
using UnityEngine.UIElements;
public class AuthenticationScreen : UIScreen
{
    private Button _registerSwitchBtn, _loginSwitchBtn, _loginBtn, _registerBtn;
    private VisualElement _registerFrame, _loginFrame;
    private TextField _logUsername, _logPassword, _regUsername, _regPassword, _regRepassword, _regEmail;
    private Toggle _rememberToggle;
    private Label _errorText;

    public override void Initialize()
    {

        _registerSwitchBtn = root.Q<Button>("Register_Switch_btn");
        _loginSwitchBtn = root.Q<Button>("Login_Switch_btn");
        _loginBtn = root.Q<Button>("Login_Btn");
        _registerBtn = root.Q<Button>("Register_Btn");

        _registerFrame = root.Q<VisualElement>("Register_Frame");
        _loginFrame = root.Q<VisualElement>("Login_Frame");

        _rememberToggle = root.Q<Toggle>("Toggle_RememberPass");

        _logUsername = root.Q<TextField>("Log_Username_textfield");
        _logPassword = root.Q<TextField>("Log_Password_textfield");
        _regUsername = root.Q<TextField>("Reg_Username_textfield");
        _regPassword = root.Q<TextField>("Reg_Password_textfield");
        _regRepassword = root.Q<TextField>("Reg_Repassword_textfield");
        _regEmail = root.Q<TextField>("Reg_Email_textfield");
        _errorText = root.Q<Label>("Log_ErrorMessage_label");


        _registerSwitchBtn.clicked += OnRegisterSwitchButtonClick;
        _loginBtn.clicked += OnLoginButtonClick;
        _registerBtn.clicked += OnRegisterButtonClick;
        _loginSwitchBtn.clicked += OnLoginSwitchButtonClick;

        GameEvent.RegisterSuccessful += OnLoginSwitchButtonClick;

        string savedPassword = SaveManager.Instance.ReadLocalFile("password");
        if (!string.IsNullOrEmpty(savedPassword))
        {
            _logPassword.value = savedPassword;
        }
    }

    void OnRegisterSwitchButtonClick()
    {
        _loginFrame.AddToClassList("Hidden");
        _registerFrame.RemoveFromClassList("Hidden");
    }

    void OnLoginSwitchButtonClick()
    {
        _registerFrame.AddToClassList("Hidden");
        _loginFrame.RemoveFromClassList("Hidden");
    }
    void HandleLoginSuccessful()
    {
        UIManager.Instance.ShowUI("HomeScreen");
    }
    private void OnLoginButtonClick()
    {
        Debug.Log(_logUsername.value + _logPassword.value);
        if (string.IsNullOrEmpty(_logUsername.value) || string.IsNullOrEmpty(_logPassword.value))
        {
            ShowErrorMessage("Username and password cannot be empty.");
            return;
        }
        HandleRememberPassword();
        StartCoroutine(AuthManager.Instance.LoginUser(_logUsername.value, _logPassword.value, (success, message) =>
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
        StartCoroutine(AuthManager.Instance.RegisterUser(_regUsername.text, _regPassword.text, _regRepassword.text, _regEmail.text, (success, message) =>
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
        if (_rememberToggle.value)
        {
            SaveManager.Instance.SaveLocalFile("password", _logPassword.text);  // Ideally should save token instead of password
        }
    }

    void ShowErrorMessage(string message)
    {
        Debug.Log(message);
        _errorText.text = message;
        _errorText.style.display = DisplayStyle.Flex;
        // var errorMessageElement = root.Q<Label>("Error_Message_Label");
        //errorMessageElement.text = message;
        //errorMessageElement.style.display = DisplayStyle.Flex; // Hiện thông báo lỗi
    }
}
