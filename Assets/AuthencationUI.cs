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

        });
    }

}
