using UnityEngine;

public interface IInteractable
{
    string Message();
    void Interact(PlayerController playerController);
}
