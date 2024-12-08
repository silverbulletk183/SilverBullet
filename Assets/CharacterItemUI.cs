using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private Button btnBuy;
    [SerializeField] private Button btnSelect;
    [SerializeField] private RawImage img;
    [SerializeField] private TextMeshProUGUI txtSelected;
    private Character character;


    public static CharacterItemUI Instance {  get; private set; }
    private void Awake()
    {
      
        Instance = this;
        btnBuy.onClick.AddListener(() =>
        {
            ShopUI.Instance.ShowLoadingUI(true);
            if(UserData.Instance.gold>=character.price)
            {
                UserData.Instance.gold -= character.price;
                StartCoroutine(CallAPIBuy.Instance.UpdateGoldUser());
                StartCoroutine(CallAPIBuy.Instance.PostUserCharacter(character._id));
                showBtnSelect();
                ShopMessageUI.Instance.ShowMessage("Buy character sussecfully");
            }
            else
            {
                ShopMessageUI.Instance.ShowMessage("Buy character failed");
            }
            
            
          
        });
        btnSelect.onClick.AddListener(() => 
        {
            CallAPISelect.instance.userSelected.id_character= character._id;
            StartCoroutine(CallAPISelect.instance.UpdateUserSelected());
            resetAllSelected();
            showtxtSelect();
        });
    }
    public void SetupCharacterData(Character character)
    {
        this.character = character;
        txtName.text = character.name;
        txtPrice.text= character.price+"";
       
        StartCoroutine(UploadAndDisplayImage.Instance.LoadImage(APIURL.CharacterImage+character._id, img));
        
    }
    public void showBtnSelect()
    {
        btnBuy.gameObject.SetActive(false);
        btnSelect.gameObject.SetActive(true);
    }
    public void showtxtSelect()
    {
        btnBuy.gameObject.SetActive(false);
        btnSelect.gameObject.SetActive(false);
        txtSelected.gameObject.SetActive(true);
    }
    public void checkAlreadyBought()
    {
        if (CallAPIBuy.Instance.userCharacters.Count == 0)
        {
            Debug.Log("list character null");
        }
        List<UserCharacter> list= CallAPIBuy.Instance.userCharacters;
        
        foreach (UserCharacter _character in list)
        {
            if (_character.id_character == character._id)
            {
                showBtnSelect();
            }
        }
    }
    public void checkSelected()
    {

        if (CallAPISelect.instance.userSelected.id_character == character._id)
        {
           showtxtSelect();
        }
    }
    public void resetAllSelected()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("CharacterItem");
        foreach (GameObject _item in items)
        {
            Debug.Log("ddsdss");
           
                if (_item.GetComponent<CharacterItemUI>().character._id != CallAPISelect.instance.userSelected.id_character)
            {
                showBtnSelect();
            }
        }
    }

}
