using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class GetDataNV : MonoBehaviour
{
    public GameObject dataRowPrefab;   // Prefab cho m?i dòng d? li?u (Không ch?a nút)
    public Transform content;          // N?i ch?a các dòng d? li?u
    public Button buyButtonPrefab;     // Prefab c?a nút "Buy" (Button không có trong dataRowPrefab)
    string baseImageUrl = "https://silverbulletapi.onrender.com/api/characterimage?id=";  // URL g?c n?i ?nh ???c l?u tr?

    [System.Serializable]
    public class ApiResponse
    {
        public string status;
        public List<Data> data;
    }

    [System.Serializable]
    public class Data
    {
        public string _id;
        public string name;
        public float price;
        public string image;  // ID hình ?nh
    }

    private void Start()
    {
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

                // Xóa danh sách c? trong Singleton
                NVData.Instance.ClearCharacters();

                foreach (var data in response.data)
                {
                    // T?o và l?u thông tin nhân v?t vào Singleton
                    Character character = new Character(data.name, data.price, data.image);
                    NVData.Instance.AddCharacter(character);

                    // Hi?n th? d? li?u lên UI
                    GameObject dataRow = Instantiate(dataRowPrefab, content);
                    dataRow.transform.Find("NameNV").GetComponent<Text>().text = data.name;
                    dataRow.transform.Find("PriceNV").GetComponent<Text>().text = data.price.ToString();

                    // Debug ID ?? ki?m tra
                    Debug.Log("id_character: " + data._id);

                    // T?o nút "Buy" t? prefab và gán s? ki?n ngoài prefab
                    Button buyButton = Instantiate(buyButtonPrefab, dataRow.transform); // T?o Button t? prefab ngoài
                    buyButton.onClick.AddListener(() => StartCoroutine(TransferData(data._id, dataRow)));

                    // Hi?n th? ?nh lên RawImage
                    RawImage rawImage = dataRow.transform.Find("ImageNV").GetComponent<RawImage>(); // Tìm RawImage trong prefab
                    string imageUrl = baseImageUrl + data.image;  // Xây d?ng URL hình ?nh t? ID
                    StartCoroutine(LoadImage(imageUrl, rawImage));  // T?i và hi?n th? ?nh
                }
            }
            else
            {
                Debug.LogError("Error fetching data: " + request.error);
            }
        }
    }

    // Coroutine ?? t?i ?nh t? URL và gán vào RawImage
    IEnumerator LoadImage(string url, RawImage rawImage)
    {
        Debug.Log("Loading image from URL: " + url); // In ra URL ?? ki?m tra

        using (UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return imageRequest.SendWebRequest();

            if (imageRequest.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)imageRequest.downloadHandler).texture;
                rawImage.texture = texture;  // Gán texture cho RawImage
                rawImage.SetNativeSize();  // Thi?t l?p kích th??c phù h?p
            }
            else
            {
                Debug.LogError("Error loading image: " + imageRequest.error);
            }
        }
    }

    IEnumerator TransferData(string id_character, GameObject dataRow)
    {
        if (string.IsNullOrEmpty(id_character))
        {
            Debug.LogError("id_character is null or empty!");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("id_character", id_character);

        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/api/usercharacter", form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data transferred successfully.");
                Destroy(dataRow);  // Xóa dòng d? li?u sau khi mua
            }
            else
            {
                Debug.LogError("Error transferring data: " + request.error);
            }
        }
    }
}
