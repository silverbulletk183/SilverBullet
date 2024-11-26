using UnityEngine;

public class Ammo : MonoBehaviour, IInteractable
{

    [Header("Ammo Settings")]
    [SerializeField] private int totalAmmo = 90;
    [SerializeField] private string interactMessage;


    public void Interact(PlayerController playerController)
    {
        playerController.CurrentWeapon().totalAmmo += totalAmmo;
        Destroy(gameObject);
    }

    public string Message()
    {
        return interactMessage;
    }
}
