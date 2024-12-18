using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData Instance;

    public string userId;
    public string nameAcc;
    public int gold;
    public int level;
    public int userCharacter = 0;
    public bool isActive;
    public int goldOfMatch;

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

