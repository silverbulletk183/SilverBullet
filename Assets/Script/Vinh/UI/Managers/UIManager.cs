using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Script.Vinh.UI.Managers
{
    public class UIManager : MonoBehaviour
    {
        UIDocument m_MainMenuDocument;

        UIScreen m_CurrentView;
        UIScreen m_PreviousView;

        // UI screens as UIScreen instances
        UIScreen m_HomeView;
        UIScreen m_AuthenticationView;
        UIScreen m_SettingView;
        UIScreen m_GameView;
        UIScreen m_LobbyView;
        UIScreen m_ShopView;
        UIScreen m_RankingView;
        UIScreen m_MenubarView;

        List<UIScreen> m_AllViews = new List<UIScreen>();

        // UI screen names as constants
        public const string k_HomeViewName = "HomeScreen";
        public const string k_AuthenticationViewName = "AuthenticationScreen";
        public const string k_SettingViewName = "SettingScreen";
        public const string k_GameViewName = "GameScreen";
        public const string k_LobbyViewName = "LobbyScreen";
        public const string k_ShopViewName = "ShopScreen";
        public const string k_RankingViewName = "RankingScreen";
        public const string k_MenuBarViewName = "MenuBar";

        public UIDocument MainMenuDocument => m_MainMenuDocument;

        private static UIManager _instance;
        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            if (_instance != null && Instance.gameObject != this)
            {
                Destroy(this.gameObject);
            }else
            {
                _instance = this;
            }
            DontDestroyOnLoad(this.gameObject) ;
        }
        void OnEnable()
        {
            m_MainMenuDocument = GetComponent<UIDocument>();
            SetupViews();
            SubscribeToEvents();
            m_AuthenticationView.Show();
        }

        void SubscribeToEvents()
        {
            GameEvent.HomeScreenShown += OnHomeScreenShown;
            GameEvent.AuthenticationScreenShown += OnAuthenticationScreenShown;
            GameEvent.SettingScreenShown += OnSettingScreenShown;
            GameEvent.GameScreenShown += OnGameScreenShown;
            GameEvent.LobbyScreenShown += OnLobbyScreenShown;
            GameEvent.StoreScreenShown += OnShopScreenShown;
            GameEvent.RankingScreenShown += OnRankingScreenShown;
            GameEvent.MenubarShown += OnMenubarShown;
            GameEvent.BackButtonCLicked += ShowPreviousModalView;
            GameEvent.LoginSuccessful += OnLoginSuccessful;
        }

        void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        void UnsubscribeFromEvents()
        {
            GameEvent.HomeScreenShown -= OnHomeScreenShown;
            GameEvent.AuthenticationScreenShown -= OnAuthenticationScreenShown;
            GameEvent.SettingScreenShown -= OnSettingScreenShown;
            GameEvent.GameScreenShown -= OnGameScreenShown;
            GameEvent.LobbyScreenShown -= OnLobbyScreenShown;
        }

        void SetupViews()
        {
            VisualElement root = m_MainMenuDocument.rootVisualElement;

            m_HomeView = new HomeScreen(root.Q<VisualElement>(k_HomeViewName));
            m_AuthenticationView = new AuthenticationScreen(root.Q<VisualElement>(k_AuthenticationViewName));
            m_SettingView = new SettingScreen(root.Q<VisualElement>(k_SettingViewName));
            m_LobbyView = new LobbyScreen(root.Q<VisualElement>(k_LobbyViewName));
            m_ShopView = new StoreScreen(root.Q<VisualElement>(k_ShopViewName));
            m_RankingView = new RankingScreen(root.Q<VisualElement>(k_RankingViewName));
            m_MenubarView = new MenuBarScreen(root.Q<VisualElement>(k_MenuBarViewName));



            m_AllViews.Add(m_HomeView);
            m_AllViews.Add(m_AuthenticationView);
            m_AllViews.Add(m_SettingView);
            m_AllViews.Add(m_GameView);
            m_AllViews.Add(m_LobbyView);
            m_AllViews.Add(m_ShopView);
            m_AllViews.Add(m_RankingView);
            m_AllViews.Add(m_MenubarView);
        }
        void ShowModalView(UIScreen newView)
        {
            m_CurrentView?.Hide();

            m_PreviousView = m_CurrentView;
            m_CurrentView = newView;

            m_CurrentView?.Show();
        }
        void OnLoginSuccessful()
        {
            m_HomeView.Show();
            m_MenubarView.Show();
        }
        void ShowPreviousModalView()
        {
            ShowModalView(m_PreviousView);
        }
        void OnHomeScreenShown()
        {
            ShowModalView(m_HomeView);
        }
        void OnAuthenticationScreenShown() => ShowModalView(m_AuthenticationView);
        void OnSettingScreenShown() => ShowModalView(m_SettingView);
        void OnGameScreenShown() => ShowModalView(m_GameView);
        void OnLobbyScreenShown() => ShowModalView(m_LobbyView);
        void OnShopScreenShown()
        {
            ShowModalView(m_ShopView);
        }
        void OnMenubarShown()
        {
            m_MenubarView.Show();
        }
        void OnRankingScreenShown()
        {
            m_PreviousView = m_CurrentView;
            m_RankingView.Show();
        }
    }
}