using UnityEngine;
using UnityEngine.UIElements;

public class EquipGun : MonoBehaviour
{
    public GameObject[] weapons;
    public GameObject currentGun;
    public UIDocument inventoryUI;
    private bool isVisible = false;

    // Buttons for weapon selection
    private Button ak47Btn;
    private Button m4a1Btn;

    private void Awake()
    {
        inventoryUI = GetComponent<UIDocument>();
        // Hide inventory UI initially
        inventoryUI.rootVisualElement.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        // Cache button elements and bind weapon selection functions
        ak47Btn = inventoryUI.rootVisualElement.Q<Button>("Ak47-btn");
        m4a1Btn = inventoryUI.rootVisualElement.Q<Button>("M4a1-btn");

        ak47Btn.clicked += () => SelectWeapon(0);
        m4a1Btn.clicked += () => SelectWeapon(1);
    }

    private void Update()
    {
        // Toggle inventory UI visibility with the Tab key
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //Switch on off
            ToggleUI();
        }
    }

    private void SelectWeapon(int index)
    {
        // Deactivate the current weapon if selected
        if (currentGun != null)
        {
            currentGun.SetActive(false);
        }

        // Activate the chosen weapon
        currentGun = weapons[index];
        currentGun.SetActive(true);
    }

    private void ToggleUI()
    {
        // Toggle inventory UI visibility
        isVisible = !isVisible;
        inventoryUI.rootVisualElement.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
    }
}
