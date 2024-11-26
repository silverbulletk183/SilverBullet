using UnityEngine;
using Unity.Netcode;

public class CameraManager : NetworkBehaviour
{
    private void Start()
    {
        if (!IsOwner)
        {
            gameObject.SetActive(false);
        }
    }


}
