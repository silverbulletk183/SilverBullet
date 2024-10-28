using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class AuthManager : MonoBehaviour
{
    private string registerUrl = "http://localhost:3000/api/user";
    private string loginUrl = "http://localhost:3000/api/user/login";

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
}
