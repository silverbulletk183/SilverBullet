using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopScreen : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset m_ListEntryTemplate;
    [SerializeField]
    VisualTreeAsset m_CardEntryTemplate;

    void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        // Initialize the character list controller
        var characterListController = new CharacterListController();
        if (m_ListEntryTemplate == null || m_CardEntryTemplate == null)
        {
            Debug.LogError("UXML templates are null. Check assignments.");
            return;
        }
        if (uiDocument == null)
        {
            Debug.Log("uiDocument is null");
        }
        characterListController.InitializeCharacterList(uiDocument.rootVisualElement, m_ListEntryTemplate, m_CardEntryTemplate);
    }
}
