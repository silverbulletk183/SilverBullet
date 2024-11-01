using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineBrain))]
public class CinemachineBrainInstance : MonoBehaviour
{

    // Instance
    public static CinemachineBrainInstance instance;

    private void Awake()
    {
        // Set the Instance.
        instance = this;
    }

    public CinemachineBrain GetCinemachineBrain()
    {
        return GetComponent<CinemachineBrain>();
    }
}
