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
            pnDK.SetActive(true);
            pnDN.SetActive(false);
        });
        btnBackLgin.onClick.AddListener(() =>
        {
            pnDK.SetActive(false);
            pnDN.SetActive(true);
        });

       
    }

}
