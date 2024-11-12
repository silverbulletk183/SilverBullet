using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.UI;

public class VoiceToggleController : MonoBehaviour
{
    public Recorder playerRecorder; // Gán Recorder c?a Player vào ?ây
    public string buttonName = "VoiceToggleButton"; // Tên c?a Button

    private bool isVoiceEnabled = true; // Tr?ng thái voice ban ??u

    void Start()
    {
        // Tìm Button theo tên
        Button toggleButton = GameObject.Find(buttonName).GetComponent<Button>();
        if (toggleButton != null)
        {
            // Gán s? ki?n ToggleVoice cho nút
            toggleButton.onClick.AddListener(ToggleVoice);
        }
        else
        {
            Debug.LogError("Không tìm th?y Button v?i tên: " + buttonName);
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
            Debug.LogError("Recorder ch?a ???c gán.");
        }
    }
}
