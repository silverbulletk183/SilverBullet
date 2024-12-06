using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


public class CallAPIBuy : MonoBehaviour
{
    public static CallAPIBuy Instance { get; private set; }
    public List<UserCharacter> userCharacters;
    public List<UserGun> userGuns;
   
    

    private void Awake()
    {
        Instance = this;
        
    }
    private void Start()
    {
        StartCoroutine(GetUserCharacter());
    }
    public IEnumerator PostUserCharacter(string characterId)
    {
        WWWForm form = new WWWForm();

        // Add form fields (key-value pairs)
        form.AddField("id_character",characterId);
        form.AddField("id_user", UserData.Instance.userId);

        using (UnityWebRequest request = UnityWebRequest.Post(APIURL.CreateUserCharacter,form))
        {
            // Gửi yêu cầu đến API
            yield return request.SendWebRequest();

            // Kiểm tra lỗi
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                // Nhận phản hồi JSON
                string jsonResponse = request.downloadHandler.text;
               // Debug.Log("Response: " + jsonResponse);

                // Chuyển đổi JSON thành đối tượng C#
               // ApiResponseUserCharacter response = JsonUtility.FromJson<ApiResponseUserCharacter>(jsonResponse);

                if (jsonResponse != null)
                {
                    ShopUI.Instance.ShowLoadingUI(false);
                }
                else
                {
                    Debug.LogError("Failed to parse API response.");
                }
            }

        }
    }
    public IEnumerator GetUserCharacter()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(APIURL.GetUserCharacter+UserData.Instance.userId))
        {
            // Gửi yêu cầu đến API

            yield return request.SendWebRequest();

            // Kiểm tra lỗi
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                // Nhận phản hồi JSON
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Response: " + jsonResponse);

                // Chuyển đổi JSON thành đối tượng C#
                ApiResponseUserCharacter response = JsonUtility.FromJson<ApiResponseUserCharacter>(jsonResponse);

                if (response != null && response.status == 200)
                {
                    userCharacters = new List<UserCharacter>();
                    userCharacters = response.data.ToList();
                    Debug.Log(jsonResponse);
                  //  CharacterItemUI.Instance.checkAlreadyBought(userCharacters);
                }
                else
                {
                    Debug.LogError("Failed to parse API response.");
                }
            }

        }
    }
    public IEnumerator PostUserGun(string gunId)
    {
        WWWForm form = new WWWForm();

        // Add form fields (key-value pairs)
        form.AddField("id_gun", gunId);
        form.AddField("id_user", UserData.Instance.userId);

        using (UnityWebRequest request = UnityWebRequest.Post(APIURL.CreateUserGun, form))
        {
            // Gửi yêu cầu đến API
            yield return request.SendWebRequest();

            // Kiểm tra lỗi
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                // Nhận phản hồi JSON
                string jsonResponse = request.downloadHandler.text;
                // Debug.Log("Response: " + jsonResponse);

                // Chuyển đổi JSON thành đối tượng C#
                // ApiResponseUserCharacter response = JsonUtility.FromJson<ApiResponseUserCharacter>(jsonResponse);

                if (jsonResponse != null)
                {
                    ShopUI.Instance.ShowLoadingUI(false);
                }
                else
                {
                    Debug.LogError("Failed to parse API response.");
                }
            }

        }
    }
    public IEnumerator GetUserGun()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(APIURL.GetUserGun + UserData.Instance.userId))
        {
            // Gửi yêu cầu đến API

            yield return request.SendWebRequest();

            // Kiểm tra lỗi
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                // Nhận phản hồi JSON
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Response: " + jsonResponse);

                // Chuyển đổi JSON thành đối tượng C#
                ApiResponseUserGun response = JsonUtility.FromJson<ApiResponseUserGun>(jsonResponse);

                if (response != null && response.status == 200)
                {
                    userGuns = new List<UserGun>();
                    userGuns = response.data.ToList();
                   
                    //  CharacterItemUI.Instance.checkAlreadyBought(userCharacters);
                }
                else
                {
                    Debug.LogError("Failed to parse API response.");
                }
            }

        }
    }
    public IEnumerator UpdateGoldUser()
    {
        // Tạo một đối tượng chứa dữ liệu cần gửi
        UserModel userModel = new UserModel
        {
            gold= UserData.Instance.gold,
            _id= UserData.Instance.userId
        };
        

        // Chuyển đổi đối tượng thành JSON
        string jsonData = JsonUtility.ToJson(userModel);
        //byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Tạo yêu cầu PUT
      
        using (UnityWebRequest request = UnityWebRequest.Put(APIURL.UserUpdate, jsonData))
        {
            // Đặt kiểu nội dung (Content-Type) là JSON
            request.SetRequestHeader("Content-Type", "application/json");

            // Gửi yêu cầu và chờ phản hồi
            yield return request.SendWebRequest();

            // Kiểm tra kết quả
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                // Lấy phản hồi từ server
                string jsonResponse = request.downloadHandler.text;

                // Xử lý phản hồi nếu cần
                if (!string.IsNullOrEmpty(jsonResponse))
                {
                
                    try
                    {
                        // Chuyển đổi phản hồi JSON thành đối tượng C#
                       /* ApiResponseUserUpdate response = JsonUtility.FromJson<ApiResponseUserUpdate>(jsonResponse);

                        if (response != null && response.status == 200)
                        {
                            Debug.Log("fdfd");
                        }
                        else
                        {
                            Debug.LogError("Failed to parse API response.");
                        }*/
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError("Exception while parsing JSON: " + ex.Message);
                    }
                }
                else
                {
                    Debug.LogError("Empty response from server.");
                }
            }

            // Ẩn giao diện loading
            ShopUI.Instance.ShowLoadingUI(false);
        }
    }

}
[System.Serializable]
public class UserCharacter
{
    public string id_character;
}

[System.Serializable]
public class ApiResponseUserCharacter
{
    public int status;
    public UserCharacter[] data;
}
[System.Serializable]
public class UserGun
{
    public string id_gun;
}

[System.Serializable]
public class ApiResponseUserGun
{
    public int status;
    public UserGun[] data;
}
[System.Serializable]
public class ApiResponseUserUpdate
{
    public int status;
}