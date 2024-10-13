using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UISetting : MonoBehaviour
{
    Button closeBtn;
    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        closeBtn = root.Q<Button>("close-btn");
        closeBtn.clicked += () => GameEvent.SettingsClosed?.Invoke();
    }
}
