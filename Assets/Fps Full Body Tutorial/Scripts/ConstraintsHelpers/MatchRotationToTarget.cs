using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchRotationToTarget : MonoBehaviour, ICustomConstraints
{

    // Settings.
    [Space]
    [Header("Settings")]
    [Tooltip("The Weight Of The Matching")]
    [Range(0, 1)]
    [SerializeField] private float weight = 1.0f;
    [Tooltip("The Smoothing Speed, Used For Smooth Results")]
    [SerializeField] private float smoothingSpeed = 15f;

    [Space(5)]
    [Tooltip("The Matching Type Local or World.")]
    [SerializeField] private MatchingType matchingType;

    [Space(5)]
    [Tooltip("The Object Which Will Be Matched.")]
    [SerializeField] private Transform constrained;

    [Tooltip("The Object To Match")]
    [SerializeField] private Transform toMatch;

    // Holds the last constrained objects rotation, Used For Smoothing.
    private Quaternion lastConstrainedRotation = Quaternion.identity;

    // Matching type Enum.
    public enum MatchingType
    {
        local,
        world,
    }

    void LateUpdate()
    {
        // Check if the matching type if local.
        if (matchingType == MatchingType.local)
        {
            // Calculate Target Rotation.
            Quaternion targetRotation = Quaternion.Slerp(lastConstrainedRotation,
                Quaternion.Lerp(constrained.localRotation, toMatch.localRotation, weight), smoothingSpeed * Time.deltaTime);

            // Set Constrained Local Rotation.
            constrained.localRotation = targetRotation;

            // Set Last Constrained Rotation.
            lastConstrainedRotation = targetRotation;
        }
        else if (matchingType == MatchingType.world)
        {
            // Calculate Target Rotation.
            Quaternion targetRotation = Quaternion.Slerp(lastConstrainedRotation,
                Quaternion.Lerp(constrained.rotation, toMatch.rotation, weight), smoothingSpeed * Time.deltaTime);

            // Set Constrained Rotation.
            constrained.rotation = targetRotation;

            // Set Last Constrained Rotation.
            lastConstrainedRotation = targetRotation;
        }
    }

    /// <summary>
    /// Returns the weight
    /// </summary>
    /// <returns></returns>
    public float GetWeight()
    {
        return weight;
    }

    /// <summary>
    /// Use for setting the weight.
    /// </summary>
    /// <param name="weight"></param>
    public void SetWeight(float weight)
    {
        this.weight = weight;
    }
}
