using UnityEngine.UIElements;
using UnityEngine;

public class TabbedMenu : MonoBehaviour
{
    private TabbedMenuController controller;
    VisualElement root;
    public TabbedMenu(VisualElement root)
    {
        this.root = root;
    }
    public void Initialize()
    {
        controller = new(root);

        controller.RegisterTabCallbacks();
    }
}
