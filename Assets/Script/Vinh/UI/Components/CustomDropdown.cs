using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class CustomDropdown
{
    public static void SetupDropdown(VisualElement root, string dropdownName, List<string> choices)
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
