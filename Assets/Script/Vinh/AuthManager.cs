using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class AuthManager : MonoBehaviour
{
    private string requestUrl = "link sever";
    private string loginUrl = "link";

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

        UnityWebRequest www = UnityWebRequest.Post(requestUrl, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.LogError(www.error);
        }
    }
    public IEnumerator LoginUser(string username, string password)
    {

        //Helper class to generate form data to post to web servers using the UnityWebRequest or WWW classes.
        WWWForm form = new WWWForm();

        form.AddField("username", username);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post(requestUrl, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.LogError(www.error);
        }
    }
}
