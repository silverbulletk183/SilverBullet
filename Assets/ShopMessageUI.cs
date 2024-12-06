using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopMessageUI : MonoBehaviour
{
    [SerializeField] private Button btnOk;
    [SerializeField] private TextMeshProUGUI txtMessage;

    public static ShopMessageUI Instance {  get; private set; }
    private void Awake()
    {
        Instance = this;
        btnOk.onClick.AddListener(() =>
        {
            HideMessage();
        });
        HideMessage();
    }
    public void ShowMessage(string mess)
    {
        txtMessage.text = mess;
        gameObject.SetActive(true);
    }
    public void HideMessage()
    {
        gameObject.SetActive(false);
    }
}
