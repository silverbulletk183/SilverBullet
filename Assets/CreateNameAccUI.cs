using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

public class CreateNameAccUI : MonoBehaviour
{
    public static CreateNameAccUI Instance {  get; private set; }
    [SerializeField] private TMP_InputField txtNameAcc;
    [SerializeField] private Button btnOk;
    [SerializeField] private TextMeshProUGUI txtError;
    [SerializeField] private GameObject loadingPN;

    private void Awake()
    {

        Instance = this;
        Hide();
        btnOk.onClick.AddListener(() =>
        {
            if (txtNameAcc.text == " "|| string.IsNullOrEmpty(txtNameAcc.text))
            {
                ShowTxtError("Account name is  empty");
            }
            else if(txtNameAcc.text.Length>10){
                ShowTxtError("Account name cannot be longer than 10 characters");
            }
            else
            {
                IsShowLoad(true);
                StartCoroutine(CallAPIUser.Instance.UpdateNameAcc(txtNameAcc.text));
            }
        });

    }
    private void Start()
    {
        if (UserData.Instance.isActive == true)
        {
            IsShow(false);
        }
    }
    public void IsShow(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
    public void Hide()
    {
        txtError.gameObject.SetActive(false);
    }
    public void ShowTxtError(string txtMess) {
        txtError.text = txtMess;
        txtError.gameObject.SetActive(true);
    }
    public void IsShowLoad(bool isshow)
    {
        gameObject.SetActive(isshow);
    }
   

}
