using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //player
    private CharacterController controller;
    public float speed = 12f;
    //jump
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool isMoving;

<<<<<<< HEAD
=======
   public Animator animations;

>>>>>>> luan
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        controller = GetComponent<CharacterController>();
=======
        
        controller = GetComponent<CharacterController>();
       
>>>>>>> luan
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckGroundStatus();
        HandleMovement();
        HandleJump();
        ApplyGravity();
        CheckIfMoving();
    }

    // Ki?m tra n?u nhân v?t ?ang trên m?t ??t
    void CheckGroundStatus()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    // X? lý di chuy?n
    void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
<<<<<<< HEAD
=======
     
>>>>>>> luan
    }

    // X? lý nh?y
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
<<<<<<< HEAD
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
=======
            animations.SetBool("jump", true);
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Invoke("offjump", 2f);
        }
       
    }
    void offjump()
    {
        animations.SetBool("jump", false);
>>>>>>> luan
    }

    // Áp d?ng tr?ng l?c
    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Ki?m tra n?u nhân v?t ?ang di chuy?n
    void CheckIfMoving()
    {
        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
<<<<<<< HEAD
=======
            animations.SetBool("run", true);
>>>>>>> luan
        }
        else
        {
            isMoving = false;
<<<<<<< HEAD
=======
            animations.SetBool("run", false);
>>>>>>> luan
        }

        lastPosition = gameObject.transform.position;
    }
}
