using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterListController
{
    // UXML template for list entries
    VisualTreeAsset m_ListEntryTemplate;
    VisualTreeAsset m_CardEntryTemplate;

    // UI element references
    ListView m_CharacterList;
    ScrollView m_GunScrollview;


    List<GunSO> m_AllGuns;
    List<string> m_OptionsList = new List<string>() { "Súng", "Nhân vật", "Vật phẩm", "Hòm đồ" };

    public void InitializeCharacterList(VisualElement root, VisualTreeAsset listElementTemplate, VisualTreeAsset cardElementTemplate)
    {
        EnumerateAllGuns();

        // Store a reference to the template for the list entries
        m_ListEntryTemplate = listElementTemplate;
        m_CardEntryTemplate = cardElementTemplate;

        // Store a reference to the character list element
        m_CharacterList = root.Q<ListView>("character-list");
        m_GunScrollview = root.Q<ScrollView>("gun-scrollview");
        if (m_GunScrollview == null)
        {
            Debug.LogError("GunScrollview not found. Check the ID in the UXML file.");
            return;
        }

        //Scrollview



        FillCharacterList();
        FillScrollView();

        // Register to get a callback when an item is selected
        m_CharacterList.selectionChanged += OnCharacterSelected;
    }

    void EnumerateAllGuns()
    {
        m_AllGuns = new List<GunSO>();
        var guns = Resources.LoadAll<GunSO>("Guns");
        m_AllGuns.AddRange(guns);
    }


    void FillCharacterList()
    {
        // Set up a make item function for a list entry
        m_CharacterList.makeItem = () =>
        {
            // Instantiate the UXML template for the entry
            var newListEntry = m_ListEntryTemplate.Instantiate();

            // Instantiate a controller for the data
            var newListEntryLogic = new CharacterListEntryController();

            // Assign the controller script to the visual element
            newListEntry.userData = newListEntryLogic;

            // Initialize the controller script
            newListEntryLogic.SetVisualElement(newListEntry);

            // Return the root of the instantiated visual tree
            return newListEntry;
        };

        // Set up bind function for a specific list entry
        m_CharacterList.bindItem = (item, index) =>
        {
            (item.userData as CharacterListEntryController)?.SetCharacterData(m_OptionsList[index]);
        };

        m_CharacterList.fixedItemHeight = 45;
        m_CharacterList.itemsSource = m_OptionsList;
    }
    void FillScrollView()
    {
        try
        {
            if (m_AllGuns == null || m_CardEntryTemplate == null)
            {
                Debug.LogError("m_AllCharacters or m_CardEntryTemplate is null.");
                return;
            }

            VisualElement rowContainer = null; // Container cho hàng
            int columnCount = 3; // Số lượng card mỗi hàng

            for (int i = 0; i < m_AllGuns.Count; i++)
            {
                var gunData = m_AllGuns[i];


                var cardElement = m_CardEntryTemplate.Instantiate();
                cardElement.style.flexGrow = 1;
                if (cardElement == null)
                {
                    Debug.LogError("cardElement is null after instantiation.");
                    continue;
                }

                var cardController = new CardController();
                cardController.SetVisualElement(cardElement);
                cardController.SetGunData(gunData);

                // Nếu rowContainer chưa được khởi tạo, tạo mới
                if (rowContainer == null)
                {
                    rowContainer = new VisualElement();
                    rowContainer.style.flexDirection = FlexDirection.Row; // Sắp xếp theo hàng
                    rowContainer.style.flexWrap = Wrap.Wrap; // Gói khi đầy
                    rowContainer.style.justifyContent = Justify.FlexStart; // Căn giữa các card
                    m_GunScrollview.Add(rowContainer); // Thêm vào ScrollView ngay khi khởi tạo
                }

                rowContainer.Add(cardElement); // Thêm card vào hàng

                // Nếu đã đủ 4 card, reset rowContainer
                if ((i + 1) % columnCount == 0)
                {
                    rowContainer = null; // Đặt lại để tạo hàng mới
                }
            }

            // Kiểm tra nếu còn card nào chưa được thêm vào
            if (rowContainer != null && rowContainer.childCount > 0)
            {
                m_GunScrollview.Add(rowContainer);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception in FillScrollView: " + ex.Message);
        }
    }



    void OnCharacterSelected(IEnumerable<object> selectedItems)
    {
        // Get the currently selected item directly from the ListView
        var selectedCharacter = m_CharacterList.selectedItem as CharacterData;
    }
}