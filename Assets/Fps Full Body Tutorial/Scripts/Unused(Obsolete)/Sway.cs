using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{


    [Space]
    [Header("Sway Settings")]
    [SerializeField] private float amount = 1.5f;
    [SerializeField] private float smoothRot = 5f;

    [SerializeField] private PlayerController playerController;

    private Quaternion initialRot;

    private float lookX;
    private float lookY;

    // Start is called before the first frame update
    void Start()
    {
        initialRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        lookX = playerController.Input.Player.Look.ReadValue<Vector2>().x * amount;
        lookY = playerController.Input.Player.Look.ReadValue<Vector2>().y * amount;

        if (playerController.GetXRotation() > 89f || playerController.GetXRotation() < -89f)
        {
            Quaternion finalRot = Quaternion.AngleAxis(-lookX, Vector3.forward);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRot * initialRot, Time.deltaTime * smoothRot);
        }
        else
        {
            Quaternion finalRot = Quaternion.AngleAxis(-lookY, Vector3.right) * Quaternion.AngleAxis(-lookX, Vector3.forward);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRot * initialRot, Time.deltaTime * smoothRot);
        }
    }
}
