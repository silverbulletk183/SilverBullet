using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CallAPIAuthencation : MonoBehaviour
{
    public TextMeshProUGUI messRG;
    public GameObject Loading;

  

    public InputField usernamerg, passwordrg, confimpasswordrg;
    private string apiUrlRG = "https://silverbulletapi.onrender.com/api/user";




    private string apiUrl= "https://silverbulletapi.onrender.com/api/user";

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
        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl+"/login", form))
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
        var urg = usernamerg.text;
        var prg = passwordrg.text;
        var cfprg = confimpasswordrg.text;

        if (prg != cfprg)
        {

            messRG.text = "Không trùng Password";
            Debug.Log("không trùng pass");

        }
        else
        {
            string jsonRegister = "{\"username\":\"" + urg + "\",\"password\":\"" + prg + "\"}";
            StartCoroutine(PostRegister(jsonRegister));
        }
        Loading.SetActive(true);
    }


    IEnumerator PostRegister(string jsonRegister)
    {
        // Tạo request
        using (UnityWebRequest request = new UnityWebRequest(apiUrlRG, "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            // Gửi dữ liệu JSON
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRegister);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            // Đợi phản hồi từ server
            yield return request.SendWebRequest();
            Loading.SetActive(true);
            // Kiểm tra lỗi
            if (request.result != UnityWebRequest.Result.Success)
            {
                Loading.SetActive(false);
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                // Xử lý phản hồi thành công
                Loading.SetActive(false);
                //  Debug.Log("Response: " + request.downloadHandler.text);
                string json = request.downloadHandler.text;
                // Debug.Log("Response: " + responseJson);
                try
                {
                    ApiResponse responseData = JsonUtility.FromJson<ApiResponse>(json);
                    if (responseData.status == 200)
                    {
                        Loading.SetActive(false);
                        // đây là nơi để chạy lệnh khi mà đăng kí thành công
                        Debug.Log("Đăng ký thành công");
                       // loginpanel.SetActive(true);
                       // Regispanel.SetActive(false);
                    }
                    else
                    {
                        Loading.SetActive(false);
                        messRG.text = "Tên đăng nhập đã được sử dụng";
                        Debug.LogWarning("API response status is not 200: " + responseData.status);
                    }
                }
                catch (System.Exception e)
                {
                    Loading.SetActive(false);
                    Debug.LogError("Error parsing JSON response: " + e.Message);
                }

            }
            request.Dispose();
        }
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
