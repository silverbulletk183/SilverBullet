using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class CallAPIAuthencation : MonoBehaviour
{
    
    public GameObject Loading;
    [SerializeField] private  GameObject pnNameAcc;
    [SerializeField] private GameObject pnDN;
    [SerializeField] private InputField ipfnameAcc;
    [SerializeField] private Button submitButton;

    

  

    public InputField usernamerg, passwordrg, confimpasswordrg;
    

    public static CallAPIAuthencation Intance {  get; private set; }
    private void Awake()
    {
        Intance = this;
    }

    

   public IEnumerator Login(string username, string password)
{
    // 1. Kiểm tra đầu vào
    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
    {
        AuthencationUI.Instance.showMess("Vui lòng nhập đầy đủ thông tin!");
        yield break; // Dừng thực hiện hàm
    }

    // if (username.Length < 6 || password.Length < 6)
    // {
    //     messRG.text = "Tên đăng nhập và mật khẩu phải có ít nhất 6 ký tự!";
    //     yield break; // Dừng thực hiện hàm
    // }

    // 2. Tạo form dữ liệu
    WWWForm form = new WWWForm();
    form.AddField("username", username);
    form.AddField("email", username);
    form.AddField("password", password);
    
    // 3. Gửi yêu cầu API
    using (UnityWebRequest request = UnityWebRequest.Post(APIURL.UserLogin, form))
    {
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        yield return request.SendWebRequest();

        // 4. Xử lý kết quả phản hồi
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Request Successful: " + request.downloadHandler.text);

            try
            {
                var jsonData = JsonUtility.FromJson<ApiResponse>(request.downloadHandler.text);
                if (jsonData.status == 200)
                {
                    UserData.Instance.userId = jsonData.data._id;
                    UserData.Instance.nameAcc = jsonData.data.nameAcc;
                    UserData.Instance.gold = jsonData.data.gold;
                    UserData.Instance.level = jsonData.data.score;
                    UserData.Instance.isActive = jsonData.data.active;

                    Loader.Load(Loader.Scene.mainHomecp);
                }
                else
                {
                    AuthencationUI.Instance.showMess("Tên đăng nhập hoặc mật khẩu không chính xác!");
                }
            }
            catch (System.Exception e)
            {
                AuthencationUI.Instance.showMess("Phản hồi từ máy chủ không hợp lệ!");
            }
        }
        else
        {
            AuthencationUI.Instance.showMess("Lỗi kết nối đến máy chủ");
        }
    }
}


   public void register()
{
    // Gọi phương thức validate trước khi thực hiện đăng ký
    ValidateAndRegister();
}

  IEnumerator PostRegister(string usernamerg, string passwordrg, string nameAcc)
{
    WWWForm form = new WWWForm();
    form.AddField("username", usernamerg);
    form.AddField("email", usernamerg + "@gmail.com");
    form.AddField("password", passwordrg);
    form.AddField("nameAcc", nameAcc); // Thêm tên tài khoản vào form

    using (UnityWebRequest request = UnityWebRequest.Post(APIURL.UserRegiter, form))
    {
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        yield return request.SendWebRequest();

        Loading.SetActive(false); // Ẩn Loading Panel khi yêu cầu hoàn tất

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Request Successful: " + request.downloadHandler.text);

            var jsonData = JsonUtility.FromJson<ApiResponse>(request.downloadHandler.text);

            if (jsonData.status == 200)
            {
                AuthencationUI.Instance.showMess("Đăng ký thành công!");
                pnDN.SetActive(true);
                pnNameAcc.SetActive(false);
            }
            else
            {
                    AuthencationUI.Instance.showMess("Đăng ký thất bại");
            }
        }
        else
        {
            Debug.LogError("Request Failed: " + request.error);
            AuthencationUI.Instance.showMess("Lỗi kết nối đến máy chủ!");
        }
    }
}

public void ValidateAndRegister()
{
    string urg = usernamerg.text.Trim(); // Bỏ khoảng trắng 2 đầu
    string prg = passwordrg.text.Trim();
    string cfprg = confimpasswordrg.text.Trim();
    string nameAcc = ipfnameAcc.text.Trim(); // Lấy và loại bỏ khoảng trắng của nameAcc

    // Kiểm tra các trường trống
    if (string.IsNullOrEmpty(urg) || string.IsNullOrEmpty(prg) || string.IsNullOrEmpty(cfprg))
    {
        AuthencationUI.Instance.showMess("Vui lòng nhập đầy đủ thông tin!");
        return;
    }

    // Kiểm tra độ dài username và password (tối thiểu 6 ký tự)
    if (urg.Length < 6 || prg.Length < 6)
    {
        AuthencationUI.Instance.showMess("Tên đăng nhập và mật khẩu phải có ít nhất 6 ký tự!");
        return;
    }

    // Kiểm tra xác nhận mật khẩu có khớp với mật khẩu không
    if (prg != cfprg)
    {
        AuthencationUI.Instance.showMess("Mật khẩu xác nhận không khớp!");
        return;
    }

    // Sau khi validate thành công, hiển thị Loading panel và gọi phương thức đăng ký
    Loading.SetActive(true);
    StartCoroutine(PostRegister(nameAcc, urg, prg));
}


    public void LoadingPN()
    {
        pnNameAcc.SetActive(true);
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
