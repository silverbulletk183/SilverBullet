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
    private string url = "https://silverbulletapi.onrender.com/api/characterimage?id=";
    
    public static CharacterItemUI Instance {  get; private set; }
    private void Awake()
    {
        Instance = this;
        btnBuy.onClick.AddListener(() =>
        {
            btnSelect.gameObject.SetActive(true);
            string characterId = character._id; 
            StartCoroutine(CallAPIBuy.Instance.SendUserCharacter(characterId));
            Destroy(btnBuy.gameObject);
        });
        btnSelect.onClick.AddListener(() => 
        { 
        
        });
    }
    private Character character;
    public void SetupCharacterData(Character character)
    {
        this.character = character;
        txtName.text = character.name;
        txtPrice.text= character.price+"";
       
        StartCoroutine(UploadAndDisplayImage.Instance.LoadImage("characterimage?id="+character._id, img));
        
    }
    void BuyCharacter(string characterId)
    {
        StartCoroutine(CallAPIBuy.Instance.SendUserCharacter(characterId));
    }

}
