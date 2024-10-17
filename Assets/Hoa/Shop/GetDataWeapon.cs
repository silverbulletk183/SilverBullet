using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; // Make sure to include this namespace

[System.Serializable]
public class ProductWeapon
{
    public string _id;
    public string name;
    public int damage;
    public int numberBullet;
    public int price;
}

[System.Serializable]
public class ProductDataWeapon
{
    public int status;
    public ProductWeapon[] data;
}

public class GetDataWeapon : MonoBehaviour
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
        UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/api/gun"); // Corrected this line
        yield return request.SendWebRequest(); // Corrected this line

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            ProductDataWeapon productData = JsonUtility.FromJson<ProductDataWeapon>(json);

            foreach (Transform child in contentPanel)
            {
                Destroy(child.gameObject);
            }

            foreach (var product in productData.data)
            {
                GameObject newProduct = Instantiate(productPrefab, contentPanel.transform);
                Text productText = newProduct.GetComponentInChildren<Text>(); // Corrected this line
                productText.text = $"Tên: {product.name}\nSát th??ng: {product.damage}\nS? ??n: {product.numberBullet}\nGiá: {product.price}"; // Fixed variable names for clarity
            }
        }
        else
        {
            jsonDisplay.text = "Error: " + request.error;
            Debug.LogError("L?i: " + request.error);
        }
    }
}
