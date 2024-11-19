using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyScreen : UIScreen
{
    Button backButton, startButton;
    TextField idTextField;
    Label idLabel;
    DropdownField gamemodeDropdown;
    DropdownField optionsDropdown;
    VisualElement team1Container, team2Container;
    [SerializeField] private VisualTreeAsset userCardTemplate;
    public LobbyScreen(VisualElement root) : base(root)
    {
    }
    public override void SetVisualElements()
    {
        base.SetVisualElements();
        backButton = root.Q<Button>("btn__back");
        startButton = root.Q<Button>("btn__start");
        idTextField = root.Q<TextField>("textfield__id");
        idLabel = root.Q<Label>("label__id");
        gamemodeDropdown = root.Q<DropdownField>("dropdown__gamemode");
        optionsDropdown = root.Q<DropdownField>("dropdown__options");
        CustomDropdown.SetupDropdown(root, "dropdown__gamemode", new List<string> { "C4", "Thường" });
        CustomDropdown.SetupDropdown(root, "dropdown__options", new List<string> { "1 vs 1", "3 vs 3", "5 vs 5" });
        team1Container = root.Q<VisualElement>("visualelement__players-team1");
        team2Container = root.Q<VisualElement>("visualelement__players-team2");
    }
    public override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();
        backButton.RegisterCallback<ClickEvent>(ClickBackButton);
        LobbyEvent.PlayerConnected += OnPlayerConnected;
        gamemodeDropdown.RegisterValueChangedCallback(evt =>
        {
            
        });
        optionsDropdown.RegisterValueChangedCallback(evt =>
        {
            
        });
    }
    void OnPlayerConnected(string playerName, string  avatarPath)
    {
        LoaderUserLobby(playerName, avatarPath, team2Container);
    }
    void LoaderUserLobby(string playerName, string avatarPath, VisualElement targetContainer)
    {
        var userCard = userCardTemplate.Instantiate();
        var nameLabel = userCard.Q<Label>("label__name-lobby");
        var avatarImage = userCard.Q<VisualElement>("visualelement__logo");

        nameLabel.text = playerName;
        targetContainer.Add(userCard);
    }
    public void ClickBackButton(ClickEvent evt)
    {
        GameEvent.BackButtonCLicked?.Invoke();
    }
}
