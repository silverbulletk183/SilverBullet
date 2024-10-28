using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public VisualElement root;

    public VisualTreeAsset homeScreen;
    public VisualTreeAsset authenticationScreen;
    public VisualTreeAsset settingScreen;
    public VisualTreeAsset gameScreen;
    public VisualTreeAsset lobbyScreen;

    private Dictionary<string, VisualTreeAsset> uiScreens = new Dictionary<string, VisualTreeAsset>();
    private UIScreen currentScreen;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument != null )
        {
            root = uiDocument.rootVisualElement;
        }else
        {
            Debug.LogError("UIDocument component is missing on the GameObject.");
            return;
        }

        InitializeScreens();
    }

    private void InitializeScreens()
    {
        uiScreens.Add("HomeScreen", homeScreen);
        uiScreens.Add("AuthenticationScreen", authenticationScreen);
        uiScreens.Add("SettingScreen", settingScreen);
        uiScreens.Add("GameScreen", gameScreen);
        uiScreens.Add("LobbyScreen", lobbyScreen);



        ShowUI("AuthenticationScreen");
    }
    public void ShowUI(string screenName)
    {
        if (uiScreens.ContainsKey(screenName))
        {
            if (currentScreen != null)
            {
                currentScreen.Hide();
                Destroy(currentScreen);
            }
            
            root.Clear();

            //clone visualtree
            VisualElement uiElement = uiScreens[screenName].CloneTree();
            uiElement.StretchToParentSize();
            root.Add(uiElement);

            UIScreen screenScript = GetScreenScript(screenName);
            screenScript.root = root;
            screenScript.Initialize();
            screenScript.Show();

            currentScreen = screenScript;
        }
    }

    private UIScreen GetScreenScript(string screenName)
    {
        switch (screenName)
        {
            case "AuthenticationScreen": return gameObject.AddComponent<AuthenticationScreen>();
            case "HomeScreen": return gameObject.AddComponent<HomeScreen>();
            case "SettingScreen": return gameObject.AddComponent<SettingScreen>();
            case "GameScreen": return gameObject.AddComponent<GameScreen>();
            case "LobbyScreen": return gameObject.AddComponent<LobbyScreen>();
            default: return null;
        }
    }
}
