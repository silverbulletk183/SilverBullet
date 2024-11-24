using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Volume))]
public class GlobalVolumeInstance : MonoBehaviour
{

    // Instance.
    public static GlobalVolumeInstance instance;

    private void Awake()
    {
        // Set instance to this.
        instance = this;
    }

    public Volume GetGlobalVolumeRef()
    {
        return GetComponent<Volume>();
    }
}
