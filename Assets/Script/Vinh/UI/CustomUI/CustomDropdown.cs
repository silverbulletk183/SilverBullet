using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomDropdown : MonoBehaviour
{

     public void Initialize()
    {
        // Get the root VisualElement from the UIDocument
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

        // Thiết lập DropdownField cho từng dropdown khác nhau
        SetupDropdown(rootVisualElement, "language-dropdown", new List<string> { "English", "Vietnamese", "Chinese" });
        SetupDropdown(rootVisualElement, "highlight-dropdown", new List<string> { "Red", "Yellow", "Orange" });
    }

    // Phương thức để thiết lập DropdownField
    private void SetupDropdown(VisualElement root, string dropdownName, List<string> choices)
    {
        // Get a reference to the dropdown field from UXML
        var uxmlField = root.Q<DropdownField>(dropdownName);
        if (uxmlField != null)
        {
            uxmlField.choices = choices;
            uxmlField.value = choices[0];  // Set default value
        }
        else
        {
            Debug.LogError($"DropdownField '{dropdownName}' not found!");
        }
    }
}
