using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopScreen : UIScreen
{
    VisualTreeAsset m_ListEntryTemplate;
    VisualTreeAsset m_CardEntryTemplate;

    public ShopScreen(VisualElement root) : base(root)
    {
    }
    public override void SetVisualElements()
    {
        base.SetVisualElements();
        StoreEvent.StoreUpdated += LoadData;
    }
    public override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();

    }
    void LoadData(VisualTreeAsset m_ListEntryTemplate, VisualTreeAsset m_CardEntryTemplate)
    {
        var characterListController = new CharacterListController();
        if (m_ListEntryTemplate == null || m_CardEntryTemplate == null)
        {
            Debug.LogError("UXML templates are null. Check assignments.");
            return;
        }
        characterListController.InitializeCharacterList(root, m_ListEntryTemplate, m_CardEntryTemplate);
    }
}
