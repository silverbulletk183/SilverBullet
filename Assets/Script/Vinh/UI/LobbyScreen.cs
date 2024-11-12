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
    }
    public override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();
        backButton.RegisterCallback<ClickEvent>(ClickBackButton);
        gamemodeDropdown.RegisterValueChangedCallback(evt =>
        {
            
        });
        optionsDropdown.RegisterValueChangedCallback(evt =>
        {
            
        });

    }
    public void ClickBackButton(ClickEvent evt)
    {
        GameEvent.BackButtonCLicked?.Invoke();
    }
}
