using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AuthenticationEvent
{
    public static Action<string, string> LoginButtonClicked;
    public static Action<string, string, string, string> RegisterButtonClicked;
    public static Action<string> LoginFail;
    public static Action<string> RegisterFail;
    public static Action RegisterSuccess;
}
