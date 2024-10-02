using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public
 float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    public GameObject Players;
         public GameObject capsua;

    public Animator animator;
void Update()
    {
        Players.transform.position = capsua.transform.position;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y
 = -2f;
        }
       
        float x = Input.GetAxis("Horizontal");
       
        float z = Input.GetAxis("Vertical");

        Vector3
 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("run", true);
        }
        else if  (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("runR", true);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("runL", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("runB", true);
        }
        else
        {
            animator.SetBool("run", false);
            animator.SetBool("runB", false);
            animator.SetBool("runL", false);
            animator.SetBool("runR", false);
        }
        
       




        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("jump", true);
            Invoke("offjump", 1f);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }

    void offjump()
    {
        animator.SetBool("jump", false);
    }
}
