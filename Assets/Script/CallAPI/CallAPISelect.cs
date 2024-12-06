using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class CallAPISelect : MonoBehaviour
{
    public static CallAPISelect instance {  get; private set; }
    public UserSelected userSelected;
    
    private void Awake()
    {
        instance = this;
    }
    public IEnumerator UpdateUserSelected()
    {
        // Tạo một đối tượng chứa dữ liệu cần gửi
        UserSelected _userSelected= new UserSelected
        {
            id_user=UserData.Instance.userId,
            id_character=userSelected.id_character,
            id_gun=userSelected.id_gun,
        };
        // Chuyển đổi đối tượng thành JSON
        string jsonData = JsonUtility.ToJson(_userSelected);
       // byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Tạo yêu cầu PUT
        using (UnityWebRequest request = UnityWebRequest.Put(APIURL.UpdateUserSelect, jsonData))
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
                Debug.Log("Response: " + jsonResponse);

                // Xử lý phản hồi nếu cần
                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    try
                    {
                        // Chuyển đổi phản hồi JSON thành đối tượng C#
                        ApiResponseUserUpdate response = JsonUtility.FromJson<ApiResponseUserUpdate>(jsonResponse);

                        if (response != null && response.status == 200)
                        {
                            
                        }
                        else
                        {
                            Debug.LogError("Failed to parse API response.");
                        }
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
    public IEnumerator GetUserSelected()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(APIURL.GetUserSelect + UserData.Instance.userId))
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
                

                // Chuyển đổi JSON thành đối tượng C#
                ApiResponseUserSelected response = JsonUtility.FromJson<ApiResponseUserSelected>(jsonResponse);

                if (response != null && response.status == 200)
                {
                    
                    userSelected = new UserSelected();
                    userSelected = response.data;
                }
                else
                {
                    Debug.LogError("Failed to parse API response.");
                }
            }

        }
    }
}

[System.Serializable]
public class UserSelected
{
    public string id_user;
    public string id_character;
    public string id_gun;
    
}

[System.Serializable]
public class ApiResponseUserSelected
{
    public int status;
    public UserSelected data;
}
