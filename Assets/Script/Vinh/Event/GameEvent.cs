using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvent
{
    public static Action LoginSuccessful;
    public static Action LoginFailed;

    public static Action OptionButtonOnClick;
    public static Action ArenaButtonOnClick;
    public static Action PlayButtonOnClick;

    public static Action OptionSelected;
    public static Action OptionCancelled;
    public static Action RegisterSuccessful;
    public static Action RegisterFailed;

    #region Game state change events;

    public static Action GameStarted;

    public static Action GameWon;

    public static Action GameLost;
    public static Action GameAborted;

    #endregion
}
