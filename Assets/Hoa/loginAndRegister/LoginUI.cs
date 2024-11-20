using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class GetData : MonoBehaviour
{
    public GameObject dataRowPrefab; // Prefab ch?a giao di?n c?a t?ng nhân v?t
    public Transform content;        // Vùng hi?n th? danh sách
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
        public string _id;       // ID c?a nhân v?t
        public string name;      // Tên nhân v?t
        public float price;      // Giá c?a nhân v?t
        public string image_url; // URL ?nh c?a nhân v?t
    }

    private void Start()
    {
        StartCoroutine(FetchData());
    }

    IEnumerator FetchData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/api/character"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(request.downloadHandler.text);

                foreach (var data in response.data)
                {
                    // T?o m?t hàng giao di?n m?i t? prefab
                    GameObject dataRow = Instantiate(dataRowPrefab, content);

                    // Hi?n th? tên
                    dataRow.transform.Find("NameNV").GetComponent<Text>().text = "Name: " + data.name;

                    // Hi?n th? giá
                    dataRow.transform.Find("PriceNV").GetComponent<Text>().text = "Price: $" + data.price;

                  

                    // Hi?n th? ?nh
                    RawImage avatarImage = dataRow.transform.Find("ImageNV").GetComponent<RawImage>();
                    string imageUrl = baseImageUrl + data._id; // URL t?i ?nh d?a trên ID
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

            // Hi?n th? ?nh trên UI RawImage
            avatarImage.texture = texture;

            // ?i?u ch?nh kích th??c ?nh ?? phù h?p v?i RawImage
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
