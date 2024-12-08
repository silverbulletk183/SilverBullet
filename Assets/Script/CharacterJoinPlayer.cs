using Photon.Voice.PUN.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterJoinPlayer : MonoBehaviour
{
    [SerializeField] private int playerIndex; // 1-based or 0-based; ensure consistency
    [SerializeField] private TextMeshProUGUI txtReady;
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private RawImage avt;

    private void Start()
    {
        if (SilverBulletMultiplayer.Instance != null)
        {
            SilverBulletMultiplayer.Instance.OnPlayerDataNetworkListChanged += SilverBulletMulltiplayer_OnPlayerDataNetworkListChanged;
        }
        else
        {
            Debug.LogWarning("SilverBulletMultiplayer.Instance is null!");
        }

        if (CharacterSelectReady.Instance != null)
        {
            CharacterSelectReady.Instance.OnReadyChanged += CharacterSelectReady_OnReadyChanged;
        }
        else
        {
            Debug.LogWarning("CharacterSelectReady.Instance is null!");
        }

        UpdatePlayer();
    }

    private void CharacterSelectReady_OnReadyChanged(object sender, System.EventArgs e)
    {
        Debug.Log("Ready changed");
        UpdatePlayer();
    }

    private void SilverBulletMulltiplayer_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        try {

            if (SilverBulletMultiplayer.Instance != null &&
                SilverBulletMultiplayer.Instance.IsPlayerIndexConneted(playerIndex - 1)) // Adjust based on 0-based or 1-based indexing
            {
                Show();
                PlayerData playerData = SilverBulletMultiplayer.Instance.GetPlayerDataFromPlayerIndex(playerIndex - 1);
             
                if (CharacterSelectReady.Instance != null &&
                    CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId))
                {
                    txtReady.text = "Sẵn sàng";
                }
                else
                {
                    txtReady.text = "Vui lòng chờ";
                }
                txtName.text = playerData.playerName.ToString();
                if (playerData.userId != "")
                {
                    StartCoroutine(UploadAndDisplayImage.Instance.LoadImage(APIURL.UserImage + playerData.userId, avt));
                }

                
            }
            else
            {
                Hide();
            }

        }
        catch{
            Debug.Log("loioday");
        }
        
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        SilverBulletMultiplayer.Instance.OnPlayerDataNetworkListChanged -= SilverBulletMulltiplayer_OnPlayerDataNetworkListChanged;
        /* if (SilverBulletMultiplayer.Instance != null)
         {
             SilverBulletMultiplayer.Instance.OnPlayerDataNetworkListChanged -= SilverBulletMulltiplayer_OnPlayerDataNetworkListChanged;
         }

         if (CharacterSelectReady.Instance != null)
         {
             CharacterSelectReady.Instance.OnReadyChanged -= CharacterSelectReady_OnReadyChanged;
         }*/
    }
}
