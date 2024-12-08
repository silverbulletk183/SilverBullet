using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class CallAPIAuthencation : MonoBehaviour
{
    public TextMeshProUGUI messRG;
    public GameObject Loading;
    [SerializeField] private  GameObject pnNameAcc;
    [SerializeField] private GameObject pnDN;
    [SerializeField] private InputField ipfnameAcc;
    [SerializeField] private Button submitButton;

    

  

    public InputField usernamerg, passwordrg, confimpasswordrg;
    private string apiUrlRG = "http://localhost:3000/api/user";/*"https://silverbulletapi.onrender.com/api/user";*/


    public static CallAPIAuthencation Intance {  get; private set; }
    private void Awake()
    {
        Intance = this;
    }

    

    public IEnumerator Login(string username, string password)
    {
        // Create a new WWWForm object
        WWWForm form = new WWWForm();

        // Add form fields (key-value pairs)
        form.AddField("username",username);
        form.AddField("email", username);
        form.AddField("password", password);
        Debug.Log(username+password);
        // Create a UnityWebRequest for the POST request with form data
        using (UnityWebRequest request = UnityWebRequest.Post(APIURL.UserLogin, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            // Send the request and wait for a response
            yield return request.SendWebRequest();

            // Check for errors in the response
            if (request.result == UnityWebRequest.Result.Success)
            {
                // On success, print the response data
                Debug.Log("Request Successful: " + request.downloadHandler.text);
                var jsonData = JsonUtility.FromJson<ApiResponse>(request.downloadHandler.text);
                if (jsonData.status == 200)
                {
                    UserData.Instance.userId = jsonData.data._id;
                    UserData.Instance.nameAcc = jsonData.data.nameAcc;
                    UserData.Instance.gold = jsonData.data.gold;
                    UserData.Instance.level = jsonData.data.score;
                    Loader.Load(Loader.Scene.mainHomecp);
                }
                else
                {
                    Debug.Log("login false");
                }
            }
            else
            {
                // On failure, print the error message
                Debug.LogError("Request Failed: " + request.error);
            }
        }
         
    }


    public void register()
    {
        string urg = usernamerg.text;
        string prg = passwordrg.text;
        string cfprg = confimpasswordrg.text;
        string nameAcc = ipfnameAcc.text; // Lấy tên tài khoản

        if (string.IsNullOrEmpty(urg) || string.IsNullOrEmpty(prg) || string.IsNullOrEmpty(cfprg) || string.IsNullOrEmpty(nameAcc))
        {
            messRG.text = "Vui lòng nhập đầy đủ thông tin";
            return;
        }

        if (prg != cfprg)
        {
            messRG.text = "Mật khẩu xác nhận không khớp!";
            return;
        }

        // Gọi API đăng ký
        StartCoroutine(PostRegister(urg, prg, nameAcc));
    }

    IEnumerator PostRegister(string usernamerg, string passwordrg, string nameAcc)
    {
        Loading.SetActive(true);

        WWWForm form = new WWWForm();
        form.AddField("username", usernamerg);
        form.AddField("email", usernamerg + "@gmail.com");
        form.AddField("password", passwordrg);
        form.AddField("nameAcc", nameAcc); // Thêm tên tài khoản vào form

        using (UnityWebRequest request = UnityWebRequest.Post(apiUrlRG, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            yield return request.SendWebRequest();

            Loading.SetActive(false);

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Request Successful: " + request.downloadHandler.text);

                var jsonData = JsonUtility.FromJson<ApiResponse>(request.downloadHandler.text);

                if (jsonData.status == 200)
                {
                    messRG.text = "Đăng ký thành công!";
                    // Chuyển cảnh khi đăng ký thành công
                    pnDN.SetActive(true);
                    pnNameAcc.SetActive(false);
                }
                else
                {
                    messRG.text = "Đăng ký thất bại: " + jsonData.status;
                }
            }
            else
            {
                Debug.LogError("Request Failed: " + request.error);
                messRG.text = "Lỗi kết nối đến máy chủ!";
            }
        }
    }

    public void LoadingPN()
    {
        pnNameAcc.SetActive(true);
    }


}



[System.Serializable]
public class ApiResponse
{
    public int status;
    public bool active;
    public UserModel data;
}


[System.Serializable]
public class UserModel
{
    public string _id;
    public string username;
    public string password;
    public string nameAcc;
    public string email;
    public int gold;
    public int score;
    public bool active;
    public string avt;
    public string createdAt;
    public string updatedAt;
    public int __v;
}
