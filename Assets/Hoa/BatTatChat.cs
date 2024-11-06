using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTatChat : MonoBehaviour
{
    public GameObject pnChatTextJoin;
    public GameObject pnChatTextRoom;

    public void BatChatText()
    {
        pnChatTextJoin.SetActive(false);
        pnChatTextRoom.SetActive(true);
    }
    public void TatChatText()
    {
        pnChatTextJoin.SetActive(true);
        pnChatTextRoom.SetActive(false); 
        
    }
}
