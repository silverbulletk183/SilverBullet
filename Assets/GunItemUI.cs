using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class GunItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private Button btnBuy;
    [SerializeField] private Button btnSelect;
    [SerializeField] private RawImage img;
    [SerializeField] private TextMeshProUGUI txtSelected;
    private Gun gun;
   
    public static GunItemUI Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        btnBuy.onClick.AddListener(() =>
        {
            if (UserData.Instance.gold >= gun.price)
            {
                UserData.Instance.gold -= gun.price;
                ShopUI.Instance.ShowLoadingUI(true);
                StartCoroutine(CallAPIBuy.Instance.UpdateGoldUser());
                StartCoroutine(CallAPIBuy.Instance.PostUserGun(gun._id));
                showBtnSelect();
                ShopMessageUI.Instance.ShowMessage("Buy gun sussecfully");
            }
            else
            {
                ShopMessageUI.Instance.ShowMessage("Buy gun failed");
            }
            
        });
       btnSelect.onClick.AddListener(() =>
        {
            CallAPISelect.instance.userSelected.id_gun = gun._id;
            StartCoroutine(CallAPISelect.instance.UpdateUserSelected());
            showtxtSelect();
        });
    }
    public void SetupGunData(Gun gun)
    {
        this.gun = gun;
        txtName.text = gun.name;
        txtPrice.text = gun.price + "";

        StartCoroutine(UploadAndDisplayImage.Instance.LoadImage(APIURL.GunImage+ gun._id, img));
        
    }
    public void showtxtSelect()
    {
        btnBuy.gameObject.SetActive(false);
        btnSelect.gameObject.SetActive(false);
        txtSelected.gameObject.SetActive(true);
    }
    public void showBtnSelect()
    {
        btnBuy.gameObject.SetActive(false);
        btnSelect.gameObject.SetActive(true);
    }
    public void checkAlreadyBought()
    {
        List<UserGun> list = CallAPIBuy.Instance.userGuns;
        foreach (UserGun _gun in list)
        {
            if (_gun.id_gun == gun._id)
            {
                showBtnSelect();
            }
        }
    }

}
