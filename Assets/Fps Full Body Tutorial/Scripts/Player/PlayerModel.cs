using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{

    [Space]
    [Header("Audio Clip...")]
    [SerializeField] private AudioClip footAudioClip;
    [SerializeField] private AudioClip reloadAudioClip;
    [SerializeField] private AudioClip magInOutAudioClip;

    [Space]
    [Header("Ref")]
    [SerializeField] private Transform magHoldingPos;


    private PlayerController playerController;


    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    // Animation Events
    #region Reloading
    public void PlayFootSound()
    {
        // check if there is some movement input.
        if (playerController.Input.Player.Move.ReadValue<Vector2>() != Vector2.zero)
        {
            // play the foot audio clip
            AudioSource.PlayClipAtPoint(footAudioClip, transform.position, 1f);
        }
    }

    public void PlayReloadSound()
    {
        playerController.CurrentWeapon().audioSource.PlayOneShot(reloadAudioClip, 1f);
    }

    public void PlayMagInOut()
    {
        playerController.CurrentWeapon().audioSource.PlayOneShot(magInOutAudioClip, 1f);
    }

    public void Reload(int Step)
    {
        /////////////////
        //if (Step == 1)
        //{
        //    // Step 1
        //    // set the parent of the mag to the mag holding pos.
        //    playerController.CurrentWeapon().mag.transform.SetParent(magHoldingPos);
        //}
        //else if (Step == 2)
        //{
        //    // Step 2
        //    // Create a clone of the original mag.
        //    playerController.CurrentWeapon().mag.gameObject.SetActive(false);
        //    GameObject newMag = Instantiate(playerController.CurrentWeapon().mag, playerController.CurrentWeapon().mag.transform.position, playerController.CurrentWeapon().mag.transform.rotation);
        //    newMag.AddComponent<Rigidbody>();
        //    newMag.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        //    newMag.AddComponent<DecalDestroyer>();
        //    newMag.SetActive(true);
        //}
        //else if (Step == 3)
        //{
        //    // Step 3
        //    playerController.CurrentWeapon().mag.gameObject.SetActive(true);
        //}
        //else if (Step == 4)
        //{
        //    // Step 4
        //    playerController.CurrentWeapon().mag.transform.SetParent(playerController.CurrentWeapon().magParent);
        //    playerController.CurrentWeapon().mag.transform.position = playerController.CurrentWeapon().magParent.position;
        //    playerController.CurrentWeapon().mag.transform.rotation = playerController.CurrentWeapon().magParent.rotation;
        //}
        //else
        //{
        //    // For letting over nice user know what he has done wrong!!!
        //    Debug.LogError("Reload step cannot be other than 1, 2, 3, 4 in the animation events");
        //}
    }
    #endregion
}
