using System.Collections;
using System.Collections.Generic; // Add this to use List<T>
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
    public GameObject product2Prefab;
    public Transform contentPanel;
    public Text jsonDisplay;

    private List<Product> productList = new List<Product>(); // Declare productList

    public void Start()
    {
        StartCoroutine(GetDataFromNodeJS());
        StartCoroutine(GetData2FromNodeJS());
    }

    IEnumerator GetDataFromNodeJS()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/api/character");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("Dữ liệu nhận được: " + json);

            ProductData productData = JsonUtility.FromJson<ProductData>(json);

            foreach (Transform child in contentPanel)
            {
                Destroy(child.gameObject);
            }

            productList.Clear(); // Clear the previous products
            foreach (var product in productData.data)
            {
                productList.Add(product); // Add to productList
                GameObject newProduct = Instantiate(productPrefab, contentPanel);
                Text productText = newProduct.GetComponentInChildren<Text>();
                productText.text = $"Tên: {product.name}\nGiá: {product.gia}";
            }
        }
        else
        {
            jsonDisplay.text = "Error: " + request.error;
            Debug.LogError("Lỗi: " + request.error);
        }
    }

    IEnumerator GetData2FromNodeJS()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/api/usercharacter");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("Dữ liệu nhận được: " + json);

            ProductData productData = JsonUtility.FromJson<ProductData>(json);

            foreach (Transform child in contentPanel)
            {
                Destroy(child.gameObject);
            }

            foreach (var product in productData.data)
            {
                GameObject newProduct = Instantiate(product2Prefab, contentPanel);
                Text productText = newProduct.GetComponentInChildren<Text>();
                productText.text = $"Tên: {product.name}\nGiá: {product.gia}";
            }
        }
        else
        {
            jsonDisplay.text = "Error: " + request.error;
            Debug.LogError("Lỗi: " + request.error);
        }
    }

    public void TransferData()
    {
        foreach (var product in productList)
        {
            StartCoroutine(SendProductDataToAnotherTable(product));
        }
    }

    IEnumerator SendProductDataToAnotherTable(Product product)
    {
        string json = JsonUtility.ToJson(product);
        UnityWebRequest request = UnityWebRequest.PostWwwForm("http://localhost:3000/api/usercharacter", json);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Dữ liệu đã được chuyển thành công!");
        }
        else
        {
            Debug.LogError("Lỗi khi chuyển dữ liệu: " + request.error);
        }
    }
}
