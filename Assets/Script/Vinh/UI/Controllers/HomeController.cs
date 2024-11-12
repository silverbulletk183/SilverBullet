using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    private void OnEnable()
    {
        HomeEvent.NameInputed += OnNameInputed;
        GameEvent.PlayButtonOnClick += OnPlayButtonOnClicked;
    }
    public void OnNameInputed(string username)
    {
        GameDataManager.Instance.UserSO.username = username;
    }
    public void OnPlayButtonOnClicked()
    {
        Loader.Load(Loader.Scene.TeamCreationUI);
    }
}
