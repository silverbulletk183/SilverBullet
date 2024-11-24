using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private Button btnBuy;
    //[SerializeField] private Button btnSelect;
    [SerializeField] private RawImage img;

    public static GunItemUI Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        btnBuy.onClick.AddListener(() =>
        {

        });
      /*  btnSelect.onClick.AddListener(() =>
        {

        });*/
    }
    public void SetupGunData(Gun gun)
    {
        txtName.text = gun.name;
        txtPrice.text = gun.price + "";

        StartCoroutine(UploadAndDisplayImage.Instance.LoadImage("gunimage?id=" + gun._id, img));
        
    }

}
