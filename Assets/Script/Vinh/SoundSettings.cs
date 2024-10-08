using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class SoundSettings : MonoBehaviour
{
    private Slider musicSlider;
    private Slider sfxSlider;
    public AudioMixer audioMixer;

    public static event Action<float> OnMusicVolumeChanged;
    public static event Action<float> OnSFXVolumeChanged;

    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        musicSlider = root.Q<Slider>("MusicSlider");
        sfxSlider = root.Q<Slider>("SFXSlider");
        float musicVolume;
        float sfxVolume;

        if (audioMixer.GetFloat("Music", out musicVolume))
        {
            musicSlider.value = Mathf.Pow(10, musicVolume / 20);
            Debug.Log(musicSlider.value);
        }

        if (audioMixer.GetFloat("SFX", out sfxVolume))
        {
            sfxSlider.value = Mathf.Pow(10, sfxVolume / 20);
        }

        musicSlider.RegisterValueChangedCallback(evt => MusicSliderChange(evt.newValue));
        sfxSlider.RegisterValueChangedCallback(evt => SFXSliderChange(evt.newValue));
    }
    public void MusicSliderChange(float newValue)
    {
        Debug.Log($"Updated music volume in AudioManager: {newValue}");
        OnMusicVolumeChanged?.Invoke(newValue);
    }public void SFXSliderChange(float newValue)
    {
        Debug.Log($"Updated SFX volume in AudioManager: {newValue}");

        OnSFXVolumeChanged?.Invoke(newValue);
    }
}
