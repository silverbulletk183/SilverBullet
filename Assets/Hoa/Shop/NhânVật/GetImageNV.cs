using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class GetImageNV : MonoBehaviour
{
    public string characterApiUrl = "http://localhost:3000/api/character"; // API l?y danh sách nhân v?t
    public string characterImageApiUrl = "http://localhost:3000/api/characterimage"; // API l?y ?nh nhân v?t
    public RawImage characterImage; // RawImage ?? hi?n th? ?nh

    private void Start()
    {
        // Tìm ho?c gán RawImage n?u ch?a ???c gán t? Inspector
        if (characterImage == null)
        {
            characterImage = FindRawImage();
        }

        // Ki?m tra và log
        if (characterImage == null)
        {
            Debug.LogError("RawImage ch?a ???c gán ho?c không tìm th?y!");
            return; // Ng?ng th?c hi?n n?u không tìm th?y RawImage
        }

        // B?t ??u t?i d? li?u
        StartCoroutine(LoadCharacterIdAndImage());
    }

    // Hàm tìm RawImage trong Scene ho?c Prefab
    private RawImage FindRawImage()
    {
        // Tr??ng h?p RawImage trong Scene
        RawImage imageInScene = GameObject.Find("CharacterImage")?.GetComponent<RawImage>();
        if (imageInScene != null)
        {
            return imageInScene;
        }

        // Tr??ng h?p RawImage là con c?a Prefab
        RawImage imageInPrefab = GetComponentInChildren<RawImage>();
        if (imageInPrefab != null)
        {
            return imageInPrefab;
        }

        // Không tìm th?y
        return null;
    }

    // Coroutine ?? t?i danh sách nhân v?t và ?nh
    IEnumerator LoadCharacterIdAndImage()
    {
        // G?i yêu c?u GET ?? l?y danh sách nhân v?t
        UnityWebRequest request = UnityWebRequest.Get(characterApiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // X? lý JSON tr? v?
            string jsonResult = request.downloadHandler.text;
            string characterId = ParseCharacterIdFromJson(jsonResult);

            if (!string.IsNullOrEmpty(characterId))
            {
                // G?i yêu c?u GET ?? t?i ?nh nhân v?t
                string imageApiUrl = $"{characterImageApiUrl}?id={characterId}";
                yield return StartCoroutine(LoadImage(imageApiUrl));
            }
            else
            {
                Debug.LogError("Không tìm th?y ID nhân v?t trong danh sách.");
            }
        }
        else
        {
            Debug.LogError("L?i khi t?i danh sách nhân v?t: " + request.error);
        }
    }

    // Phân tích ID t? JSON tr? v?
    private string ParseCharacterIdFromJson(string json)
    {
        // Parse JSON (gi? s? danh sách có c?u trúc [{ "id": "value", "name": "value" }])
        Character[] characters = JsonUtility.FromJson<CharacterList>("{\"characters\":" + json + "}").characters;

        if (characters.Length > 0)
        {
            return characters[0].id; // L?y ID c?a nhân v?t ??u tiên
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
            // Ánh x? texture lên RawImage
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            if (characterImage != null)
            {
                characterImage.texture = texture;
                Debug.Log("T?i ?nh thành công!");
            }
            else
            {
                Debug.LogError("RawImage không ???c gán, không th? hi?n th? ?nh.");
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
