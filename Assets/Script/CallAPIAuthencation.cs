using System.Collections;
using System.Collections.Generic;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class CallAPIAuthencation : MonoBehaviour
{
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
