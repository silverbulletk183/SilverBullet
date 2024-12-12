using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class GetListImage : MonoBehaviour
{
    public string apiUrl = "http://localhost:3000/api/userimage"; // URL API ch?a danh sách ?nh c?a t?t c? ng??i dùng
    public Transform imageContainer; // N?i ?? ch?a các RawImage UI
    public GameObject imagePrefab; // Prefab c?a RawImage ?? hi?n th? ?nh

    private void Start()
    {
        StartCoroutine(LoadImagesFromAPI());
    }

    // Hàm t?i t?t c? các ???ng d?n ?nh t? API và hi?n th? trên giao di?n
    private IEnumerator LoadImagesFromAPI()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("D? li?u t? API: " + request.downloadHandler.text);

            // Gi?i mã d? li?u JSON t? API thành danh sách ???ng d?n ?nh
            List<string> imageUrls = ParseImageUrlsFromJson(request.downloadHandler.text);

            // T?i và hi?n th? t?ng ?nh
            foreach (string imageUrl in imageUrls)
            {
                StartCoroutine(LoadAndDisplayImage(imageUrl));
            }
        }
        else
        {
            Debug.LogError("L?i khi t?i d? li?u t? API: " + request.error);
        }
    }

    // Hàm t?i và hi?n th? m?t ?nh t? URL
    private IEnumerator LoadAndDisplayImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            // T?o ??i t??ng RawImage m?i t? prefab và hi?n th? ?nh
            GameObject newImageObject = Instantiate(imagePrefab, imageContainer);
            RawImage rawImage = newImageObject.GetComponent<RawImage>();
            rawImage.texture = texture;

            // ?i?u ch?nh t? l? hình ?nh cho v?a v?i RawImage
            ScaleImageToFitRawImage(rawImage, texture);
        }
        else
        {
            Debug.LogError("L?i khi t?i ?nh t? URL: " + url + " - " + request.error);
        }
    }

    // Hàm phân tích chu?i JSON và tr? v? danh sách ???ng d?n ?nh
    private List<string> ParseImageUrlsFromJson(string json)
    {
        List<string> imageUrls = new List<string>();

        try
        {
            // Gi?i mã JSON và trích xu?t ???ng d?n ?nh (c?n tùy ch?nh d?a trên c?u trúc JSON API c?a b?n)
            var imagesData = JsonUtility.FromJson<ImagesData>(json);
            foreach (var image in imagesData.images)
            {
                imageUrls.Add(image.url);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("L?i khi phân tích JSON: " + e.Message);
        }

        return imageUrls;
    }

    // Hàm ?? ?i?u ch?nh ?nh sao cho n?m g?n trong RawImage mà không thay ??i kích th??c c?a RawImage
    private void ScaleImageToFitRawImage(RawImage rawImage, Texture2D texture)
    {
        RectTransform rt = rawImage.GetComponent<RectTransform>();
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

    // L?p d? li?u ?? ánh x? v?i JSON tr? v? t? API
    [System.Serializable]
    public class ImageInfo
    {
        public string url; // ???ng d?n URL c?a ?nh
    }

    [System.Serializable]
    public class ImagesData
    {
        public List<ImageInfo> images; // Danh sách các ???ng d?n ?nh c?a t?t c? ng??i dùng
    }
}
