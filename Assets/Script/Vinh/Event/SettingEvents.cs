using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SettingEvents : MonoBehaviour
{
    public static Action SettingInitialized;

    //Presenter --> View: update UI slider
    public static Action<float> SFXSliderSet;
    public static Action<float> MusicSliderSet;

    //View --> Presenter: update UI sliders
    public static Action<float> SFXSliderChanged;
    public static Action<float> MusicSliderChanged;

    //Presenter --> Model: update volume settings
    public static Action<float> SFXVolumeChanged;
    public static Action<float> MusicVolumeChanged;

    //Model --> Presenter: model values changed
    public static Action<float> ModelSFXVolumeChanged;
    public static Action<float> ModelMusicVolumeChanged;
}
