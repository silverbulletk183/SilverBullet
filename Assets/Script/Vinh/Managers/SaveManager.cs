using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance { 
        get
        {
            if (_instance == null)
            {
                Debug.LogError("SaveManager instance is Null");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null &&  _instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void SaveLocalFile(string filename, string vallue)
    {
        PlayerPrefs.SetString(filename, vallue);
    }
    public string ReadLocalFile(string filename)
    {
        return PlayerPrefs.GetString(filename, null);
    }
}
