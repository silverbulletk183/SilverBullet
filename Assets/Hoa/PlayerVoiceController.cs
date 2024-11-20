using Photon.Voice.PUN;
using Photon.Pun;
using Photon.Voice;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVoiceController : MonoBehaviourPun
{
    private PhotonVoiceView photonVoiceView;
    private Recorder recorder;
    [SerializeField]
    private Button toggleButton; // Gán tr?c ti?p trong Inspector
    private bool isMuted = false;

    void Awake()
    {
        photonVoiceView = GetComponent<PhotonVoiceView>();
        recorder = photonVoiceView.RecorderInUse;

        if (toggleButton == null)
        {
            Debug.LogError("Button không ???c gán! Hãy gán Button trong Inspector.");
            return;
        }

        toggleButton.onClick.AddListener(ToggleVoice);
    }

     public void ToggleVoice()
    {
        isMuted = !isMuted;
        recorder.TransmitEnabled = !isMuted; // T?t truy?n ?? t?t ti?ng, b?t ?? m? ti?ng

        Debug.Log($"Gi?ng nói hi?n ?ang {(isMuted ? "T?t" : "M?")}");
    }
}
