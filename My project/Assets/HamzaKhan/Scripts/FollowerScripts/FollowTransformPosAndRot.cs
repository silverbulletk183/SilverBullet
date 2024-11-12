using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransformPosAndRot : MonoBehaviour
{

    public Transform target;

    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + positionOffset;
            transform.rotation = target.rotation * Quaternion.Euler(rotationOffset);
        }
    }
}
