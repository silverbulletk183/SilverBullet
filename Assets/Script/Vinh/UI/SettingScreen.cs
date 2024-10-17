using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingScreen : UIScreen
{
    private Button backButton;
    private TabbedMenu tabbedMenu;
    private CustomDropdown dropdown;
    public override void Initialize()
    {
        tabbedMenu = gameObject.AddComponent<TabbedMenu>();
        dropdown = gameObject.AddComponent<CustomDropdown>();

        tabbedMenu.Initialize();
        dropdown.Initialize();

        backButton = root.Q<Button>("btn_close");

        Debug.Log(backButton);

        backButton.clicked += OnBackClicked;
    }

    private void OnBackClicked()
    {
        UIManager.Instance.ShowUI("MainMenuScreen");
    }
}
