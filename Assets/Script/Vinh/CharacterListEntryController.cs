using System.Collections.Generic;
using UnityEngine.UIElements;

public class CharacterListEntryController
{
    Label m_NameLabel;
    public void SetVisualElement(VisualElement visualElement)
    {
        m_NameLabel = visualElement.Q<Label>("character-name");
    }
    public void SetCharacterData(string option)
    {
        m_NameLabel.text = option;
    }
}