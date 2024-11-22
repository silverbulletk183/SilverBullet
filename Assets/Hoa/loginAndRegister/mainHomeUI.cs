//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Networking; // D�ng cho UnityWebRequest
//using System.Collections;
//using TMPro;


//public class mainHomeUI : MonoBehaviour
//{
//    public Text nameAccText;
//    public TMP_Text goldText;
//    public TMP_Text levelText;
//    public RawImage avt;
  

//    void Start()
//    {
//        // L?y th�ng tin ng??i d�ng t? UserData
//        if (UserData.Instance != null)
//        {
            
            
//            nameAccText.text = UserData.Instance.nameAcc;
//            goldText.text =  UserData.Instance.gold.ToString();
//            levelText.text = UserData.Instance.level.ToString();
//            StartCoroutine(UploadAndDisplayImage.Instance.LoadImage("userimage?id="+UserData.Instance.userId, avt));
//            // N?u c� URL h�nh ?nh, t?i v� hi?n th?
            
//        }
//    }

//    // H�m t?i ?nh avatar t? URL
//    //IEnumerator LoadAvatar(string imageUrl)
//    //{
//    //    using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl))
//    //    {
//    //        yield return www.SendWebRequest();

//    //        if (www.result == UnityWebRequest.Result.Success)
//    //        {
//    //            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
//    //            avatarImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
//    //        }
//    //        else
//    //        {
//    //            Debug.Log("Failed to load avatar: " + www.error);
//    //        }
//    //    }
//    //}
//}
