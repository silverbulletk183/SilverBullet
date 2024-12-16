using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BXHUI : MonoBehaviour
{
    public static BXHUI Instance { get; private set; }
    [SerializeField] private Button btnBack;
    [SerializeField] private Button btnCharacter;
    [SerializeField] private Button btnGun;
    [SerializeField] private TextMeshProUGUI txtGold;
    [SerializeField] private TextMeshProUGUI txtLV;
    [SerializeField] private GameObject characterPanel;
    [SerializeField] private GameObject gunPanel;
    [SerializeField] private Transform characterContainer;
    [SerializeField] private Transform gunContainer;
    [SerializeField] GameObject loadingUI;
    private string hexColor = "#FFCF00";
    private void Awake()
    {
        Instance = this;
        btnBack.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.mainHomecp);
        });
        btnCharacter.onClick.AddListener(() =>
        {
            ShowCharacterPanel();
        });
        btnGun.onClick.AddListener(() =>
        {
            ShowGunPanel();
        });
    }
    // private void Start()
    // {
    //     ShowCharacterPanel();
    // }
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
        if (HasChild(characterContainer))
        {
            ShowLoadingUI(true);
           // StartCoroutine(CallAPIBuy.Instance.GetUserCharacter());
            StartCoroutine(CallAPIUser.Instance.GetUser());
           
        }
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
        if (HasChild(gunContainer))
        {
            ShowLoadingUI(true);
            //StartCoroutine(CallAPIBuy.Instance.GetUserGun());
            StartCoroutine(CallAPIGun.Instance.GetGun());
            
        }
    }
    bool HasChild(Transform parent)
    {
        return parent.childCount <= 0;
    }
    public void ShowLoadingUI(bool isShow)
    {
        loadingUI.SetActive(isShow);   
    }
}
