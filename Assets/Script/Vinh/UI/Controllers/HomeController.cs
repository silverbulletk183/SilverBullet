using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeController : MonoBehaviour
{
    private void OnEnable()
    {
        HomeEvent.NameInputed += OnNameInputed;
    }
    public void OnNameInputed(string username)
    {
        GameDataManager.Instance.UserSO.username = username;
    }
}
