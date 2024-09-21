using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLook : MonoBehaviour
{
    //Mouse
    public static bool Cursorlocked = true;
    //camera
    public Transform player;
    public Transform cams;
    public float XSensitivity;
    public float YSensitivity;
    public float MaxAngle;

    private Quaternion camcenter;

    private void Start()
    {
        camcenter = cams.localRotation;
    }

    private void LateUpdate()
    {
        SetY();
        SetX();
        UpdateCurSorLock();
    }

    void SetY()
    {
        float t_input = Input.GetAxis("Mouse Y") * YSensitivity * Time.deltaTime;
        Quaternion quaternion = Quaternion.AngleAxis(t_input, -Vector3.right);
        Quaternion t_delta = cams.localRotation * quaternion; 

        if (Quaternion.Angle(camcenter, t_delta) < MaxAngle)
        {
            cams.localRotation = t_delta;
        }
    }
    void SetX()
    {
        float t_input = Input.GetAxis("Mouse X") * XSensitivity * Time.deltaTime;
        Quaternion quaternion = Quaternion.AngleAxis(t_input, Vector3.up);
        Quaternion t_delta = player.localRotation * quaternion;

        player.localRotation = t_delta;
    }
    
    void UpdateCurSorLock()
    {
        if (Cursorlocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Cursorlocked = false;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Cursorlocked = true;
            }
        }
    }
}
