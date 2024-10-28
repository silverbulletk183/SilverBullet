using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScreen : UIScreen
{
    CustomDropdown dropdown;
    public override void Initialize()
    {
        dropdown = gameObject.AddComponent<CustomDropdown>();
        dropdown.SetupDropdown(root, "gamemode-dropdown", new List<string> { "3 vs 3", "5 vs 5", "C4" });
    }
}
