using UnityEngine.UIElements;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private Label characterNameLabel;
    private Label characterClassLabel;
    private VisualElement characterPortrait;

    public void SetVisualElement(VisualElement cardElement)
    {
        if (cardElement == null)
        {
            Debug.LogError("Card element is null. Check UXML template and assignment.");
            return;
        }

        characterNameLabel = cardElement.Q<Label>("character-name");
        characterClassLabel = cardElement.Q<Label>("character-class");
        characterPortrait = cardElement.Q<VisualElement>("character-portrait");

        // Ensure the elements are found
        if (characterNameLabel == null || characterClassLabel == null || characterPortrait == null)
        {
            Debug.LogError("Could not find elements in card template. Check UXML and element names.");
            return;
        }
    }
    public void SetGunData(GunSO gunData)
    {
        characterNameLabel.text = gunData.gunName;
        characterPortrait.style.backgroundImage = new StyleBackground(gunData.gunSprite);
        characterClassLabel.text = gunData.gunClass.ToString();
    }


}