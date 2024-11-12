using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class GetData : MonoBehaviour
{
    public GameObject dataRowPrefab;
    public Transform content;
    string imageUrl = "http://localhost:3000/characterimage";

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
        public string image_url; // Tr??ng m?i ?? ch?a ???ng d?n ?nh
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
                    GameObject dataRow = Instantiate(dataRowPrefab, content);

                    // C?p nh?t tên và giá
                    dataRow.transform.Find("Name").GetComponent<Text>().text = data.name;
                    dataRow.transform.Find("Price").GetComponent<Text>().text = data.price.ToString();

                    // Debug ID ?? ki?m tra
                    Debug.Log("id_character: " + data._id);

                    // C?p nh?t nút mua
                    Button transferButton = dataRow.transform.Find("Buy").GetComponent<Button>();
                    transferButton.onClick.AddListener(() => StartCoroutine(TransferData(data._id, dataRow)));

                    // Ki?m tra và t?i ?nh n?u có
                    if (!string.IsNullOrEmpty(data.image_url))
                    {
                        StartCoroutine(LoadImage(data.image_url, dataRow.transform.Find("Image").GetComponent<Image>()));
                    }
                }
            }
            else
            {
                Debug.LogError("Error fetching data: " + request.error);
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
                Destroy(dataRow);
            }
            else
            {
                Debug.LogError("Error transferring data: " + request.error);
            }
        }
    }

    // Ph??ng th?c t?i ?nh t? URL
    IEnumerator LoadImage(string imageUrl, Image imageComponent)
    {
        using (UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return imageRequest.SendWebRequest();

            if (imageRequest.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(imageRequest);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                imageComponent.sprite = sprite; // Gán ?nh vào Image component
            }
            else
            {
                Debug.LogError("Error loading image: " + imageRequest.error);
            }
        }
    }
}
