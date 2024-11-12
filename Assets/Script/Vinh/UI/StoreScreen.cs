using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class StoreScreen : UIScreen
{
    VisualTreeAsset m_ListEntryTemplate;
    VisualTreeAsset m_CardEntryTemplate;

    public StoreScreen(VisualElement root) : base(root)
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
        var StoreScreenController = new StoreScreenController();
        if (m_ListEntryTemplate == null || m_CardEntryTemplate == null)
        {
            Debug.LogError("UXML templates are null. Check assignments.");
            return;
        }
        StoreScreenController.Initialize(root, m_ListEntryTemplate, m_CardEntryTemplate);
    }
}
