using System.Collections.Generic;
using UnityEngine;

public class NVData : MonoBehaviour
{
    public static NVData Instance { get; private set; } // S?a CharacterData thành NVData

    public List<Character> Characters { get; private set; } = new List<Character>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddCharacter(Character character)
    {
        Characters.Add(character);
    }

    public void ClearCharacters()
    {
        Characters.Clear();
    }
}

[System.Serializable]
public class Character
{
    public string name;
    public float price;
    public string imageUrl;

    public Character(string name, float price, string imageUrl)
    {
        this.name = name;
        this.price = price;
        this.imageUrl = imageUrl;
    }
}
