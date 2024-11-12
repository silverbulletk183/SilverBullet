using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingScreen : UIScreen
{
    private Button backButton;

    public SettingScreen(VisualElement root) : base(root)
    {
        
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

    public override void SetVisualElements()
    {
        base.SetVisualElements();
        backButton = root.Q<Button>("btn_close");
        CustomDropdown.SetupDropdown(root, "language-dropdown", new List<string> { "English", "Vietnamese", "Chinese" });
        CustomDropdown.SetupDropdown(root, "highlight-dropdown", new List<string> { "Red", "Yellow", "Orange" });
        TabbedMenu tabbedMenu = new TabbedMenu(root);
        tabbedMenu.Initialize();
    }
}
