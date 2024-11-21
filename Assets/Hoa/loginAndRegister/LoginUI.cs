using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoginUI : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Button loginButton;

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    void OnLoginButtonClicked()
    {
        StartCoroutine(Login(usernameInput.text, passwordInput.text));
    }

    IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", username);
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post("https://silverbulletapi.onrender.com/api/user/login", form))
        {
            www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.responseCode == 200)
                {
                    Debug.Log("Login Successful!");
                    Debug.Log(www.downloadHandler.text);

                    // Parse JSON
                    var jsonData = JsonUtility.FromJson<ResponseData>(www.downloadHandler.text);

                    if (jsonData.active) // Ki?m tra tr?ng th�i active
                    {
                       
                        // L?u th�ng tin ng??i d�ng v�o UserData singleton
                        UserData.Instance.userId = jsonData.data._id;
                        UserData.Instance.username = jsonData.data.username;
                        UserData.Instance.imageUrl = jsonData.data.avt; // Gi? s? 'avt' l� URL h�nh ?nh
                        UserData.Instance.nameAcc = jsonData.data.nameAcc;
                        UserData.Instance.gold = jsonData.data.gold;
                        UserData.Instance.level = jsonData.data.score; // Gi? s? 'score' l� level*/

                        // Chuy?n sang scene kh�c
                        Loader.Load(Loader.Scene.mainHomecp);
                    }
                    else
                    {
                        Debug.Log("User account is not active.");
                    }
                }
                else
                {
                    Debug.Log("Login failed. Check your credentials.");
                }
            }
        }
    }

}



[System.Serializable]
public class ResponseData
{
    public int status;
    public bool active;
    public User data;
}
[System.Serializable]
public class User
{
    public string _id;        // User ID
    public string username;
    public string password;
    public string nameAcc;
    public string email;
    public int gold;
    public int score;
    public bool active;
    public string avt;        // Avatar URL (or ID)
    public string createdAt;
    public string updatedAt;
    public int __v;
}
