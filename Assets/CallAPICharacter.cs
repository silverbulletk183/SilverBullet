using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class CallAPICharacter : MonoBehaviour
{
    // Start is called before the first frame update
   public static CallAPICharacter Instance {  get; private set; }
    private string apiUrl = "https://silverbulletapi.onrender.com/api/character";
    public List<Character> list;
    private void Awake()
    {
        Instance = this;
    }
    public IEnumerator GetCharacter()
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
                ApiResponseCharacter response = JsonUtility.FromJson<ApiResponseCharacter>(jsonResponse);

                if (response != null && response.status == 200)
                {
                    list = new List<Character>();
                    list = response.data.ToList();
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
public class Character
{
    public string _id;
    public string name;
    public int price;
    public string image;
    public string updatedAt;
}

[System.Serializable]
public class ApiResponseCharacter
{
    public int status;
    public Character[] data;
}
