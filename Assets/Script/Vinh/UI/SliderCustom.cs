using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SliderCustom : MonoBehaviour
{
    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var sliders = root.Query<UnityEngine.UIElements.Slider>().ToList(); 

        foreach (var slider in sliders)
        {
            var label = slider.Q<Label>();
            if (label != null)
            {
                label.text = slider.value.ToString();

                slider.RegisterValueChangedCallback(evt =>
                {
                    label.text = evt.newValue.ToString();
                });
            }
        }
    }
}
