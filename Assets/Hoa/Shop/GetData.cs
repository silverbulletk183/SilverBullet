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

                    dataRow.transform.Find("Name").GetComponent<Text>().text = data.name;
                    dataRow.transform.Find("Price").GetComponent<Text>().text = data.price.ToString();

                    Debug.Log("id_character: " + data._id);

                    Button transferButton = dataRow.transform.Find("Buy").GetComponent<Button>();
                    transferButton.onClick.AddListener(() => StartCoroutine(TransferData(data._id, dataRow)));
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
}
