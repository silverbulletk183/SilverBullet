using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btnClinent;
    public Button btnHost;
    private void Awake()
    {
        btnClinent.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            NetworkManager.Singleton.SceneManager.LoadScene("TeamCreationUI",LoadSceneMode.Single);
        });
        btnHost.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene("TeamCreationUI", LoadSceneMode.Single);
        });
    }
}
