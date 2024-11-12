using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomListview : MonoBehaviour
{
    public static void SetupListview(VisualElement root, string listviewName, List<string> users, VisualTreeAsset listTemplate)
    {
        // Get a reference to the ListView field from UXML
        var listview = root.Q<ListView>(listviewName);

        if (listview == null)
        {
            Debug.LogError($"ListView '{listviewName}' not found!");
            return;
        }

        // Set the itemsSource to the list of choices
        listview.itemsSource = users;

        // Define makeItem to create a new item using the provided VisualTreeAsset template
        listview.makeItem = () =>
        {
            var newListEntry = listTemplate.Instantiate();
            var newListEntryLogic = new CharacterListEntryController();
            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);
            return newListEntry;
        };

        // Define bindItem to bind each choice to a ListView item
        listview.bindItem = (element, index) =>
        {
            // Retrieve the data logic associated with this element
            var entryController = (CharacterListEntryController)element.userData;

            // Update the entry data based on the current index in choices
            entryController.SetCharacterData(users[index]);
        };

        // Optional: Set some styling or interaction properties for user experience
        listview.selectionType = SelectionType.Single;
        listview.style.flexGrow = 1;
    }
}
