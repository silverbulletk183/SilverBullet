using UnityEngine;

public class PlayerMovement_h : MonoBehaviour
{
    public float moveSpeed = 5f;          // Speed of the player
    public float lookSpeed = 2f;          // Mouse look sensitivity
    public float jumpHeight = 1f;         // Jump height
    public Camera playerCamera;            // Reference to the player camera

    [SerializeField] private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    private float xRotation = 0f;          // Track vertical rotation
    public float minYAngle = -35f;         // Minimum angle for vertical look
    public float maxYAngle = 35f;          // Maximum angle for vertical look

    private void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    private void Update()
    {
        Move();
        Look();
        Jump();
    }

    private void Move()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f; // Reset vertical velocity when grounded
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Apply gravity
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        // Calculate the vertical rotation and clamp it
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minYAngle, maxYAngle);

        // Apply rotations
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }
    }
}
