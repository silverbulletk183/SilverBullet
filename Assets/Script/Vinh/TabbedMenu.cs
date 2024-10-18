using UnityEngine.UIElements;
using UnityEngine;

public class TabbedMenu : MonoBehaviour
{
    private TabbedMenuController controller;

    public void Initialize()
    {
        UIDocument menu = GetComponent<UIDocument>();
        VisualElement root = menu.rootVisualElement;

        controller = new(root);

        controller.RegisterTabCallbacks();
    }
}
