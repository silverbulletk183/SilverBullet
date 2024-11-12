using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopController : MonoBehaviour
{
    [SerializeField] VisualTreeAsset m_ListEntryTemplate;
    [SerializeField] VisualTreeAsset m_CardEntryTemplate;
    private void Start()
    {
        StoreEvent.StoreUpdated?.Invoke(m_ListEntryTemplate,m_CardEntryTemplate);
    }
}
