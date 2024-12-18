using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummaryUI : MonoBehaviour
{
    public static SummaryUI instance {  get; private set; }

    [SerializeField] private Button btnBack;
    [SerializeField] private TextMeshProUGUI txtEX;
    [SerializeField] private TextMeshProUGUI txtGold;
    [SerializeField] private GameObject Win;
    [SerializeField] private GameObject lost;
    [SerializeField] private GameObject Loadding;

    private void Awake()
    {
        instance = this;
        UserData.Instance.gold += UserData.Instance.goldOfMatch;
        UserData.Instance.level += UserData.Instance.levelOfMatch;
        btnBack.onClick.AddListener(() => 
        { 
            showLoad(true);
            Loader.Load(Loader.Scene.mainHomecp);
            StartCoroutine(CallAPIUser.Instance.UpdateGoldAndEX());
            
        });
        setEX("+"+UserData.Instance.levelOfMatch);
        setGold("+"+UserData.Instance.goldOfMatch);
        showResult(UserData.Instance.resultOfMatch);
    }
    public void setEX(string txt)
    {
        txtEX.text = txt;
    }
    public void setGold(string txt)
    {
        txtGold.text = txt;
    }
    public void showResult(bool isShow)
    {
        Win.SetActive(isShow);
        lost.SetActive(!isShow);
    }
    public void showLoad(bool isShow)
    {
        Loadding.SetActive(isShow);
    }

}
