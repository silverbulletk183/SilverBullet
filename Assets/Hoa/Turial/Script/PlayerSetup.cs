using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode; // Import Netcode for GameObjects

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    
    Camera sceneCamera;

    void Start()
    {
        if (!IsOwner) // Thay isLocalPlayer b?ng IsOwner
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
        else
        {
            sceneCamera = Camera.main;
            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }
    }

    void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }
}
