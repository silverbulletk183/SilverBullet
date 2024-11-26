using UnityEngine;
using System;

[Serializable]
public class HandsIKTransform
{

    [Header("Left Hand IK Transforms")]
    public Transform leftHandIKTransform;

    [Space(5)]
    public Transform leftHandIndexIKTransform;
    public Transform leftHandMiddleIKTransform;
    public Transform leftHandPinkyIKTransform;
    public Transform leftHandRingIKTransform;
    public Transform leftHandThumbIKTransform;

    [Header("Right Hand IK Transforms")]
    public Transform rightHandIKTransform;

    [Space(5)]
    public Transform rightHandIndexIKTransform;
    public Transform rightHandMiddleIKTransform;
    public Transform rightHandPinkyIKTransform;
    public Transform rightHandRingIKTransform;
    public Transform rightHandThumbIKTransform;

}
