using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // AudioMixerGroup names
    public static string MusicGroup = "Music";
    public static string SfxGroup = "SFX";

    // parameter suffix
    const string k_Parameter = "Volume";

    [SerializeField] AudioMixer m_MainAudioMixer;

    private void OnEnable()
    {
        SoundSettings.OnMusicVolumeChanged += UpdateMusicVolume;
        SoundSettings.OnSFXVolumeChanged += UpdateSFXVolume;
    }

    private void OnDisable()
    {
        SoundSettings.OnMusicVolumeChanged -= UpdateMusicVolume;
        SoundSettings.OnSFXVolumeChanged -= UpdateSFXVolume;
    }

    private void UpdateMusicVolume(float newValue)
    {
        float newVolume = Mathf.Log10(newValue);
        m_MainAudioMixer.SetFloat(MusicGroup + k_Parameter, newVolume);
    }

    private void UpdateSFXVolume(float newValue)
    {
        float newVolume = Mathf.Log10(newValue);
        m_MainAudioMixer.SetFloat(SfxGroup + k_Parameter, newVolume);
    }
}
