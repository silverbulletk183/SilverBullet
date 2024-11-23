using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance { get; private set; }
    [SerializeField] private Button btnBack;
    [SerializeField] private Button btnCharacter;
    [SerializeField] private Button btnGun;
    [SerializeField] private TextMeshProUGUI txtGold;
    [SerializeField] private TextMeshProUGUI txtLV;
    [SerializeField] private GameObject characterPanel;
    [SerializeField] private GameObject gunPanel;
    private string hexColor = "#FFCF00";
    private void Awake()
    {
        Instance = this;
        btnBack.onClick.AddListener(() =>
        {

        });
        btnCharacter.onClick.AddListener(() =>
        {
            ShowCharacterPanel();
        });
        btnGun.onClick.AddListener(() =>
        {
            ShowGunPanel();
        });
        ShowCharacterPanel();
    }
    public void SetGoldText(string text)
    {
        txtGold.text = text;
    }
    public void SetLVText(string text) 
    {  
        txtLV.text = text;
    }
    public void ShowCharacterPanel()
    {
        characterPanel.SetActive(true);
        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            // Gán màu mới cho Image
            btnGun.GetComponent<Image>().color = Color.white;
            btnCharacter.GetComponent<Image>().color = newColor;
        }
        
        gunPanel.SetActive(false);
    }
    public void ShowGunPanel()
    {
        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            // Gán màu mới cho Image
            btnCharacter.GetComponent<Image>().color = Color.white;
            btnGun.GetComponent<Image>().color = newColor;
        }
        gunPanel.SetActive(true) ;
        characterPanel.SetActive(false) ;
    }
}
