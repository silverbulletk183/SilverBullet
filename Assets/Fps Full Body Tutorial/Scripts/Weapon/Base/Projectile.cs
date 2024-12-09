using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : NetworkBehaviour
{

    private Rigidbody rb;
    [Header("Settings")]
    [SerializeField] private float maxDestructionTime = 5f;

    public void Fire(float fireForce)
    {
        rb = GetComponent<Rigidbody>();

        Destroy(gameObject, maxDestructionTime);

        rb.AddForce(transform.forward * fireForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // get the IHitable interface reference.
        IHitable iHitable = collision.transform.GetComponent<IHitable>();

        // check if ihitable is not null.
        if (iHitable != null)
        {
            // Hit.
            iHitable.Hit(collision.transform.gameObject, collision.GetContact(0).point, collision.GetContact(0).normal);
        }

        // destroy.
        Destroy(gameObject);
    }
}
