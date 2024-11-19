using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyEvent : MonoBehaviour
{
    public static Action<string, string> PlayerConnected;
    public static Action PlayerDisconnected;

    //lobby state change event
    public static Action LobbyStarted;
    public static Action LobbyEnded;

    //player input event
    public static Action PlayerReady;
    public static Action PlayerChat;

    //UI interaction event
    public static Action StartButtonCLicked;
    public static Action LeaveLobbyButtonClicked;
}
