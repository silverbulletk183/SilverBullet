using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using Unity.Services.Relay.Models;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

public class AuthManager : MonoBehaviour
{
    public LevelSO level;
    private string registerUrl = "http://localhost:3000/api/user";
    private string loginUrl = "http://localhost:3000/api/user/login";
    private string updateUrl = "http://localhost:3000/api/user";
    private string gunsUrl = "http://localhost:3000/api/gun";
    private string imageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT9JLxC73TpfJyOtIMrVmSLcfrOpJdFsLNlFA&s";
    private static AuthManager _instance;
    public static AuthManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject); // Giữ lại đối tượng này qua các scene
        }
    }
    public void StartUpdateUser(string nameAcc)
    {
        StartCoroutine(UpdateUser(nameAcc));
    }
    public async Task<List<GunData>> FetchGunAsync()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(gunsUrl))
        {
            request.timeout = 10;
            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(request.downloadHandler.text);
                return response.data;
            }
            else
            {
                Debug.LogError("Failed to fetch guns: " + request.error);
                return null;
            }
        }
    }

    public IEnumerator UpdateUser(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("nameAcc", username);
        form.AddField("active", "true");
        form.AddField("id", "670e87c6b46aa6a17f6caf3d");
        var www = UnityWebRequest.Post(updateUrl, form);
        www.method = "PUT";
        yield return www.SendWebRequest();
        // Check for errors
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            Debug.Log("Update successful: " + www.downloadHandler.text);
        }
    }

    public IEnumerator RegisterUser(string username, string password, string repassword, string email, Action<bool, string> callback)
    {
        if (password != repassword)
        {
            callback(false, "Passwords do not match.");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("email", email);

        using (UnityWebRequest www = UnityWebRequest.Post(registerUrl, form))
        {
            yield return www.SendWebRequest();

            HandleResponse(www, callback, "Registration");
        }
    }

    public IEnumerator LoginUser(string username, string password, Action<bool, string> callback)
    {
        WWWForm form = new();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(loginUrl, form))
        {
            yield return www.SendWebRequest();

            HandleResponse(www, callback, "Login");
        }
    }
    IEnumerator LoadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        // Chờ tải về ảnh từ URL
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Lấy texture từ phản hồi
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            // Hiển thị texture lên UI RawImage
            level.thumbnail = texture;
        }
        else
        {
            Debug.LogError("Lỗi khi tải ảnh: " + request.error);
        }
    }

    private void HandleResponse(UnityWebRequest www, Action<bool, string> callback, string action)
    {
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            callback(false, $"{action} error: {www.error}");
            return;
        }

        int statusCode = (int)www.responseCode;
        Debug.Log(www.result +(int)www.responseCode + ": " + statusCode);

        switch (statusCode)
        {
            case 200:
                Debug.Log(www.downloadHandler.text);
                var response = JsonUtility.FromJson<ResponseLogin>(www.downloadHandler.text);
                HomeEvent.IsUserActive?.Invoke(response.active);
                callback(true, $"{action} successful.");
                if (action == "Registration") GameEvent.RegisterSuccessful?.Invoke();
                else if (action == "Login") GameEvent.LoginSuccessful?.Invoke();
                break;
            case 401:
                callback(false, "Unauthorized: Invalid username or password.");
                GameEvent.LoginFailed?.Invoke();
                break;
            case 404:
                callback(false, "Not Found: The requested resource does not exist.");
                GameEvent.LoginFailed?.Invoke();
                break;
            default:
                callback(false, $"{action} failed: Unexpected error.");
                GameEvent.LoginFailed?.Invoke();
                break;
        }
    }
    public class ResponseLogin
    {
        public bool active;
    }
    public class ApiResponse
    {
        public string status { get; set; }
        public List<GunData> data { get; set; }
    }

}
