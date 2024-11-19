using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData Instance;

    public string userId;
    public string username;
    
    public string imageUrl;
    public string nameAcc;
    public int gold;
    public int level;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
   
}

