using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// All Changes Are Marked by "// CHANGES"
[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/WeaponScriptableObject")]
public class WeaponSO : ScriptableObject
{
    [Space]
    // Weapon Settings Header.
    [Header("Weapon Settings")]

    // Weapon UI Settings.
    [Header("Weapon UI")]
    [HideInInspector] public string weaponName;

    [HideInInspector] public Sprite weaponIconSprite;
    [HideInInspector] public Sprite crossHairSprite;

    [HideInInspector] public float weaponIconImageHeight;
    [HideInInspector] public float weaponIconImageWidth;

    // Weapon Mechanics Settings
    [Header("Weapon Mechanics & Type")]
    [HideInInspector] public float timeBetweenShot;
    [HideInInspector] public float reloadingAnimationTime;

    [HideInInspector] public int magazineSize;

    [HideInInspector] public WeaponType weaponType;
    [HideInInspector] public FiringType firingType;

    [HideInInspector] public float maxShootRange;
    [HideInInspector] public float projectileForce;

    [HideInInspector] public ForceMode rigidbodyForceMode;

    [HideInInspector] public GameObject projectilePrefab;

    [HideInInspector] public bool ejectShellOnShoot = true;

    // Weapon Clipping Settings.
    [Space]
    [Header("Weapon Clipping Settings")]
    [HideInInspector] public Vector3 clipProjectorPosition;

    [HideInInspector] public Vector3 newPosition;
    [HideInInspector] public Vector3 newRotation;

    [HideInInspector] public Vector3 boxCastSize;
    [HideInInspector] public Vector3 boxCastClippedSize;

    // Camera Positions
    [Space]
    [Header("Camera Positions")]
    [HideInInspector] public Vector3 cameraNormalPosition;
    [HideInInspector] public Vector3 cameraAimingPosition;
    [HideInInspector] public Vector3 cameraClippedPosition;

    // Recoil Settings
    [Space]
    [Header("Recoil & Sway Settings")]
    [HideInInspector] public float recoilX;
    [HideInInspector] public float recoilY;
    [HideInInspector] public float recoilZ;
    [HideInInspector] public float snappiness;
    [HideInInspector] public float returnSpeed;

    [HideInInspector] public Vector3 cinemachineRecoilImpulse;
    [HideInInspector] public float swayAmount = 1.5f;
    [HideInInspector] public float swaySmooth = 5f;

    // Weapon Offsets
    [Header("Offsets")]
    [Header("Position Offset added when childed to gunholder")]
    [HideInInspector] public Vector3 weaponPositionOffset;

    [HideInInspector] public float spineConstraintOffsetY;

    // Enums

    // Weapon Type Enum.
    public enum WeaponType
    {
        rifle,
        pistol,
        knife
    }
    
    // Firing Type Enum.
    public enum FiringType
    {
        raycast,
        projectile
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(WeaponSO))]
public class WeaponSOEditor : Editor
{
    // SerializedProperty fields.

    // Weapon UI Settings.
    SerializedProperty weaponName;
    SerializedProperty weaponIconSprite;
    SerializedProperty crossHairSprite;
    SerializedProperty weaponIconImageHeight;
    SerializedProperty weaponIconImageWidth;

    // Weapon Mechanics Settings
    SerializedProperty timeBetweenShot;
    SerializedProperty reloadingAnimationTime;
    SerializedProperty maxShootRange;
    SerializedProperty projectileForce;
    SerializedProperty magazineSize;
    SerializedProperty weaponType;
    SerializedProperty firingType;

    SerializedProperty rigidbodyForceMode;

    SerializedProperty projectilePrefab;

    SerializedProperty ejectShellOnShoot;

    // Weapon Clipping Settings.
    SerializedProperty clipProjectorPosition;
    SerializedProperty newPosition;
    SerializedProperty newRotation;
    SerializedProperty boxCastSize;
    SerializedProperty boxCastClippedSize;

    // Camera Positions
    SerializedProperty cameraNormalPosition;
    SerializedProperty cameraAimingPosition;
    SerializedProperty cameraClippedPosition;

    // Recoil Settings
    SerializedProperty recoilX;
    SerializedProperty recoilY;
    SerializedProperty recoilZ;
    SerializedProperty snappiness;
    SerializedProperty returnSpeed;
    SerializedProperty cinemachineRecoilImpulse;
    SerializedProperty swayAmount;
    SerializedProperty swaySmooth;

    // Weapon Offsets
    SerializedProperty weaponPositionOffset;
    SerializedProperty spineConstraintOffsetY;

    // OnEnable.
    private void OnEnable()
    {
        // Get all the properties.
        GetSerializedPropertyReferences();
    }

    /// <summary>
    /// Gets all SerializedProperty references.
    /// </summary>
    private void GetSerializedPropertyReferences()
    {
        // Weapon UI Settings.
        weaponName = serializedObject.FindProperty("weaponName");
        weaponIconSprite = serializedObject.FindProperty("weaponIconSprite");
        crossHairSprite = serializedObject.FindProperty("crossHairSprite");
        weaponIconImageHeight = serializedObject.FindProperty("weaponIconImageHeight");
        weaponIconImageWidth = serializedObject.FindProperty("weaponIconImageWidth");

        // Weapon Mechanics Settings
        timeBetweenShot = serializedObject.FindProperty("timeBetweenShot");
        reloadingAnimationTime = serializedObject.FindProperty("reloadingAnimationTime");
        maxShootRange = serializedObject.FindProperty("maxShootRange");
        projectileForce = serializedObject.FindProperty("projectileForce");
        magazineSize = serializedObject.FindProperty("magazineSize");
        weaponType = serializedObject.FindProperty("weaponType");
        firingType = serializedObject.FindProperty("firingType");

        rigidbodyForceMode = serializedObject.FindProperty("rigidbodyForceMode");

        projectilePrefab = serializedObject.FindProperty("projectilePrefab");

        ejectShellOnShoot = serializedObject.FindProperty("ejectShellOnShoot");

        // Recoil Settings
        clipProjectorPosition = serializedObject.FindProperty("clipProjectorPosition");
        newPosition = serializedObject.FindProperty("newPosition");
        newRotation = serializedObject.FindProperty("newRotation");
        boxCastSize = serializedObject.FindProperty("boxCastSize");
        boxCastClippedSize = serializedObject.FindProperty("boxCastClippedSize");

        // Camera Positions
        cameraNormalPosition = serializedObject.FindProperty("cameraNormalPosition");
        cameraAimingPosition = serializedObject.FindProperty("cameraAimingPosition");
        cameraClippedPosition = serializedObject.FindProperty("cameraClippedPosition");

        // Recoil Settings
        recoilX = serializedObject.FindProperty("recoilX");
        recoilY = serializedObject.FindProperty("recoilY");
        recoilZ = serializedObject.FindProperty("recoilZ");
        snappiness = serializedObject.FindProperty("snappiness");
        returnSpeed = serializedObject.FindProperty("returnSpeed");
        cinemachineRecoilImpulse = serializedObject.FindProperty("cinemachineRecoilImpulse");
        swayAmount = serializedObject.FindProperty("swayAmount");
        swaySmooth = serializedObject.FindProperty("swaySmooth");

        // Weapon Offsets
        weaponPositionOffset = serializedObject.FindProperty("weaponPositionOffset");
        spineConstraintOffsetY = serializedObject.FindProperty("spineConstraintOffsetY");
    }

    // override Inspector method.
    public override void OnInspectorGUI()
    {
        // draw default inspector.
        base.OnInspectorGUI();

        // update the serializedObject Update.
        serializedObject.Update();

        // Draw properties

        // Weapon UI Settings.
        DrawPropertyField(weaponName);
        DrawPropertyField(weaponIconSprite);
        DrawPropertyField(crossHairSprite);
        DrawPropertyField(weaponIconImageHeight);
        DrawPropertyField(weaponIconImageWidth);

        // Weapon Mechanics Settings
        DrawPropertyField(timeBetweenShot);
        DrawPropertyField(reloadingAnimationTime);
        DrawPropertyField(magazineSize);
        DrawPropertyField(weaponType);
        DrawPropertyField(firingType);

        // check if firing type is raycast.
        if (firingType.enumValueIndex == 0f)
        {
            // draw maxshoot range.
            DrawPropertyField(maxShootRange);
        }
        else
        {
            // draw projectile fields.
            DrawPropertyField(projectileForce);

            DrawPropertyField(rigidbodyForceMode);

            DrawPropertyField(projectilePrefab);
        }
        
        DrawPropertyField(ejectShellOnShoot);

        // Weapon Clipping Settings.
        DrawPropertyField(clipProjectorPosition);
        DrawPropertyField(newPosition);
        DrawPropertyField(newRotation);
        DrawPropertyField(boxCastSize);
        DrawPropertyField(boxCastClippedSize);

        // Camera Settings
        DrawPropertyField(cameraNormalPosition);
        DrawPropertyField(cameraAimingPosition);
        DrawPropertyField(cameraClippedPosition);

        // Recoil Settings
        DrawPropertyField(recoilX);
        DrawPropertyField(recoilY);
        DrawPropertyField(recoilZ);
        DrawPropertyField(snappiness);
        DrawPropertyField(returnSpeed);
        DrawPropertyField(cinemachineRecoilImpulse);
        DrawPropertyField(swayAmount);
        DrawPropertyField(swaySmooth);

        DrawPropertyField(weaponPositionOffset);
        DrawPropertyField(spineConstraintOffsetY);

        // apply the modified properties.
        serializedObject.ApplyModifiedProperties();
    }

    // draw Property field function.
    private void DrawPropertyField(SerializedProperty property)
    {
        // draw a property.
        EditorGUILayout.PropertyField(property);
    }
}
#endif
