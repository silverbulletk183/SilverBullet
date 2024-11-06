using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements
using Photon.Pun; // Required for PhotonView
using Photon.Voice.Unity; // Required for Photon Voice

public class VoiceManager : MonoBehaviour
{
    public Recorder recorder;
    private PhotonView photonView;

    private bool isActiveVoice = true;
    private Text textStateVoice;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        recorder = GameObject.FindGameObjectWithTag("Recorder").GetComponent<Recorder>();

        textStateVoice = GameObject.
            FindGameObjectWithTag("StateVoice").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            ClickButtonToVoiceChat();
        }
    }

    public void ClickButtonToVoiceChat()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (isActiveVoice)
            {
                recorder.RecordingEnabled = false;

                isActiveVoice = false;
                textStateVoice.text = "Voice chat: OFF";
            }
            else
            {
                recorder.RecordingEnabled = true;

                isActiveVoice = true;
                textStateVoice.text = "Voice chat: ON";
            }
        }
    }
}
