using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class GetImageNV : MonoBehaviour
{
    public string characterApiUrl = "http://localhost:3000/api/character"; // API l?y danh s�ch nh�n v?t
    public string characterImageApiUrl = "http://localhost:3000/api/characterimage"; // API l?y ?nh nh�n v?t
    public RawImage characterImage; // RawImage ?? hi?n th? ?nh

    private void Start()
    {
        // T�m ho?c g�n RawImage n?u ch?a ???c g�n t? Inspector
        if (characterImage == null)
        {
            characterImage = FindRawImage();
        }

        // Ki?m tra v� log
        if (characterImage == null)
        {
            Debug.LogError("RawImage ch?a ???c g�n ho?c kh�ng t�m th?y!");
            return; // Ng?ng th?c hi?n n?u kh�ng t�m th?y RawImage
        }

        // B?t ??u t?i d? li?u
        StartCoroutine(LoadCharacterIdAndImage());
    }

    // H�m t�m RawImage trong Scene ho?c Prefab
    private RawImage FindRawImage()
    {
        // Tr??ng h?p RawImage trong Scene
        RawImage imageInScene = GameObject.Find("CharacterImage")?.GetComponent<RawImage>();
        if (imageInScene != null)
        {
            return imageInScene;
        }

        // Tr??ng h?p RawImage l� con c?a Prefab
        RawImage imageInPrefab = GetComponentInChildren<RawImage>();
        if (imageInPrefab != null)
        {
            return imageInPrefab;
        }

        // Kh�ng t�m th?y
        return null;
    }

    // Coroutine ?? t?i danh s�ch nh�n v?t v� ?nh
    IEnumerator LoadCharacterIdAndImage()
    {
        // G?i y�u c?u GET ?? l?y danh s�ch nh�n v?t
        UnityWebRequest request = UnityWebRequest.Get(characterApiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // X? l� JSON tr? v?
            string jsonResult = request.downloadHandler.text;
            string characterId = ParseCharacterIdFromJson(jsonResult);

            if (!string.IsNullOrEmpty(characterId))
            {
                // G?i y�u c?u GET ?? t?i ?nh nh�n v?t
                string imageApiUrl = $"{characterImageApiUrl}?id={characterId}";
                yield return StartCoroutine(LoadImage(imageApiUrl));
            }
            else
            {
                Debug.LogError("Kh�ng t�m th?y ID nh�n v?t trong danh s�ch.");
            }
        }
        else
        {
            Debug.LogError("L?i khi t?i danh s�ch nh�n v?t: " + request.error);
        }
    }

    // Ph�n t�ch ID t? JSON tr? v?
    private string ParseCharacterIdFromJson(string json)
    {
        // Parse JSON (gi? s? danh s�ch c� c?u tr�c [{ "id": "value", "name": "value" }])
        Character[] characters = JsonUtility.FromJson<CharacterList>("{\"characters\":" + json + "}").characters;

        if (characters.Length > 0)
        {
            return characters[0].id; // L?y ID c?a nh�n v?t ??u ti�n
        }

        return null;
    }

    // Coroutine t?i ?nh t? URL
    IEnumerator LoadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // �nh x? texture l�n RawImage
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            if (characterImage != null)
            {
                characterImage.texture = texture;
                Debug.Log("T?i ?nh th�nh c�ng!");
            }
            else
            {
                Debug.LogError("RawImage kh�ng ???c g�n, kh�ng th? hi?n th? ?nh.");
            }
        }
        else
        {
            Debug.LogError("L?i khi t?i ?nh: " + request.error);
        }
    }

    // Class ph? ?? parse JSON
    [System.Serializable]
    public class Character
    {
        public string id;
        public string name;
    }

    [System.Serializable]
    public class CharacterList
    {
        public Character[] characters;
    }
}
