using UnityEngine.UIElements;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private Label nameLabel;
    private Label classLabel;
    private VisualElement portraitVisualElement;

    public void SetVisualElement(VisualElement cardElement, string nameLabel, string nameClassLabel, string namePortraitVisualelement)
    {
        this.nameLabel = cardElement.Q<Label>(nameLabel);
        classLabel = cardElement.Q<Label>(nameClassLabel);
        portraitVisualElement = cardElement.Q<VisualElement>(namePortraitVisualelement);
    }
    public void SetGunData(GunData gunData)
    {
        nameLabel.text = gunData.name;
        //portraitVisualElement.style.backgroundImage = new StyleBackground(gunData.gunSprite);
        classLabel.text = gunData.damage.ToString();
    }
    public void SetCharacterData(CharacterData characterData)
    {
        nameLabel.text = characterData.name;
        //portraitVisualElement.style.backgroundImage = new StyleBackground(characterData.PortraitImage);
        classLabel.text = characterData.price.ToString();
    }


}