 using UnityEngine;
 using UnityEngine.UI;
 using UnityEngine.Networking;  // D�ng cho UnityWebRequest
 using System.Collections;
 using TMPro;


 public class mainHomeUI : MonoBehaviour
 {
     public Text nameAccText;
     public TMP_Text goldText;
     public TMP_Text levelText;
     public RawImage avt;
    


     void Start()
     {
        // L?y th�ng tin ng??i d�ng t? UserData
      
         if (UserData.Instance != null)
         {
             nameAccText.text = UserData.Instance.nameAcc;
             goldText.text =  UserData.Instance.gold.ToString();
             levelText.text = UserData.Instance.level.ToString();
             StartCoroutine(UploadAndDisplayImage.Instance.LoadImage("userimage?id="+UserData.Instance.userId, avt));
            //   N?u c� URL h�nh ?nh, t?i v� hi?n th?

         }
     }

 }
