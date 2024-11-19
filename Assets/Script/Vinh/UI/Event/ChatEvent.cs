using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatEvent
{
    public static Action<string, object> OnMessageReceived;
    public static Action ChatMessageSubmited;
}
