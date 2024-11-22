using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class GetDataNV : MonoBehaviour
{
    public GameObject dataRowPrefab;   // Prefab cho m?i d�ng d? li?u (Kh�ng ch?a n�t)
    public Transform content;          // N?i ch?a c�c d�ng d? li?u
    public Button buyButtonPrefab;     // Prefab c?a n�t "Buy" (Button kh�ng c� trong dataRowPrefab)
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
        public string image;  // ID h�nh ?nh
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

                // X�a danh s�ch c? trong Singleton
                NVData.Instance.ClearCharacters();

                foreach (var data in response.data)
                {
                    // T?o v� l?u th�ng tin nh�n v?t v�o Singleton
                    Character character = new Character(data.name, data.price, data.image);
                    NVData.Instance.AddCharacter(character);

                    // Hi?n th? d? li?u l�n UI
                    GameObject dataRow = Instantiate(dataRowPrefab, content);
                    dataRow.transform.Find("NameNV").GetComponent<Text>().text = data.name;
                    dataRow.transform.Find("PriceNV").GetComponent<Text>().text = data.price.ToString();

                    // Debug ID ?? ki?m tra
                    Debug.Log("id_character: " + data._id);

                    // T?o n�t "Buy" t? prefab v� g�n s? ki?n ngo�i prefab
                    Button buyButton = Instantiate(buyButtonPrefab, dataRow.transform); // T?o Button t? prefab ngo�i
                    buyButton.onClick.AddListener(() => StartCoroutine(TransferData(data._id, dataRow)));

                    // Hi?n th? ?nh l�n RawImage
                    RawImage rawImage = dataRow.transform.Find("ImageNV").GetComponent<RawImage>(); // T�m RawImage trong prefab
                    string imageUrl = baseImageUrl + data.image;  // X�y d?ng URL h�nh ?nh t? ID
                    StartCoroutine(LoadImage(imageUrl, rawImage));  // T?i v� hi?n th? ?nh
                }
            }
            else
            {
                Debug.LogError("Error fetching data: " + request.error);
            }
        }
    }

    // Coroutine ?? t?i ?nh t? URL v� g�n v�o RawImage
    IEnumerator LoadImage(string url, RawImage rawImage)
    {
        Debug.Log("Loading image from URL: " + url); // In ra URL ?? ki?m tra

        using (UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return imageRequest.SendWebRequest();

            if (imageRequest.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)imageRequest.downloadHandler).texture;
                rawImage.texture = texture;  // G�n texture cho RawImage
                rawImage.SetNativeSize();  // Thi?t l?p k�ch th??c ph� h?p
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
                Destroy(dataRow);  // X�a d�ng d? li?u sau khi mua
            }
            else
            {
                Debug.LogError("Error transferring data: " + request.error);
            }
        }
    }
}
