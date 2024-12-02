using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class testhost : NetworkBehaviour
{
    // Start is called before the first frame update

    public Transform prefab;
    Transform spanwobject;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        
        if (Input.GetKeyDown(KeyCode.K))
        {
         spanwobject = Instantiate(prefab);
            spanwobject.GetComponent<NetworkObject>().Spawn(true);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Destroy(spanwobject.gameObject);
        }
    }
}
