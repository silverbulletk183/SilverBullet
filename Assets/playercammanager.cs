using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class playercammanager : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame  
  
}
