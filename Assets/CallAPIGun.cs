using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class CallAPIGun : MonoBehaviour
{// Start is called before the first frame update
    public static CallAPIGun Instance { get; private set; }
    private string apiUrl = "https://silverbulletapi.onrender.com/api/gun";
    private List<Gun> list;
    private void Awake()
    {
        Instance = this;
    }
    public IEnumerator GetGun()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
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
                Debug.Log("Response: " + jsonResponse);

                // Chuyển đổi JSON thành đối tượng C#
                ApiResponseGun response = JsonUtility.FromJson<ApiResponseGun>(jsonResponse);

                if (response != null && response.status == 200)
                {
                    list = new List<Gun>();
                    list = response.data.ToList();
                    GunUI.Instance.PopulateShop(list);
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
public class Gun
{
    public string _id;
    public string name;
    public int damage;
    public int numberBullet;
    public int price;
    public string image;
    public string updatedAt;
}

[System.Serializable]
public class ApiResponseGun
{
    public int status;
    public Gun[] data;
}