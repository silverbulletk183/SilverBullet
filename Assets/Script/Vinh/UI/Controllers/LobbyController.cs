using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyController : MonoBehaviour
{
    private void OnEnable()
    {
        LobbyEvent.LobbyStarted += OnLobbyStarted;
    }
    void OnLobbyStarted()
    {
        LobbyEvent.PlayerConnected?.Invoke(GameDataManager.Instance.UserSO.fullname, "path");
    }
}