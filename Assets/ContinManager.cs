using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ContinManager : NetworkBehaviour
{
    public string tag1 = "Player";
    public string tag2 = "ConTin";

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag(tag1)) 
        {
            Debug.Log($"Tag {tag1} đã chạm với Tag {tag2}");
            PlayerUI.Instance.ShowTextHelp(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tag1))
        {
            Debug.Log($"Tag {tag1} rời khỏi Tag {tag2}");
            PlayerUI.Instance.ShowTextHelp(false);
        }
    }
}
