using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;


public class RegisterScript : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public InputField returnPasswordInput; 
    public InputField NamePlayer;
    public Button registerButton;

    private string registerUrl = "http://localhost:3000/api/user"; 

    void Start()
    {
        registerButton.onClick.AddListener(() => StartCoroutine(RegisterUser()));
    }

    IEnumerator RegisterUser()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;
        string returnPassword = returnPasswordInput.text;
        string namePlayer = NamePlayer.text; // L?y d? li?u t? tr??ng NamePlayer

        // Ki?m tra nh?p li?u
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
            string.IsNullOrEmpty(returnPassword) || string.IsNullOrEmpty(namePlayer))
        {
            Debug.Log("Vui lòng nh?p ??y ?? thông tin!");
            yield break;
        }

        // Ki?m tra m?t kh?u kh?p
        if (password != returnPassword)
        {
            Debug.Log("M?t kh?u không kh?p, vui lòng nh?p l?i!");
            yield break;
        }

        // T?o form d? li?u g?i lên server
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("namePlayer", namePlayer); // G?i thêm d? li?u NamePlayer

        UnityWebRequest request = UnityWebRequest.Post(registerUrl, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("??ng ký thành công!");
            SceneManager.LoadScene("mainHomecp"); // Chuy?n qua Scene A
        }
        else
        {
            Debug.Log($"L?i: {request.downloadHandler.text}");
        }
    }

}
