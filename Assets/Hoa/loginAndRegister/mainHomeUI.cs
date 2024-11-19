using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking; // Dùng cho UnityWebRequest
using System.Collections;
using TMPro;


public class mainHomeUI : MonoBehaviour
{
    public Text nameAccText;
    public TMP_Text goldText;
    public TMP_Text levelText;
  

    void Start()
    {
        // L?y thông tin ng??i dùng t? UserData
        if (UserData.Instance != null)
        {
            nameAccText.text = "Name: " + UserData.Instance.nameAcc;
            goldText.text = "Gold: " + UserData.Instance.gold.ToString();
            levelText.text = "Level: " + UserData.Instance.level.ToString();

            // N?u có URL hình ?nh, t?i và hi?n th?
            
        }
    }

    // Hàm t?i ?nh avatar t? URL
    //IEnumerator LoadAvatar(string imageUrl)
    //{
    //    using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result == UnityWebRequest.Result.Success)
    //        {
    //            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //            avatarImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    //        }
    //        else
    //        {
    //            Debug.Log("Failed to load avatar: " + www.error);
    //        }
    //    }
    //}
}
