using UnityEngine;

[RequireComponent(typeof(FollowTransformPosLateUpdate))]
public class HeadBob : MonoBehaviour
{

    [Space]
    [Header("Settings")]
    [SerializeField] private float walkingBobSpeed;
    [SerializeField] private float walkingBobAmount;

    [SerializeField] private float sprintingBobSpeed;
    [SerializeField] private float sprintingBobAmount;

    [SerializeField] private float crouchingBobSpeed;
    [SerializeField] private float crouchingBobAmount;

    [SerializeField] private float returnSpeed = 20f;

    [SerializeField] private PlayerController playerController;


    private float defaultYPos;
    private float timer;

    private FollowTransformPosLateUpdate followTransformPosLateUpdate;


    // Start is called before the first frame update
    void Start()
    {
        followTransformPosLateUpdate = GetComponent<FollowTransformPosLateUpdate>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.IsGrounded() == true && playerController.Input.Player.Move.ReadValue<Vector2>() != Vector2.zero)
        {
            if (playerController.IsWalking() == true)
            {
                timer += Time.deltaTime * walkingBobSpeed;
                followTransformPosLateUpdate.offset = new Vector3(
                    followTransformPosLateUpdate.offset.x,
                    defaultYPos + Mathf.Sin(timer) * walkingBobAmount,
                    followTransformPosLateUpdate.offset.z);
            }
            else if (playerController.IsSprinting() == true)
            {
                timer += Time.deltaTime * sprintingBobSpeed;
                followTransformPosLateUpdate.offset = new Vector3(
                    followTransformPosLateUpdate.offset.x,
                    defaultYPos + Mathf.Sin(timer) * sprintingBobAmount,
                    followTransformPosLateUpdate.offset.z);
            }
            else if (playerController.IsCrouching() == true)
            {
                timer += Time.deltaTime * crouchingBobSpeed;
                followTransformPosLateUpdate.offset = new Vector3(
                    followTransformPosLateUpdate.offset.x,
                    defaultYPos + Mathf.Sin(timer) * crouchingBobAmount,
                    followTransformPosLateUpdate.offset.z);
            }
        }
        else
        {
            followTransformPosLateUpdate.offset = new Vector3(followTransformPosLateUpdate.offset.x, Mathf.Lerp(followTransformPosLateUpdate.offset.y, 0f, Time.deltaTime * returnSpeed), followTransformPosLateUpdate.offset.z);
        }
    }
}