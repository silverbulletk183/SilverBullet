using UnityEngine;

public class Gate : MonoBehaviour, IInteractable
{
    private Animator anim;
    [SerializeField] private string interactMessage = "Open/Close Gate";

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public string Message()
    {
        return interactMessage;
    }

    public void Interact(PlayerController playerController)
    {
        anim.SetTrigger("OpenCloseTrigger");
    }
}
