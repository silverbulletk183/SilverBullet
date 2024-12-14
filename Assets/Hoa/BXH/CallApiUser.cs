using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TextCore.Text;

public class CallApiUser : MonoBehaviour
{
    // Start is called before the first frame update
    public static CallApiUser Instance { get; private set; }
    private string apiUrl = "http://localhost:3000/api/user";
    private List<User> list;
    private void Awake()
    {
        Instance = this;
    }
    public IEnumerator GetUser()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            // G?i yêu c?u ??n API
            yield return request.SendWebRequest();

            // Ki?m tra l?i
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                // Nh?n ph?n h?i JSON
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Response: " + jsonResponse);

                // Chuy?n ??i JSON thành ??i t??ng C#
                ApiResponseUser response = JsonUtility.FromJson<ApiResponseUser>(jsonResponse);

                if (response != null && response.status == 200)
                {
                    list = new List<User>();
                    list = response.data.ToList();
                    UserUI.Instance.PopulateShop(list);
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
public class User
{
    public string _id;
    public string level;
    public string image;
    public string updatedAt;
}

[System.Serializable]
public class ApiResponseUser
{
    public int status;
    public User[] data;
}
