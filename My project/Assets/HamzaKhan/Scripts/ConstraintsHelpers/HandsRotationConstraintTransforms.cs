using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HandsRotationConstraintTransforms
{
    [Header("Left Hand IK Transforms")]
    public Transform leftHandIKTransform;

    [Space(10)]
    public Transform leftHandIndex1ConstraintTransform;
    public Transform leftHandIndex2ConstraintTransform;
    public Transform leftHandIndex3ConstraintTransform;

    [Space(5)]
    public Transform leftHandMiddle1ConstraintTransform;
    public Transform leftHandMiddle2ConstraintTransform;
    public Transform leftHandMiddle3ConstraintTransform;

    [Space(5)]
    public Transform leftHandPinky1ConstraintTransform;
    public Transform leftHandPinky2ConstraintTransform;
    public Transform leftHandPinky3ConstraintTransform;

    [Space(5)]
    public Transform leftHandRing1ConstraintTransform;
    public Transform leftHandRing2ConstraintTransform;
    public Transform leftHandRing3ConstraintTransform;

    [Space(5)]
    public Transform leftHandThumb1ConstraintTransform;
    public Transform leftHandThumb2ConstraintTransform;
    public Transform leftHandThumb3ConstraintTransform;

    [Header("Right Hand IK Transforms")]
    public Transform rightHandIKTransform;

    [Space(10)]
    public Transform rightHandIndex1ConstraintTransform;
    public Transform rightHandIndex2ConstraintTransform;
    public Transform rightHandIndex3ConstraintTransform;

    [Space(5)]
    public Transform rightHandMiddle1ConstraintTransform;
    public Transform rightHandMiddle2ConstraintTransform;
    public Transform rightHandMiddle3ConstraintTransform;

    [Space(5)]
    public Transform rightHandPinky1ConstraintTransform;
    public Transform rightHandPinky2ConstraintTransform;
    public Transform rightHandPinky3ConstraintTransform;

    [Space(5)]
    public Transform rightHandRing1ConstraintTransform;
    public Transform rightHandRing2ConstraintTransform;
    public Transform rightHandRing3ConstraintTransform;

    [Space(5)]
    public Transform rightHandThumb1ConstraintTransform;
    public Transform rightHandThumb2ConstraintTransform;
    public Transform rightHandThumb3ConstraintTransform;
}
