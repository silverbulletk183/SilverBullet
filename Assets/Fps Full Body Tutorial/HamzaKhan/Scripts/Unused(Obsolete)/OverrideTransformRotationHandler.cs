using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(OverrideTransform))]
public class OverrideTransformRotationHandler : MonoBehaviour
{


    [Space]
    [Header("Target")]
    [Tooltip("The Target Transform whoes local rotation will be used")]
    [SerializeField] private Transform targetRotation;

    [Space(5)]
    [Tooltip("When Enabled the values provided in (valueBasedRotation) are going to be used instead of the transform")]
    [SerializeField] private bool useValues = false;

    [Tooltip("The euler rotation for the override transform, Ignored if use values is disabled")]
    [SerializeField] private Vector3 valueBasedRotation;

    // The Override Transform Component.
    private OverrideTransform overrideTransform;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Override Transform Component.
        overrideTransform = GetComponent<OverrideTransform>();

        // Check if the override transform component is null.
        if (overrideTransform == null)
        {
            Debug.LogWarning("There isn't any override transform component attached, Please attach one to use this component");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Handle Override Transform.
        HandleOverrideTransform(overrideTransform != null ? true : false);
    }

    /// <summary>
    /// Some Basic Code for Handling The Attached Override Transform.
    /// </summary>
    /// <param name="HasRefToOverrideTransform"></param>
    private void HandleOverrideTransform(bool HasRefToOverrideTransform)
    {
        // Check if we override transform isn't null.
        if (HasRefToOverrideTransform == true)
        {
            // Check if we need to use values instead of transforms.
            if (useValues == true)
            {
                // Set Override Transform Rotation to the Value Based Rotation.
                overrideTransform.data.rotation = valueBasedRotation;
            }
            else
            {
                // Set Override Transform Rotation to the Target.
                overrideTransform.data.rotation = targetRotation.localRotation.eulerAngles;
            }
        }
    }
}
