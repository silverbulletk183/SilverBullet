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
    public event EventHandler buyFailed;

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

            }
            else
            {
                 buyFailed?.Invoke(this, EventArgs.Empty);
            }
            
            
          
        });
        btnSelect.onClick.AddListener(() => 
        {
            CallAPISelect.instance.userSelected.id_character= character._id;
            StartCoroutine(CallAPISelect.instance.UpdateUserSelected());
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
        List<UserCharacter> list= CallAPIBuy.Instance.userCharacters;
        foreach (UserCharacter _character in list)
        {
            if (_character.id_character == character._id)
            {
                showBtnSelect();
            }
        }
    }
   

}
