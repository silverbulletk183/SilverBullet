using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GunDataPanel : BindableElement
{
    readonly TemplateContainer templateContainer;
    public new class UxmlFactory : UxmlFactory<GunDataPanel>
    {

    }
    public GunDataPanel()
    {
        templateContainer = Resources.Load<VisualTreeAsset>("GunDataPanel").Instantiate();
        templateContainer.style.flexGrow = 1;
        hierarchy.Add(templateContainer);
    }
}
