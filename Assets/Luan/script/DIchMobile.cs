using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dichmobile : MonoBehaviour
{
    // Speed for movement
    private Rigidbody rb; // Rigidbody for 3D
    public Joystick joystick;  // Reference to the joystick

    Collider collider;
    bool isSliding;

    public float speed = 6f;

    private CharacterController controller;
    public Animator ani;

    public GameObject player;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Get the joystick's horizontal and vertical input axes
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        // Use this input to move the character
        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;

        // Move the character using CharacterController
        controller.Move(move * speed * Time.deltaTime);

        // Update the animation based on movement
        if (horizontalInput > 0) { ani.SetBool("runR", true); ani.SetBool("runL", false); }
        else if (horizontalInput < 0) { ani.SetBool("runL", true); ani.SetBool("runR", false); }
        else { ani.SetBool("runR", false); ani.SetBool("runL", false); }

        if (verticalInput > 0) { ani.SetBool("run", true); ani.SetBool("runB", false); }
        else if (verticalInput < 0) { ani.SetBool("runB", true); ani.SetBool("run", false); }
        else { ani.SetBool("run", false); ani.SetBool("runB", false); }

        // If there's no input, set the animations to idle (or stop)
        if (horizontalInput == 0 && verticalInput == 0)
        {
            ani.SetBool("run", false);
            ani.SetBool("runR", false);
            ani.SetBool("runL", false);
            ani.SetBool("runB", false);
        }
    }

}
