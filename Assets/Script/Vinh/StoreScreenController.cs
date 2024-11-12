using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class StoreScreenController
{
    VisualTreeAsset m_ListEntryTemplate;
    VisualTreeAsset m_CardEntryTemplate;

    ListView m_OptionListview;
    ScrollView m_GunScrollview, m_CharacterScrollview;

    List<string> m_OptionsList = new List<string>() { "Súng", "Nhân vật"};
    List<GunData> m_AllGuns;
    List<CharacterData> m_AllCharacters;

    enum ViewState { GunsView, CharactersView }


    public void Initialize(VisualElement root, VisualTreeAsset listElementTemplate, VisualTreeAsset cardElementTemplate)
    {

        DisplayAllGuns();

        // Store a reference to the template for the list entries
        m_ListEntryTemplate = listElementTemplate;
        m_CardEntryTemplate = cardElementTemplate;

        // Store a reference to the character list element
        m_OptionListview = root.Q<ListView>("listview__options");
        m_GunScrollview = root.Q<ScrollView>("scrollview__guns");
        m_CharacterScrollview = root.Q<ScrollView>("scrollview__characters");

        FillOptionsList();
        // Register to get a callback when an item is selected
        m_OptionListview.selectionChanged += OnCharacterSelected;
    }

    async void DisplayAllGuns()
    {
        //enumerate all guns
        m_AllGuns = await AuthManager.Instance.FetchGunAsync();
        FillGunsScrollview(m_AllGuns);
    }
   

    void FillOptionsList()
    {
        m_OptionListview.makeItem = () =>
        {
            var newListEntry = m_ListEntryTemplate.Instantiate();
            var newListEntryLogic = new CharacterListEntryController();
            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);
            return newListEntry;
        };

        m_OptionListview.bindItem = (item, index) =>
        {
            (item.userData as CharacterListEntryController)?.SetCharacterData(m_OptionsList[index]);
        };

        m_OptionListview.fixedItemHeight = 45;
        m_OptionListview.itemsSource = m_OptionsList;

        // Register selection callback for options
        m_OptionListview.selectionChanged += selectedItems =>
        {
            var selectedOption = selectedItems.FirstOrDefault() as string;
            if (selectedOption != null)
            {
                OnOptionSelected(selectedOption);
            }
        };
    }
    void SwitchView(ViewState view)
    {
        // Toggle visibility based on the current view
        m_GunScrollview.style.display = (view == ViewState.GunsView) ? DisplayStyle.Flex : DisplayStyle.None;
        m_CharacterScrollview.style.display = (view == ViewState.CharactersView) ? DisplayStyle.Flex : DisplayStyle.None;
    }
    void OnOptionSelected(string selectedOption)
    {
        // Check the selected option and switch view accordingly
        if (selectedOption == "Súng")
        {
            SwitchView(ViewState.GunsView);
        }
        else if (selectedOption == "Nhân vật")
        {
            SwitchView(ViewState.CharactersView);
        }
        // You can add other cases if you have additional views
    }


    private void FillGunsScrollview(List<GunData> guns)
    {
        VisualElement rowContainer = null; // Container for a row
        int columnCount = 3; // Number of cards per row
        int i = 0; // Counter for columns

        foreach (var gun in guns)
        {
            // Create and configure a card element for the gun
            var cardElement = m_CardEntryTemplate.Instantiate();
            cardElement.style.flexGrow = 1;

            var cardController = new CardController();
            cardController.SetVisualElement(cardElement, "character-name", "character-class", "character-portrait");
            cardController.SetGunData(gun);

            // If rowContainer is not initialized or column limit is reached, create a new row
            if (rowContainer == null || i % columnCount == 0)
            {
                rowContainer = new VisualElement();
                rowContainer.style.flexDirection = FlexDirection.Row;
                rowContainer.style.flexWrap = Wrap.Wrap;
                rowContainer.style.justifyContent = Justify.FlexStart;
                m_GunScrollview.Add(rowContainer); // Add the new row to the ScrollView
            }

            rowContainer.Add(cardElement); // Add the card to the current row
            i++;

            // Reset the rowContainer after reaching columnCount
            if (i % columnCount == 0)
            {
                rowContainer = null; // Prepare to create a new row
            }
        }

        // Add the last row if it has remaining cards
        if (rowContainer != null && rowContainer.childCount > 0 && i % columnCount != 0)
        {
            m_GunScrollview.Add(rowContainer);
        }
    }
    private void FillCharactersScrollview(List<CharacterData> characters)
    {
        VisualElement rowContainer = null; // Container for a row
        int columnCount = 3; // Number of cards per row
        int i = 0; // Counter for columns

        foreach (var character in characters)
        {
            // Create and configure a card element for the gun
            var cardElement = m_CardEntryTemplate.Instantiate();
            cardElement.style.flexGrow = 1;

            var cardController = new CardController();
            cardController.SetVisualElement(cardElement, "character-name", "character-class", "character-portrait");
            cardController.SetCharacterData(character);

            // If rowContainer is not initialized or column limit is reached, create a new row
            if (rowContainer == null || i % columnCount == 0)
            {
                rowContainer = new VisualElement();
                rowContainer.style.flexDirection = FlexDirection.Row;
                rowContainer.style.flexWrap = Wrap.Wrap;
                rowContainer.style.justifyContent = Justify.FlexStart;
                m_CharacterScrollview.Add(rowContainer); // Add the new row to the ScrollView
            }

            rowContainer.Add(cardElement); // Add the card to the current row
            i++;

            // Reset the rowContainer after reaching columnCount
            if (i % columnCount == 0)
            {
                rowContainer = null; // Prepare to create a new row
            }
        }

        // Add the last row if it has remaining cards
        if (rowContainer != null && rowContainer.childCount > 0 && i % columnCount != 0)
        {
            m_CharacterScrollview.Add(rowContainer);
        }
    }
    void OnCharacterSelected(IEnumerable<object> selectedItems)
    {
        // Get the currently selected item directly from the ListView
        var selectedCharacter = m_OptionListview.selectedItem as CharacterSO;
    }
}