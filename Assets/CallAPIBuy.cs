using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class CallAPIBuy : MonoBehaviour
{
    public static CallAPIBuy Instance { get; private set; }
    public IEnumerator SendUserCharacter(string characterId)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
    {
        { "characterId", characterId }
    };
      
        string jsonPayload = JsonUtility.ToJson(payload);

        // T?o y�u c?u POST
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm("https://silverbulletapi.onrender.com/api/usercharacter", jsonPayload))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonPayload));
            request.downloadHandler = new DownloadHandlerBuffer();

            // G?i y�u c?u ??n API
            yield return request.SendWebRequest();

            // Ki?m tra ph?n h?i
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error sending user character: " + request.error);
            }
            else
            {
                Debug.Log("Response from server: " + request.downloadHandler.text);
            }
        }
    }

}
