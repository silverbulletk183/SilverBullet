using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json; // Nếu sử dụng Newtonsoft.Json

public class CallAPIUser : MonoBehaviour
{
    public static CallAPIUser Instance { get; private set; }
    private string apiUrl = "https://silverbulletapi.onrender.com/api/user";
    private List<User> list;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public IEnumerator GetUser()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error + " | Response: " + request.downloadHandler.text);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Raw API response: " + jsonResponse); // In phản hồi

                try
                {
                    ApiResponseUser response = JsonConvert.DeserializeObject<ApiResponseUser>(jsonResponse);
                    if (response != null && response.status == 200)
                    {
                        list = response.data.ToList();
                        UserUI.Instance?.PopulateShop(list);
                    }
                    else
                    {
                        Debug.LogError("Failed to parse API response or status is not 200.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Exception while parsing API response: " + ex.Message);
                }
            }
        }
    }
    public IEnumerator UpdateNameAcc(string _nameAcc)
    {
        UserModel userModel = new UserModel
        {
            nameAcc = _nameAcc,
            active=true,
            _id = UserData.Instance.userId
        };


        // Chuyển đổi đối tượng thành JSON
        string jsonData = JsonUtility.ToJson(userModel);
        Debug.Log(jsonData);
        using (UnityWebRequest request = UnityWebRequest.Put(APIURL.UserUpdate,jsonData))
        {
            request.SetRequestHeader("Content-Type", "application/json"); // Change from x-www-form-urlencoded to application/json

            yield return request.SendWebRequest();

         

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Request Successful: " + request.downloadHandler.text);

                var jsonDataRes = JsonUtility.FromJson<ApiResponseUserUpdate>(request.downloadHandler.text);

                if (jsonDataRes.status == 200)
                {
                    UserData.Instance.isActive = true;
                    StartCoroutine(CallAPISelect.instance.CreateUserSelect());
                    StartCoroutine(CallAPIBuy.Instance.PostUserCharacter("67623547fb3842809839df4f",false));
                    StartCoroutine(CallAPIBuy.Instance.PostUserGun("6761a495a4c7ddb682084f0e",false));
                    StartCoroutine(showNotication());
                    
                }
                else
                {
                    CreateNameAccUI.Instance.ShowTxtError("Create account name Failed!");
                }
            }
            else
            {
                CreateNameAccUI.Instance.ShowTxtError("Can not connect to server!");
            }
        }
    }
   public IEnumerator showNotication()
    {
        CreateNameAccUI.Instance.IsShowLoad(false);
        CreateNameAccUI.Instance.ShowTxtError("You have received 1 character and 1 weapon");
        yield return new WaitForSeconds(5);
        Loader.Load(Loader.Scene.Instruct);

    }
}

[System.Serializable]
public class User
{
    public string _id;
    public string username;
    public string password;
    public string nameAcc;
    public string email;
    public int gold;
    public int score;
    public bool active;
    public DateTime? deleteDate; // Sửa thành DateTime? 
    public string avt;
    public string updatedAt;
}

[System.Serializable]
public class ApiResponseUser
{
    public int status;
    public string message;
    public User[] data;
}

