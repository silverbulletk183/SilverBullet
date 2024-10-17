using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class UIScreen : MonoBehaviour
{
    public VisualElement root;
    public abstract void Initialize();
    public virtual void Show()
    {
        if (root != null)
        {
            root.style.display = DisplayStyle.Flex;
        }
    }
    public virtual void Hide()
    {
        if (root != null)
        {
            root.style.display = DisplayStyle.None;
        }
    }
}
