using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using System;

public class ConstraintsWeightModifier : MonoBehaviour
{


    [Header("The Weight For All The Constraints Assigned in the List")]
    [Range(0, 1)]
    [SerializeField] private float weight = 1.0f;

    // List to hold GameObjects
    [SerializeField] private List<GameObject> iKConstraints = new List<GameObject>();

    // Internal list to hold the actual constraint components
    private List<IRigConstraint> rigConstraints = new List<IRigConstraint>();
    private List<ICustomConstraints> customConstraints = new List<ICustomConstraints>();

    void Start()
    {
        // Initialize Constraints.
        InitializeConstraints();
    }

    void Update()
    {
        // Loop through each of the rig constraint.
        foreach (var rigConstraint in rigConstraints)
        {
            // Check if the constrant isn't null.
            if (rigConstraint != null)
            {
                // Set the weight property
                rigConstraint.weight = weight;
            }
        }

        // Loop through each of the custom constraint.
        foreach (ICustomConstraints customConstraint in customConstraints)
        {
            // Check if the constrant isn't null.
            if (customConstraints != null)
            {
                // Set the weight property
                customConstraint.SetWeight(weight);
            }
        }
    }

    /// <summary>
    /// Initializes the Constraints.
    /// </summary>
    private void InitializeConstraints()
    {
        // Clear the Contraints List.
        rigConstraints.Clear();

        // Loop through every gameobject 
        foreach (var obj in iKConstraints)
        {
            if (obj != null)
            {
                var rigConstraint = obj.GetComponent<IRigConstraint>();
                if (rigConstraint != null)
                {
                    rigConstraints.Add(rigConstraint);
                }
                else if (obj.GetComponent<ICustomConstraints>() != null)
                {
                    var customConstraint = obj.GetComponent<ICustomConstraints>();
                    customConstraints.Add(customConstraint);
                }
                else
                {
                    Debug.LogWarning($"GameObject {obj.name} does not have a constraint component.");
                }
            }
        }
    }

    public float GetWeight()
    {
        return weight;
    }

    public void SetWeight(float weight)
    {
        this.weight = weight;
    }

    public List<IRigConstraint> GetAffectedRigConstraints()
    {
        return rigConstraints;
    }

    public List<ICustomConstraints> GetAffectedCustomConstraints()
    {
        return customConstraints;
    }
}