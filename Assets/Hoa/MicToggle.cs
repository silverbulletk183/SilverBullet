using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.UI;

public class VoiceToggleController : MonoBehaviour
{
    public Recorder playerRecorder; // G�n Recorder c?a Player v�o ?�y
    public string buttonName = "VoiceToggleButton"; // T�n c?a Button

    private bool isVoiceEnabled = true; // Tr?ng th�i voice ban ??u

    void Start()
    {
        // T�m Button theo t�n
        Button toggleButton = GameObject.Find(buttonName).GetComponent<Button>();
        if (toggleButton != null)
        {
            // G�n s? ki?n ToggleVoice cho n�t
            toggleButton.onClick.AddListener(ToggleVoice);
        }
        else
        {
            Debug.LogError("Kh�ng t�m th?y Button v?i t�n: " + buttonName);
        }
    }

    void ToggleVoice()
    {
        if (playerRecorder != null)
        {
            isVoiceEnabled = !isVoiceEnabled;
            playerRecorder.TransmitEnabled = isVoiceEnabled;
            Debug.Log("Voice " + (isVoiceEnabled ? "b?t" : "t?t"));
        }
        else
        {
            Debug.LogError("Recorder ch?a ???c g�n.");
        }
    }
}
