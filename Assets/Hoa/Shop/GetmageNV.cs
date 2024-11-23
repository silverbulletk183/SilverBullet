using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class GetImageNV : MonoBehaviour
{
    public static GetImageNV Instance { get; private set; }
    public RawImage rawImage; // RawImage ?? hi?n th? ?nh
    private string apiImageUrl = "http://localhost:3000/api/characterimage"; // ???ng d?n API l?y ?nh nh�n v?t

    void Start()
    {
        // Thay th? "characterId" b?ng ID c?a nh�n v?t mu?n t?i ?nh
        string characterId = "671f8c1ada47fa8041063785"; // V� d? ID
        StartCoroutine(GetImageNV.Instance.LoadImageById(characterId));
    }


    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// G?i API ?? t?i ?nh nh�n v?t d?a tr�n ID.
    /// </summary>
    /// <param name="characterId">ID c?a nh�n v?t</param>
    public IEnumerator LoadImageById(string characterId)
    {
        // T?o URL ??y ?? v?i ID
        string url = $"{apiImageUrl}?id={characterId}";
        Debug.Log($"?ang t?i ?nh t?: {url}");

        // G?i y�u c?u t?i ?nh
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // L?y texture t? ph?n h?i
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            // Hi?n th? texture tr�n RawImage
            rawImage.texture = texture;

            // ?i?u ch?nh k�ch th??c ?nh ?? v?a v?i RawImage
            ScaleImageToFitRawImage(rawImage, texture);

            Debug.Log("T?i ?nh th�nh c�ng!");
        }
        else
        {
            Debug.LogError($"L?i khi t?i ?nh: {request.error}");
        }
    }

    /// <summary>
    /// ?i?u ch?nh k�ch th??c ?nh ?? ph� h?p v?i RawImage m� kh�ng thay ??i k�ch th??c RawImage.
    /// </summary>
    /// <param name="rawImage">RawImage ?? hi?n th?</param>
    /// <param name="texture">Texture c?a ?nh</param>
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
}
