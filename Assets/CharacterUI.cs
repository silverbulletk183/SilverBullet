using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    // Start is called before the first frame update
    public static CharacterUI Instance {  get; private set; }
    [SerializeField ] private GameObject characterItem;
    [SerializeField ] private Transform content;
    private void Awake()
    {
        Instance = this;
    }
    public void PopulateShop(List<Character> characters)
    {
        foreach (var character in characters)
        {
            GameObject item = Instantiate(characterItem, content);
            CharacterItemUI ui = item.GetComponent<CharacterItemUI>();
            if (ui != null)
            {
                Debug.Log("set characterdata"+character);
                ui.SetupCharacterData(character);
                
                ui.checkAlreadyBought();
                ui.checkSelected();
              
                
            }
        }

        ShopUI.Instance.ShowLoadingUI(false);
    }
}
