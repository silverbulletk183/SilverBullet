using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private static GameDataManager _instance;
    [SerializeField] private LevelSO LevelSO;
    public UserSO UserSO;
    public static GameDataManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance.gameObject != this)
        {
            Destroy(this.gameObject);
        }else
        {
            _instance = this;
        }
    }
    public LevelSO UserData()
    {
        return LevelSO;
    }

}
