using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;
using Unity.Netcode;

[RequireComponent(typeof(FirstPersonController))]
public class PlayerNetworkMover : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private GameObject gunObject;
    [SerializeField] private GameObject playerObject;

    private FirstPersonController firstPersonController;

    void Awake()
    {
        firstPersonController = GetComponent<FirstPersonController>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            // Chỉ bật camera và controller cho người chơi local
            cameraObject.SetActive(true);
            firstPersonController.enabled = true;

            // Ẩn model của chính người chơi để tránh che camera
            if (playerObject != null)
            {
                MoveToLayer(playerObject, LayerMask.NameToLayer("Hidden"));
            }
        }
        else
        {
            // Tắt camera và controller cho người chơi khác
            cameraObject.SetActive(false);
            firstPersonController.enabled = false;
        }
    }

    void Update()
    {
        if (!IsSpawned || !IsOwner) return;

        // Chỉ người chơi local mới cập nhật animator
        UpdateAnimatorParameters();
    }

    private void UpdateAnimatorParameters()
    {
        if (animator != null)
        {
            animator.SetFloat("Horizontal", CrossPlatformInputManager.GetAxis("Horizontal"));
            animator.SetFloat("Vertical", CrossPlatformInputManager.GetAxis("Vertical"));
            animator.SetBool("Running", Input.GetKey(KeyCode.LeftShift));

            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                animator.SetTrigger("IsJumping");
            }
        }
    }

    void MoveToLayer(GameObject gameObject, int layer)
    {
        foreach (Transform child in gameObject.transform)
        {
            MoveToLayer(child.gameObject, layer);
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (cameraObject != null)
        {
            cameraObject.SetActive(false);
        }
        if (firstPersonController != null)
        {
            firstPersonController.enabled = false;
        }
    }
}