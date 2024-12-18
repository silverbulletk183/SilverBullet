using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class AuthencationUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TMP_InputField edtUserName;
    [SerializeField] private TMP_InputField edtPassword;
    [SerializeField] private Button btnLogin;
    [SerializeField] private Button btnCreateAcc;
    [SerializeField] private Button btnBackLgin;
    [SerializeField] private GameObject pnDN;
    [SerializeField] private GameObject pnDK;
    [SerializeField] private TextMeshProUGUI messRG;


    public static AuthencationUI Instance {  get; private set; }
    private void Awake()
    {
        Instance = this;
        btnLogin.onClick.AddListener(() =>
        {
            string userName= edtUserName.text;
            string password= edtPassword.text;
            StartCoroutine(CallAPIAuthencation.Intance.Login(userName, password));
        });
        btnCreateAcc.onClick.AddListener(() =>
        {
            hideMess();
            pnDK.SetActive(true);
            pnDN.SetActive(false);

        });
        btnBackLgin.onClick.AddListener(() =>
        {
            hideMess();
            pnDK.SetActive(false);
            pnDN.SetActive(true);
        });

       
    }
    public void showMess(string txt)
    {

        messRG.text = txt;
        messRG.gameObject.SetActive(true);
    }
    public void hideMess()
    {
        messRG.gameObject.SetActive(false);
    }

}
