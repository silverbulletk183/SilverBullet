using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransformRot : MonoBehaviour
{

    public Transform target;
    [SerializeField] private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.rotation = target.rotation * Quaternion.Euler(offset);
        }
    }
}
