using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;



[RequireComponent(typeof(FirstPersonController))]
public class PlayerNetworkMover : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject cameraObject;
    [SerializeField]
    private GameObject gunObject;
    [SerializeField]
    private GameObject playerObject;

    /// <summary>
    /// Move game objects to another layer.
    /// </summary>
    void MoveToLayer(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
        {
            MoveToLayer(child.gameObject, layer);
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Enable camera for local player
        cameraObject.SetActive(true);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // Enable FirstPersonController for local movement
        GetComponent<FirstPersonController>().enabled = true;

        // Move gun and player model to appropriate layers
        MoveToLayer(gunObject, LayerMask.NameToLayer("Hidden"));
        MoveToLayer(playerObject, LayerMask.NameToLayer("Hidden"));
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Handle animation updates
        animator.SetFloat("Horizontal", CrossPlatformInputManager.GetAxis("Horizontal"));
        animator.SetFloat("Vertical", CrossPlatformInputManager.GetAxis("Vertical"));

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            animator.SetTrigger("IsJumping");
        }

        animator.SetBool("Running", Input.GetKey(KeyCode.LeftShift));
    }
}
