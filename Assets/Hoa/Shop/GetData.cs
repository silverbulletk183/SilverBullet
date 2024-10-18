using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;  

[System.Serializable]
public class Product
{
    public string _id;   
    public string name;  
    public int gia;      
}

[System.Serializable]
public class ProductData
{
    public int status;     
    public Product[] data; 
}

public class GetData : MonoBehaviour
{
    public GameObject productPrefab; 
    public Transform contentPanel;   
    public Text jsonDisplay; 

    public void Start()
    {
        StartCoroutine(GetDataFromNodeJS());
    }

    IEnumerator GetDataFromNodeJS()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/api/character");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("D? li?u nh?n ???c: " + json);

            
            ProductData productData = JsonUtility.FromJson<ProductData>(json);

            
            foreach (Transform child in contentPanel)
            {
                Destroy(child.gameObject);
            }

           
            foreach (var product in productData.data)
            {
                GameObject newProduct = Instantiate(productPrefab, contentPanel);
                Text productText = newProduct.GetComponentInChildren<Text>();
                productText.text = $"Tên: {product.name}\nGiá: {product.gia}";

            }
        }
        else
        {
            jsonDisplay.text = "Error: " + request.error; 
            Debug.LogError("L?i: " + request.error);
        }
    }
}
