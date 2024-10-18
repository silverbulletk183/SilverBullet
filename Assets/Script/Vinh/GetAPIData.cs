using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetAPIData : MonoBehaviour
{
    public string URL = "http://localhost:3000/api/gun";
    public void GetData()
    {
        StartCoroutine(FetchData());
    }

    public IEnumerator FetchData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }else
            {
                GunList gunList = new GunList();
                gunList = JsonUtility.FromJson<GunList>(request.downloadHandler.text);

                foreach (Gun gun in gunList.guns)
                {
                    Debug.Log($"Name: {gun.name}, Damage: {gun.damage}");
                }
            }
        }
    }
}
