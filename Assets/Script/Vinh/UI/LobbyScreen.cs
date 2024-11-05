using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyScreen : UIScreen
{
    Button backButton, startButton;
    TextField idTextField;
    Label idLabel;

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
        CustomDropdown.SetupDropdown(root, "dropdown__gamemode", new List<string> { "C4", "Thường" });
    }
    public override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();
        backButton.RegisterCallback<ClickEvent>(ClickBackButton);
    }
    public void ClickBackButton(ClickEvent evt)
    {
        GameEvent.BackButtonCLicked?.Invoke();
    }
}
