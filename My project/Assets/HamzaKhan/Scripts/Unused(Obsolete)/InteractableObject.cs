using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private UnityEvent interact;

    public void Interact()
    {
        // interact logic
        Debug.Log("Interact" + gameObject.name);
        interact.Invoke();
    }
}
