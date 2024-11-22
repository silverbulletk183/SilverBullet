using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class GetData : MonoBehaviour
{
    public GameObject dataRowPrefab; // Prefab ch?a giao di?n c?a t?ng nh�n v?t
    public Transform content;        // V�ng hi?n th? danh s�ch
    public string baseImageUrl = "http://localhost:3000/api/characterimage?id="; // ???ng d?n c? s? ?? t?i ?nh

    [System.Serializable]
    public class ApiResponse
    {
        public string status;
        public List<Data> data;
    }

    [System.Serializable]
    public class Data
    {
        public string _id;       // ID c?a nh�n v?t
        public string name;      // T�n nh�n v?t
        public float price;      // Gi� c?a nh�n v?t
        public string image_url; // URL ?nh c?a nh�n v?t
    }

    private void Start()
    {
        WWWForm form = new WWWForm();
      //  form.AddField("email", username);
       // form.AddField("username", username);
        //form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post("https://silverbulletapi.onrender.com/api/user/login", form))
        StartCoroutine(FetchData());
    }

    IEnumerator FetchData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("https://silverbulletapi.onrender.com/api/character"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(request.downloadHandler.text);

                foreach (var data in response.data)
                {
                    // T?o m?t h�ng giao di?n m?i t? prefab
                    GameObject dataRow = Instantiate(dataRowPrefab, content);

                    // Hi?n th? t�n
                    dataRow.transform.Find("NameNV").GetComponent<Text>().text = "Name: " + data.name;

                    // Hi?n th? gi�
                    dataRow.transform.Find("PriceNV").GetComponent<Text>().text = "Price: $" + data.price;

                  

                    // Hi?n th? ?nh
                    RawImage avatarImage = dataRow.transform.Find("ImageNV").GetComponent<RawImage>();
                    string imageUrl = baseImageUrl + data._id; // URL t?i ?nh d?a tr�n ID
                    StartCoroutine(LoadImage(imageUrl, avatarImage));
                }
            }
            else
            {
                Debug.LogError("Error fetching data: " + request.error);
            }
        }
    }

    IEnumerator LoadImage(string url, RawImage avatarImage)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        // Ch? t?i ?nh v? t? URL
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            // Hi?n th? ?nh tr�n UI RawImage
            avatarImage.texture = texture;

            // ?i?u ch?nh k�ch th??c ?nh ?? ph� h?p v?i RawImage
            ScaleImageToFitRawImage(texture, avatarImage);
        }
        else
        {
            Debug.LogError("Error loading image: " + request.error);
        }
    }

    private void ScaleImageToFitRawImage(Texture2D texture, RawImage avatarImage)
    {
        RectTransform rt = avatarImage.GetComponent<RectTransform>();
        float rawImageWidth = rt.rect.width;
        float rawImageHeight = rt.rect.height;

        float imageWidth = texture.width;
        float imageHeight = texture.height;

        float rawImageAspect = rawImageWidth / rawImageHeight;
        float imageAspect = imageWidth / imageHeight;

        if (imageAspect > rawImageAspect)
        {
            float scale = rawImageWidth / imageWidth;
            rt.sizeDelta = new Vector2(rawImageWidth, imageHeight * scale);
        }
        else
        {
            float scale = rawImageHeight / imageHeight;
            rt.sizeDelta = new Vector2(imageWidth * scale, rawImageHeight);
        }
    }
}
