using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class AuthManager : MonoBehaviour
{
    private string registerUrl = "http://localhost:3000/api/user";

    private string loginUrl = "http://localhost:3000/api/user/login";



    public IEnumerator RegisterUser (string username, string password, string repassword, string email)
    {
        if (password != repassword)
        {
            Debug.LogWarning("Password is not match");
            yield break;
        }

        //Helper class to generate form data to post to web servers using the UnityWebRequest or WWW classes.
        WWWForm form = new WWWForm();

        form.AddField ("username", username);
        form.AddField ("password", password);
        form.AddField ("email", email);

        UnityWebRequest www = UnityWebRequest.Post(registerUrl, form);
        yield return www.SendWebRequest();



        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Registration error: " + www.error);
        }
        else
        {
            GameEvent.RegisterSuccessful.Invoke();
            Debug.Log("Registration successful: " + www.downloadHandler.text);
        }
    }
    public IEnumerator LoginUser(string username, string password)
    {

        //Helper class to generate form data to post to web servers using the UnityWebRequest or WWW classes.
        WWWForm form = new WWWForm();

        form.AddField("username", username);
        form.AddField("password", password);
        Debug.Log(form);
        Debug.Log($"Attempting to log in with Username: {username} and Password: {password}");
        UnityWebRequest www = UnityWebRequest.Post(loginUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Login error: " + www.error);
        }
        else
        {
            GameEvent.LoginSuccessful?.Invoke();
            Debug.Log("Login successful: " + www.downloadHandler.text);
        }
    }

}
