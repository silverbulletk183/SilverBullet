using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIStartScreen : MonoBehaviour
{
    private Button optionsBtn;
    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        optionsBtn = root.Q<Button>("Options_btn");


        optionsBtn.RegisterCallback<ClickEvent>(evt => GameEvent.OptionButtonOnClick?.Invoke());
    }
    private void OnDisable()
    {
        optionsBtn.clicked -= GameEvent.OptionButtonOnClick;
    }

}
