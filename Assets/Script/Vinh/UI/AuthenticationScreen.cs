using UnityEngine;
using UnityEngine.UIElements;
public class AuthenticationScreen : UIScreen
{
    private Button _registerSwitchBtn, _loginSwitchBtn, _loginBtn, _registerBtn;
    private VisualElement _registerFrame, _loginFrame;
    private TextField _logUsername, _logPassword, _regUsername, _regPassword, _regRepassword, _regEmail;
    private Toggle _rememberToggle;
    private Label _errorText;

    public AuthenticationScreen(VisualElement root) : base(root)
    {
        AuthenticationEvent.LoginFail += OnLoginFail;
        AuthenticationEvent.RegisterFail += OnRegisterFail;
    }
    public override void SetVisualElements()
    {
        base.SetVisualElements();
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
    }
    public override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();
        _registerSwitchBtn.RegisterCallback<ClickEvent>(ClickRegisterSwitchButton);
        _loginBtn.RegisterCallback<ClickEvent>(ClickLoginButton);
        _registerBtn.RegisterCallback<ClickEvent>(ClickRegisterButton);
        GameEvent.RegisterSwitchButtonClicked += OnRegisterSwitchButtonClick;
    }
    void ClickRegisterSwitchButton(ClickEvent evt)
    {
        GameEvent.RegisterSwitchButtonClicked?.Invoke();
    }
    void ClickLoginButton(ClickEvent evt)
    {
        AuthenticationEvent.LoginButtonClicked?.Invoke(_logUsername.value, _logPassword.value);
    }
    void ClickRegisterButton(ClickEvent evt)
    {
        AuthenticationEvent.RegisterButtonClicked?.Invoke(_regUsername.text, _regPassword.text, _regRepassword.text, _regEmail.text);
    }

    void OnLoginFail(string message)
    {
        ShowErrorMessage(message);
    }
    void OnRegisterFail(string message)
    {
        ShowErrorMessage(message);
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
    }
}
