using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using Cinemachine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WeaponBase : MonoBehaviour, IInteractable
{

    #region Variables

    [Space]
    [Header("References")]
    public WeaponSO weaponData;
    public AudioSource audioSource;
    public ParticleSystem muzzleFlashEffect;

    public ParticleSystem shellEjectEffect;

    public Transform firePoint;
    public Transform magParent;

    public HandsConstraintType handsConstraintType;

    public HandsIKTransform handsIKTargets;
    public HandsRotationConstraintTransforms handsRotationConstraintTransforms;

    public GameObject mag;

    public int totalAmmo;
    public int currentAmmo;


    public CinemachineImpulseSource recoilImpulseSource;
    
    [Space]
    [Header("Audio")]
    public AudioClip magInOutAudioClip;
    public AudioClip reloadAudioClip;

    [Space]
    [Header("Animator")]
    public Animator animator;

    // RECOIL.
    // Rotation
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    [Space]
    [Header("Interactable Settings")]
    public string message = "Swap (This weapons Name)";

    [Space()]
    [Header("Callback Events")]
    [Space(5)]
    public UnityEvent onShoot;
    public UnityEvent onAimEnter;
    public UnityEvent onAimExit;
    public UnityEvent onReloadStart;
    public UnityEvent onInteraction;


    private bool isCurrentWeapon;
    private PlayerController controller;

    private Quaternion initialRot;

    #endregion

    #region General

    // update is called once per frame.
    public virtual void Update()
    {
        // Check if enable recoil.
        if (isCurrentWeapon == true)
        {
            // Lerp the target rotation to zero.
            targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, weaponData.returnSpeed * Time.deltaTime);

            // Slerp the current rotation to the target rotation.
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, weaponData.snappiness * Time.deltaTime);

            float lookX = controller.Input.Player.Look.ReadValue<Vector2>().x * weaponData.swayAmount;
            float lookY = controller.Input.Player.Look.ReadValue<Vector2>().y * weaponData.swayAmount;

            Quaternion swayRotation;

            if (controller.GetXRotation() > 89f || controller.GetXRotation() < -89f)
            {
                Quaternion finalRot = Quaternion.AngleAxis(-lookX, Vector3.forward);

                swayRotation = Quaternion.Slerp(transform.localRotation, finalRot * initialRot * Quaternion.Euler(currentRotation), Time.deltaTime * weaponData.swaySmooth);
            }
            else
            {
                Quaternion finalRot = Quaternion.AngleAxis(-lookY, Vector3.right) * Quaternion.AngleAxis(-lookX, Vector3.forward);

                swayRotation = Quaternion.Slerp(transform.localRotation, finalRot * initialRot * Quaternion.Euler(currentRotation), Time.deltaTime * weaponData.swaySmooth);
            }

            transform.localRotation = swayRotation;
        }
    }

  
    public void Shoot()
    {
        // Invoke Event
        onShoot.Invoke();

        // Check if we should automatically eject a shell out.
        if (weaponData.ejectShellOnShoot == true)
        {
            shellEjectEffect.Play();
        }

        // Recoil
        // Set the target rotation to the recoil calculation...
        targetRotation += new Vector3(weaponData.recoilX, Random.Range(-weaponData.recoilY, weaponData.recoilY), Random.Range(-weaponData.recoilZ, weaponData.recoilZ));

        Vector3 impulseRecoil = weaponData.cinemachineRecoilImpulse;

        // Generate impulse.
        recoilImpulseSource.GenerateImpulse(new Vector3(Random.Range(-impulseRecoil.x, impulseRecoil.x)
            , Random.Range(-impulseRecoil.y, impulseRecoil.y)
            , impulseRecoil.z));

        // Animator
        animator.SetTrigger("Shoot");
    }


    // Reload
    public virtual void StartReloading()
    {
        // Invoke Event
        onReloadStart.Invoke();

        // Set animator trigger.
        animator.SetTrigger("Reload");
    }

    public void SetMagParentToHand(AnimationEvent animationEventData)
    {
        // Play the mag out audio.
        AudioSource.PlayClipAtPoint(magInOutAudioClip, mag.transform.position, 1.0f);

        // Check if the left hand should be the parent.
        if (animationEventData.stringParameter == "LeftHand")
        {
            // The Hand to parent is left.
            if (handsIKTargets.leftHandIKTransform != null) mag.transform.SetParent(handsIKTargets.leftHandIKTransform);
            else if (handsRotationConstraintTransforms.leftHandIKTransform != null) mag.transform.SetParent(handsRotationConstraintTransforms.leftHandIKTransform);
        }
        else
        {
            // The Hand to parent is right.
            if (handsIKTargets.rightHandIKTransform != null) mag.transform.SetParent(handsIKTargets.rightHandIKTransform);
            else if (handsRotationConstraintTransforms.rightHandIKTransform != null) mag.transform.SetParent(handsRotationConstraintTransforms.rightHandIKTransform);
        }
    }


    public void SetMagToMagParent()
    {
        // Play the mag in audio.
        AudioSource.PlayClipAtPoint(magInOutAudioClip, mag.transform.position, 1.0f);

        mag.transform.SetParent(magParent);
        mag.transform.position = magParent.position;
        mag.transform.rotation = magParent.rotation;

        // For Normal weapons will only be calling this function, custom ones may or may not, we need to do the bullets loading code.
        if (totalAmmo < weaponData.magazineSize)
        {
            currentAmmo = totalAmmo;
            totalAmmo = 0;
        }
        else
        {
            currentAmmo = weaponData.magazineSize;
            totalAmmo -= weaponData.magazineSize;
        }
    }

   
    public void SpawnMagAtMagPosition()
    {
        GameObject spawnedMag = Instantiate(mag, mag.transform.position, mag.transform.rotation);

        spawnedMag.transform.SetParent(null);

        if (spawnedMag.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
        }
        else
        {
            Rigidbody newRb = spawnedMag.AddComponent<Rigidbody>();
            newRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        DecalDestroyer decalDestroyer = spawnedMag.AddComponent<DecalDestroyer>();
        decalDestroyer.lifeTime = 5f;

        // Set the mag gameobject to disabled.
        mag.SetActive(false);
    }


    public void TakeOutNewMag()
    {
        mag.SetActive(true);
    }

    public void PlayAudioClip(AnimationEvent animationData)
    {
        AudioClip providedReloadAudioClip = (AudioClip)animationData.objectReferenceParameter;

        if (providedReloadAudioClip != null)
        {
            AudioSource.PlayClipAtPoint(providedReloadAudioClip, transform.position, 1.0f);
        }
        else
        {
            AudioSource.PlayClipAtPoint(reloadAudioClip, transform.position, 1.0f);
        }
    }

    public void PlayShellEjectEffect()
    {
        shellEjectEffect.Play();
    }

    // Return The Interaction Range Message
    public string Message()
    {
        return message;
    }

    // Interact
    public void Interact(PlayerController playerController)
    {
        // Swap The Weapon.
        //playerController.SwapWeapon(this);

        // Invoke Event.
        onInteraction.Invoke();
    }

    public void SetCurrentWeapon(bool currentWeapon)
    {
        isCurrentWeapon = currentWeapon;

        if (isCurrentWeapon == true)
        {
            initialRot = transform.localRotation;
        }
        else
        {
            onInteraction.Invoke();
        }
    }

    public bool IsCurrentWeapon()
    {
        return isCurrentWeapon;
    }

    public void SetControllerReference(PlayerController controller)
    {
        this.controller = controller;
    }

    public PlayerController GetPlayer()
    {
        return controller;
    }

    public void OnAimEnter()
    {
        onAimEnter.Invoke();
    }

    public void OnAimExit()
    {
        onAimExit.Invoke();
    }

    #endregion
}

#region Editor

#if UNITY_EDITOR

[CustomEditor(typeof(WeaponBase))]
public class WeaponBaseEditor : Editor
{
    
    #region Weapon Setup Editor Window

    // Weapon Setup Window.
    public class SetupHelperEditorWindow : EditorWindow
    {

        #region Variables

        // The references from the class which opened the window....
        private Transform parentTransform;
        private WeaponBase weaponBaseClass;

        // Names of Hand IK targets
        private string leftHandIKTargetName = "LeftHandIKTarget";
        private string leftHandIndexIKTargetName = "LeftHandIndexIKTarget";
        private string leftHandMiddleIKTargetName = "LeftHandMiddleIKTarget";
        private string leftHandPinkyIKTargetName = "LeftHandPinkyIKTarget";
        private string leftHandRingIKTargetName = "LeftHandRingIKTarget";
        private string leftHandThumbIKTarget = "LeftHandThumbIKTarget";

        private string rightHandIKTargetName = "RightHandIKTarget";
        private string rightHandIndexIKTargetName = "RightHandIndexIKTarget";
        private string rightHandMiddleIKTargetName = "RightHandMiddleIKTarget";
        private string rightHandPinkyIKTargetName = "RightHandPinkyIKTarget";
        private string rightHandRingIKTargetName = "RightHandRingIKTarget";
        private string rightHandThumbIKTarget = "RightHandThumbIKTarget";

        // Names of Fingers Rotation Constraints targets
        // Left Hand's Fingers.
        private string leftHandIndex1ConstraintTargetName = "LeftHandIndex1ConstraintTarget";
        private string leftHandIndex2ConstraintTargetName = "LeftHandIndex2ConstraintTarget";
        private string leftHandIndex3ConstraintTargetName = "LeftHandIndex3ConstraintTarget";

        private string leftHandMiddle1ConstraintTargetName = "LeftHandMiddle1ConstraintTarget";
        private string leftHandMiddle2ConstraintTargetName = "LeftHandMiddle2ConstraintTarget";
        private string leftHandMiddle3ConstraintTargetName = "LeftHandMiddle3ConstraintTarget";

        private string leftHandPinky1ConstraintTargetName = "LeftHandPinky1ConstraintTarget";
        private string leftHandPinky2ConstraintTargetName = "LeftHandPinky2ConstraintTarget";
        private string leftHandPinky3ConstraintTargetName = "LeftHandPinky3ConstraintTarget";

        private string leftHandRing1ConstraintTargetName = "LeftHandRing1ConstraintTarget";
        private string leftHandRing2ConstraintTargetName = "LeftHandRing2ConstraintTarget";
        private string leftHandRing3ConstraintTargetName = "LeftHandRing3ConstraintTarget";

        private string leftHandThumb1ConstraintTargetName = "LeftHandThumb1ConstraintTarget";
        private string leftHandThumb2ConstraintTargetName = "LeftHandThumb2ConstraintTarget";
        private string leftHandThumb3ConstraintTargetName = "LeftHandThumb3ConstraintTarget";

        // Right Hand's Fingers.
        private string rightHandIndex1ConstraintTargetName = "RightHandIndex1ConstraintTarget";
        private string rightHandIndex2ConstraintTargetName = "RightHandIndex2ConstraintTarget";
        private string rightHandIndex3ConstraintTargetName = "RightHandIndex3ConstraintTarget";

        private string rightHandMiddle1ConstraintTargetName = "RightHandMiddle1ConstraintTarget";
        private string rightHandMiddle2ConstraintTargetName = "RightHandMiddle2ConstraintTarget";
        private string rightHandMiddle3ConstraintTargetName = "RightHandMiddle3ConstraintTarget";

        private string rightHandPinky1ConstraintTargetName = "RightHandPinky1ConstraintTarget";
        private string rightHandPinky2ConstraintTargetName = "RightHandPinky2ConstraintTarget";
        private string rightHandPinky3ConstraintTargetName = "RightHandPinky3ConstraintTarget";

        private string rightHandRing1ConstraintTargetName = "RightHandRing1ConstraintTarget";
        private string rightHandRing2ConstraintTargetName = "RightHandRing2ConstraintTarget";
        private string rightHandRing3ConstraintTargetName = "RightHandRing3ConstraintTarget";

        private string rightHandThumb1ConstraintTargetName = "RightHandThumb1ConstraintTarget";
        private string rightHandThumb2ConstraintTargetName = "RightHandThumb2ConstraintTarget";
        private string rightHandThumb3ConstraintTargetName = "RightHandThumb3ConstraintTarget";


        private string weaponScriptableObjectName = "WeaponScriptableObject";

        private GameObject muzzleFlashEffectGameObject = null;
        private GameObject shellEjectEffectGameObject = null;
        private GameObject mag = null;

        private Transform meshTransform = null;

        private AudioClip firingAudioClip = null;

        private int pageNumber = 0;


        SerializedProperty handsConstraintTypeProperty;


        private Vector2 scrollViewAmount;

        #endregion

        // Open Window.
        /// <summary>
        /// transform : the transform with the weapon base class.
        /// weaponBase : the weaponBase that needs setup.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="weaponBase"></param>
        public static void OpenWindow(Transform transform, WeaponBase weaponBase)
        {
            // Get the window.
            SetupHelperEditorWindow window = GetWindow<SetupHelperEditorWindow>();

            // Set window stuff...
            window.titleContent = new GUIContent("Weapon Setup");
            window.parentTransform = transform.GetChild(0);
            window.weaponBaseClass = weaponBase;
            window.pageNumber = 0;
            window.Show();
        }

        private void OnGUI()
        {
            if (weaponBaseClass == null)
            {
                // Weapon Base Class was destroyed or this window was opened while assigning weapon base to null.
                Close();
            }

            scrollViewAmount = GUILayout.BeginScrollView(scrollViewAmount);

            // Some Space To Start Nicely.
            GUILayout.Space(5);

            // Begin Horizontal.
            GUILayout.BeginHorizontal();

            // 5 Pixels Space.
            GUILayout.Space(5);

            // Create a new Style.
            GUIStyle stepLabelStyle = new GUIStyle();

            // Set the Styles that we want to change.
            stepLabelStyle.fontSize = 15;
            stepLabelStyle.normal.textColor = Color.white;
            stepLabelStyle.fontStyle = FontStyle.Italic;

            // Display the current step + / + the amount of steps.
            GUILayout.Label("Step " + (pageNumber + 1) + "/" + 4, stepLabelStyle);

            // End Horizontal.
            GUILayout.EndHorizontal();

            // 10 pixels space.
            GUILayout.Space(10);

            // If The first page, Then draw the first page
            if ((pageNumber + 1) == 1) // Page num one.
            {
                DrawPageOne();
            }
            else if ((pageNumber + 1) == 2) // Page num two.
            {
                DrawPageTwo();
            }
            else if ((pageNumber + 1) == 3) // Page num three.
            {
                DrawPageThree();
            }
            else if ((pageNumber + 1) == 4) // Page num four
            {
                DrawPageFour();
            }

            GUILayout.BeginHorizontal();

            // Draw previous button only if the page number isn't 0.
            if ((pageNumber + 1) != 0)
            {
                if (GUILayout.Button("Previous Step"))
                {
                    PreviousPage();
                }
            }

            // Draw next button only if the page number isn't 4, Otherwise draw close button.
            if ((pageNumber + 1) != 4)
            {
                if (GUILayout.Button("Next Step"))
                {
                    NextPage();
                }
            }
            else
            {
                if (GUILayout.Button("Close"))
                {
                    // Finish Setup Code.
                    Close();
                }
            }

            // End horizontal.
            GUILayout.EndHorizontal();

            // Quite a bit of space.
            GUILayout.Space(70);

            // End Scroll View.
            GUILayout.EndScrollView();
        }

        #region Page Changing Functions

        /// <summary>
        /// Changes the page number by one.
        /// </summary>
        private void NextPage()
        {
            pageNumber += 1;
        }

        /// <summary>
        /// Changes the page number by negative one.
        /// </summary>
        private void PreviousPage()
        {
            pageNumber -= 1;
        }

        #endregion

        #region All Page Drawing Functions.

        /// <summary>
        /// Draws the page one.
        /// </summary>
        private void DrawPageOne()
        {
            // Title.
            GUILayout.Label("Create Hands IK Targets", EditorStyles.boldLabel);

            // Space.
            GUILayout.Space(5);

            // Tell what the transform field is for.
            GUILayout.Label("Transform In Which To Create Hands IK Targets");

            // The transform everything will be childed to.
            parentTransform = EditorGUILayout.ObjectField(parentTransform, typeof(Transform), true) as Transform;

            // Space.
            GUILayout.Space(20);

            // Draw the Hands Constraint Type Field.
            weaponBaseClass.handsConstraintType = (HandsConstraintType)EditorGUILayout.EnumPopup("Hands Constraint Type", weaponBaseClass.handsConstraintType);

            // Space.
            GUILayout.Space(10);

            // Check if the hands constraint type if ikbased fingers.
            if (weaponBaseClass.handsConstraintType == HandsConstraintType.IKBasedFingers)
            {
                // Show text fields for IK target names
                DrawTextField("Left Hand IK Target Name", ref leftHandIKTargetName);
                DrawTextField("Left Hand Index IK Target Name", ref leftHandIndexIKTargetName);
                DrawTextField("Left Hand Middle IK Target Name", ref leftHandMiddleIKTargetName);
                DrawTextField("Left Hand Pinky IK Target Name", ref leftHandPinkyIKTargetName);
                DrawTextField("Left Hand Ring IK Target Name", ref leftHandRingIKTargetName);
                DrawTextField("Left Hand Thumb IK Target Name", ref leftHandThumbIKTarget);

                DrawTextField("Right Hand IK Target Name", ref rightHandIKTargetName);
                DrawTextField("Right Hand Index IK Target Name", ref rightHandIndexIKTargetName);
                DrawTextField("Right Hand Middle IK Target Name", ref rightHandMiddleIKTargetName);
                DrawTextField("Right Hand Pinky IK Target Name", ref rightHandPinkyIKTargetName);
                DrawTextField("Right Hand Ring IK Target Name", ref rightHandRingIKTargetName);
                DrawTextField("Right Hand Thumb IK Target Name", ref rightHandThumbIKTarget);
            }
            else
            {
                // Show text fields for IK target names
                DrawTextField("Left Hand IK Target Name", ref leftHandIKTargetName);
                DrawTextField("Left Hand Index1 IK Target Name", ref leftHandIndex1ConstraintTargetName);
                DrawTextField("Left Hand Index2 IK Target Name", ref leftHandIndex2ConstraintTargetName);
                DrawTextField("Left Hand Index3 IK Target Name", ref leftHandIndex3ConstraintTargetName);

                DrawTextField("Left Hand Middle1 IK Target Name", ref leftHandMiddle1ConstraintTargetName);
                DrawTextField("Left Hand Middle2 IK Target Name", ref leftHandMiddle2ConstraintTargetName);
                DrawTextField("Left Hand Middle3 IK Target Name", ref leftHandMiddle3ConstraintTargetName);

                DrawTextField("Left Hand Pinky1 IK Target Name", ref leftHandPinky1ConstraintTargetName);
                DrawTextField("Left Hand Pinky2 IK Target Name", ref leftHandPinky2ConstraintTargetName);
                DrawTextField("Left Hand Pinky3 IK Target Name", ref leftHandPinky3ConstraintTargetName);

                DrawTextField("Left Hand Ring1 IK Target Name", ref leftHandRing1ConstraintTargetName);
                DrawTextField("Left Hand Ring2 IK Target Name", ref leftHandRing2ConstraintTargetName);
                DrawTextField("Left Hand Ring3 IK Target Name", ref leftHandRing3ConstraintTargetName);

                DrawTextField("Left Hand Thumb1 IK Target Name", ref leftHandThumb1ConstraintTargetName);
                DrawTextField("Left Hand Thumb2 IK Target Name", ref leftHandThumb2ConstraintTargetName);
                DrawTextField("Left Hand Thumb3 IK Target Name", ref leftHandThumb3ConstraintTargetName);


                DrawTextField("Right Hand Index1 IK Target Name", ref rightHandIndex1ConstraintTargetName);
                DrawTextField("Right Hand Index2 IK Target Name", ref rightHandIndex2ConstraintTargetName);
                DrawTextField("Right Hand Index3 IK Target Name", ref rightHandIndex3ConstraintTargetName);

                DrawTextField("Right Hand Middle1 IK Target Name", ref rightHandMiddle1ConstraintTargetName);
                DrawTextField("Right Hand Middle2 IK Target Name", ref rightHandMiddle2ConstraintTargetName);
                DrawTextField("Right Hand Middle3 IK Target Name", ref rightHandMiddle3ConstraintTargetName);

                DrawTextField("Right Hand Pinky1 IK Target Name", ref rightHandPinky1ConstraintTargetName);
                DrawTextField("Right Hand Pinky2 IK Target Name", ref rightHandPinky2ConstraintTargetName);
                DrawTextField("Right Hand Pinky3 IK Target Name", ref rightHandPinky3ConstraintTargetName);

                DrawTextField("Right Hand Ring1 IK Target Name", ref rightHandRing1ConstraintTargetName);
                DrawTextField("Right Hand Ring2 IK Target Name", ref rightHandRing2ConstraintTargetName);
                DrawTextField("Right Hand Ring3 IK Target Name", ref rightHandRing3ConstraintTargetName);

                DrawTextField("Right Hand Thumb1 IK Target Name", ref rightHandThumb1ConstraintTargetName);
                DrawTextField("Right Hand Thumb2 IK Target Name", ref rightHandThumb2ConstraintTargetName);
                DrawTextField("Right Hand Thumb3 IK Target Name", ref rightHandThumb3ConstraintTargetName);
            }

            // Space
            GUILayout.Space(20);

            // Draw a Button for Creating Hands IK Targets
            if (GUILayout.Button("Create Hands IK Targets", GUILayout.Height(25)))
            {
                // Create the IK targets.
                CreateHandsConstraintTargets();

                // And go to the next page.
                NextPage();
            }

            // Space
            GUILayout.Space(10);
        }

        /// <summary>
        /// Draws the page two.
        /// </summary>
        private void DrawPageTwo()
        {
            // Page name Label.
            GUILayout.Label("Create Weapon Scriptable Object Settings Asset", EditorStyles.boldLabel);

            // Space.
            GUILayout.Space(10);

            // Draw the text field for the name of the weapon SO that shall be created.
            DrawTextField("Weapon Scriptable Object Name", ref weaponScriptableObjectName);

            // Space
            GUILayout.Space(10);

            // Draw a Button for Creating the Weapon Scriptable Object (SO) Settings.
            if (GUILayout.Button("Create " + weaponScriptableObjectName, GUILayout.Height(25)))
            {
                // Function Handles Weapon SO Creation, Returns true if created, And false if cancelled.
                bool created = CreateWeaponSOAsset("Select Folder To Create " + weaponScriptableObjectName, weaponScriptableObjectName);

                // Check if we have created a weapon SO Asset.
                if (created == true)
                {
                    // Next Page
                    NextPage();
                }
            }

            // Space
            GUILayout.Space(10);
        }

        /// <summary>
        /// Draws the page three.
        /// </summary>
        private void DrawPageThree()
        {
            // Title
            GUILayout.Label("Create Fire Point, Muzzle Flash Effect & Mag Parent", EditorStyles.boldLabel);

            // 10 pixels space
            GUILayout.Space(10);

            // Label for telling what the field below is for.
            EditorGUILayout.LabelField("Assign your weapons Main Mesh Parent(Objects Will be Childed to that)");

            // The transform everything will be childed to.
            meshTransform = EditorGUILayout.ObjectField(meshTransform, typeof(Transform), true) as Transform;

            // 10 pixels space
            GUILayout.Space(10);

            #region Fire Point

            // Draw UI Elements for Fire Point creation.

            // Label for telling what the field below is for.
            EditorGUILayout.LabelField("Assign a Firing Audio Clip");

            // The transform everything will be childed to.
            firingAudioClip = EditorGUILayout.ObjectField(firingAudioClip, typeof(AudioClip), true) as AudioClip;

            // Draw a Button for creating the fire point.
            if (GUILayout.Button("Create Fire Point GameObject", GUILayout.Height(25)))
            {
                // Check if the mesh transform is not assigned, and return if not.
                if (meshTransform == null)
                {
                    Debug.LogWarning("Please Assign The Mesh Transform");
                    return;
                }
                else if (firingAudioClip == null) // And Also if firing audio clip hasn't been assigned.
                {
                    Debug.LogWarning("Please Assign A weapon firing audio clip");
                    return;
                }

                // Create the Fire Point GameObject.
                GameObject firePoint = new GameObject("FirePoint");

                // Set the parent.
                firePoint.transform.parent = meshTransform;

                // Add an AudioSource Component.
                AudioSource firingAudioSource = firePoint.AddComponent<AudioSource>();

                // Relevant Parameters of the Firing Audio Source.
                firingAudioSource.clip = firingAudioClip;
                firingAudioSource.playOnAwake = false;
                firingAudioSource.spatialBlend = 1.0f;

                // Set the weapon's References.
                weaponBaseClass.firePoint = firePoint.transform;
                weaponBaseClass.audioSource = firingAudioSource;
            }

            // 10 pixels space
            GUILayout.Space(10);

            #endregion

            #region Muzzle Flash

            // Draw UI Elements for Muzzle Flash Effect creation.

            // Label for telling what the field below is for.
            EditorGUILayout.LabelField("Assign a muzzle flash which shall be spawned");

            // Muzzle Flash Effect to spawn into the mesh.
            muzzleFlashEffectGameObject = EditorGUILayout.ObjectField(muzzleFlashEffectGameObject, typeof(GameObject), true) as GameObject;

            // Draw a Button for creating the fire muzzle Flash Effect.
            if (GUILayout.Button("Create Muzzle Flash Effect", GUILayout.Height(25)))
            {
                // Check if the muzzle flash gameobject has been assigned.
                if (muzzleFlashEffectGameObject != null)
                {
                    // Check if the mesh transform is not assigned, and return if not.
                    if (meshTransform == null)
                    {
                        Debug.LogWarning("Please Assign The Mesh Transform");
                        return;
                    }

                    // Create A new Gameobject or SPAWN.
                    GameObject muzzleFlashEffect = Instantiate(muzzleFlashEffectGameObject);

                    // Set the Name Of the GameObject
                    muzzleFlashEffect.name = "MuzzleFlashEffect";

                    // Set the parent, position, rotation.
                    muzzleFlashEffect.transform.parent = meshTransform;
                    muzzleFlashEffect.transform.position = meshTransform.position;
                    muzzleFlashEffect.transform.rotation = meshTransform.rotation;

                    // Set the weapon's References.
                    weaponBaseClass.muzzleFlashEffect = muzzleFlashEffect.GetComponent<ParticleSystem>();

                    // Selection
                    Selection.activeGameObject = muzzleFlashEffect;
                }
                else
                {
                    // Log a warning letting the user know that they must assign a muzzle flash to create.
                    Debug.LogWarning("To Create a Muzzle Flash GameObject, Please Assign One.");
                }
            }

            // 10 pixels space
            GUILayout.Space(10);

            #endregion

            #region Shell Eject Effect

            // Draw UI Elements for Shell Eject Effect creation.

            // Label for telling what the field below is for.
            EditorGUILayout.LabelField("Assign a Shell Eject Effect which shall be spawned");

            // Shell Eject effect to spawn into the mesh.
            shellEjectEffectGameObject = EditorGUILayout.ObjectField(shellEjectEffectGameObject, typeof(GameObject), true) as GameObject;

            // Draw a Button for creating the shell eject Effect.
            if (GUILayout.Button("Create Shell Eject Effect", GUILayout.Height(25)))
            {
                // Check if the muzzle flash gameobject has been assigned.
                if (shellEjectEffectGameObject != null)
                {
                    // Check if the mesh transform is not assigned, and return if not.
                    if (meshTransform == null)
                    {
                        // Log Warning.
                        Debug.LogWarning("Please Assign The Mesh Transform");
                        return;
                    }

                    // Create A new Gameobject or SPAWN.
                    GameObject shellEjectEffect = Instantiate(shellEjectEffectGameObject);

                    // Set the Name Of the GameObject
                    shellEjectEffect.name = "ShellEjectEffect";

                    // Set the parent, position, rotation.
                    shellEjectEffect.transform.parent = meshTransform;
                    shellEjectEffect.transform.position = meshTransform.position;
                    shellEjectEffect.transform.rotation = meshTransform.rotation;

                    // Set the weapon's References.
                    weaponBaseClass.shellEjectEffect = shellEjectEffect.GetComponent<ParticleSystem>();

                    // Selection
                    Selection.activeGameObject = shellEjectEffect;
                }
                else
                {
                    // Log a warning letting the user know that they must assign a muzzle flash to create.
                    Debug.LogWarning("To Create a Shell Efect Flash GameObject, Please Assign One.");
                }
            }

            // 10 pixels space
            GUILayout.Space(10);

            #endregion

            #region Mag parent

            // Label for telling what the field below is for.
            EditorGUILayout.LabelField("Assign your weapons mag for creating a mag parent");

            // The transform everything will be childed to.
            mag = EditorGUILayout.ObjectField(mag, typeof(GameObject), true) as GameObject;

            // Draw a Button for creating a mag parent.
            if (GUILayout.Button("Create Mag Parent", GUILayout.Height(25)))
            {
                if (mag != null)
                {
                    // Create a new GameObject for the MagParent.
                    GameObject magParent = new GameObject("MagParent");

                    // Set Mag Parent's Position and rotation.
                    magParent.transform.position = mag.transform.position;
                    magParent.transform.rotation = mag.transform.rotation;

                    // Set Mag Parent's Parent.
                    magParent.transform.parent = mag.transform.parent;

                    // Child the mag to the mag parent.
                    mag.transform.parent = magParent.transform;

                    // Set script references.
                    weaponBaseClass.mag = mag;
                    weaponBaseClass.magParent = magParent.transform;
                }
                else
                {
                    // Tell the user that they must assign there weapon's mag
                    Debug.LogWarning("Please Assign your weapon's mag, So that The Mag Parent can be created");
                }
            }

            // 10 pixels space
            GUILayout.Space(10);

            #endregion
        }

        /// <summary>
        /// Draws the page four.
        /// </summary>
        private void DrawPageFour()
        {
            // Title
            GUILayout.Label("Finishing Weapon Setup", EditorStyles.boldLabel);

            // 10 pixels space
            GUILayout.Space(10);

            // Label.
            GUILayout.Label("Add a Cinemachine Impulse Source for Camera Recoil Effect");

            // Button for adding the CinemachineImpulse source component.
            if (GUILayout.Button("Add Recoil Impulse Component(Cinemachine)", GUILayout.Height(25)))
            {
                // Create Recoil Impulse Source.
                CinemachineImpulseSource recoilImpulseSource = weaponBaseClass.gameObject.AddComponent<CinemachineImpulseSource>();

                // Set Script references.
                weaponBaseClass.recoilImpulseSource = recoilImpulseSource;
            }

            // 10 pixels space
            GUILayout.Space(10);

            // Label.
            GUILayout.Label("Add a Box collider, Adjust the Bounds to fit,");
            GUILayout.Label("or if you want you can add a different type of Collider yourself");

            // Button for adding a box collider.
            if (GUILayout.Button("Add Box Collider", GUILayout.Height(25)))
            {
                // Add a Box Collider Component.
                weaponBaseClass.gameObject.AddComponent<BoxCollider>();
            }

            // 10 pixels space
            GUILayout.Space(10);

            // Label.
            GUILayout.Label("Add an Animator, So you can create animations for this weapon");

            // Button For adding animator component.
            if (GUILayout.Button("Add Animator Component", GUILayout.Height(25)))
            {
                // Add Animator Component.
                Animator weaponAnimator = weaponBaseClass.gameObject.AddComponent<Animator>();

                // Set weapon Base Classes Reference of the animator.
                weaponBaseClass.animator = weaponAnimator;
            }

            // 10 pixels space
            GUILayout.Space(10);

            // Label, Set your weapons current ammo and total ammo by default.
            GUILayout.Label("Set your weapons current ammo and total ammo.");

            // Ammo fields.
            weaponBaseClass.totalAmmo = EditorGUILayout.IntField("Current Ammo", weaponBaseClass.totalAmmo);
            weaponBaseClass.currentAmmo = EditorGUILayout.IntField("Total Ammo", weaponBaseClass.currentAmmo);

            // 10 pixels space
            GUILayout.Space(10);

            // Label, Set your Weapon's reloading audio clips.
            GUILayout.Label("Set your Weapon's reloading audio clips");

            // Field for the audio clip
            GUILayout.Label("Mag In/Out");
            weaponBaseClass.magInOutAudioClip = EditorGUILayout.ObjectField(weaponBaseClass.magInOutAudioClip, typeof(AudioClip), true) as AudioClip;

            GUILayout.Space(5);

            GUILayout.Label("Reload");
            weaponBaseClass.reloadAudioClip = EditorGUILayout.ObjectField(weaponBaseClass.reloadAudioClip, typeof(AudioClip), true) as AudioClip;

            // 10 pixels space
            GUILayout.Space(10);

            // Field for interaction message.
            weaponBaseClass.message = EditorGUILayout.TextField("Interaction Prompt", weaponBaseClass.message);

            // 10 pixels space
            GUILayout.Space(10);
        }

        #endregion

        /// <summary>
        /// Helper Function Used for Drawing a Text Field.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="text"></param>
        private void DrawTextField(string label, ref string text)
        {
            // Begin Horizontal.
            GUILayout.BeginHorizontal();

            // Show the Label or name of the Field.
            GUILayout.Label(label, GUILayout.Width(200));

            // Show the Text of the field.
            text = EditorGUILayout.TextField(text);

            // End Horizontal.
            GUILayout.EndHorizontal();
        }

        #region Creation Functions

        /// <summary>
        /// Creates all ik Targets & assgns the references to the HandsIKTargets.
        /// </summary>
        private void CreateHandsConstraintTargets()
        {
            // Create the HandsIKTargets Parent.
            GameObject handsIKTargets = new GameObject("HandsIKTargets");

            // Create LeftHandIKTarget and RightHandIKTarget
            GameObject leftHandIKTarget = new GameObject(leftHandIKTargetName);
            GameObject rightHandIKTarget = new GameObject(rightHandIKTargetName);

            if (weaponBaseClass.handsConstraintType == HandsConstraintType.IKBasedFingers)
            {
                // Create children for LeftHandIKTarget
                GameObject leftIndex = new GameObject(leftHandIndexIKTargetName);
                GameObject leftMiddle = new GameObject(leftHandMiddleIKTargetName);
                GameObject leftPinky = new GameObject(leftHandPinkyIKTargetName);
                GameObject leftRing = new GameObject(leftHandRingIKTargetName);
                GameObject leftThumb = new GameObject(leftHandThumbIKTarget);

                // Create children for RightHandIKTarget
                GameObject rightIndex = new GameObject(rightHandIndexIKTargetName);
                GameObject rightMiddle = new GameObject(rightHandMiddleIKTargetName);
                GameObject rightPinky = new GameObject(rightHandPinkyIKTargetName);
                GameObject rightRing = new GameObject(rightHandRingIKTargetName);
                GameObject rightThumb = new GameObject(rightHandThumbIKTarget);

                // Set parents for children
                leftIndex.transform.parent = leftHandIKTarget.transform;
                leftMiddle.transform.parent = leftHandIKTarget.transform;
                leftPinky.transform.parent = leftHandIKTarget.transform;
                leftRing.transform.parent = leftHandIKTarget.transform;
                leftThumb.transform.parent = leftHandIKTarget.transform;

                rightIndex.transform.parent = rightHandIKTarget.transform;
                rightMiddle.transform.parent = rightHandIKTarget.transform;
                rightPinky.transform.parent = rightHandIKTarget.transform;
                rightRing.transform.parent = rightHandIKTarget.transform;
                rightThumb.transform.parent = rightHandIKTarget.transform;

                // Set Created Hands IK Target To Hands IK Target Reference.
                // Left Hand
                weaponBaseClass.handsIKTargets.leftHandIKTransform = leftHandIKTarget.transform;
                weaponBaseClass.handsIKTargets.leftHandIndexIKTransform = leftIndex.transform;
                weaponBaseClass.handsIKTargets.leftHandMiddleIKTransform = leftMiddle.transform;
                weaponBaseClass.handsIKTargets.leftHandPinkyIKTransform = leftPinky.transform;
                weaponBaseClass.handsIKTargets.leftHandRingIKTransform = leftRing.transform;
                weaponBaseClass.handsIKTargets.leftHandThumbIKTransform = leftThumb.transform;

                // Right Hand
                weaponBaseClass.handsIKTargets.rightHandIKTransform = rightHandIKTarget.transform;
                weaponBaseClass.handsIKTargets.rightHandIndexIKTransform = rightIndex.transform;
                weaponBaseClass.handsIKTargets.rightHandMiddleIKTransform = rightMiddle.transform;
                weaponBaseClass.handsIKTargets.rightHandPinkyIKTransform = rightPinky.transform;
                weaponBaseClass.handsIKTargets.rightHandRingIKTransform = rightRing.transform;
                weaponBaseClass.handsIKTargets.rightHandThumbIKTransform = rightThumb.transform;
            }
            else
            {
                // Create children for LeftHandIKTarget
                GameObject leftIndex1 = new GameObject(leftHandIndex1ConstraintTargetName);
                GameObject leftIndex2 = new GameObject(leftHandIndex2ConstraintTargetName);
                GameObject leftIndex3 = new GameObject(leftHandIndex3ConstraintTargetName);

                GameObject leftMiddle1 = new GameObject(leftHandMiddle1ConstraintTargetName);
                GameObject leftMiddle2 = new GameObject(leftHandMiddle2ConstraintTargetName);
                GameObject leftMiddle3 = new GameObject(leftHandMiddle3ConstraintTargetName);

                GameObject leftPinky1 = new GameObject(leftHandPinky1ConstraintTargetName);
                GameObject leftPinky2 = new GameObject(leftHandPinky2ConstraintTargetName);
                GameObject leftPinky3 = new GameObject(leftHandPinky3ConstraintTargetName);

                GameObject leftRing1 = new GameObject(leftHandRing1ConstraintTargetName);
                GameObject leftRing2 = new GameObject(leftHandRing2ConstraintTargetName);
                GameObject leftRing3 = new GameObject(leftHandRing3ConstraintTargetName);

                GameObject leftThumb1 = new GameObject(leftHandThumb1ConstraintTargetName);
                GameObject leftThumb2 = new GameObject(leftHandThumb2ConstraintTargetName);
                GameObject leftThumb3 = new GameObject(leftHandThumb3ConstraintTargetName);

                // Create children for RightHandIKTarget
                GameObject rightIndex1 = new GameObject(rightHandIndex1ConstraintTargetName);
                GameObject rightIndex2 = new GameObject(rightHandIndex2ConstraintTargetName);
                GameObject rightIndex3 = new GameObject(rightHandIndex3ConstraintTargetName);

                GameObject rightMiddle1 = new GameObject(rightHandMiddle1ConstraintTargetName);
                GameObject rightMiddle2 = new GameObject(rightHandMiddle2ConstraintTargetName);
                GameObject rightMiddle3 = new GameObject(rightHandMiddle3ConstraintTargetName);

                GameObject rightPinky1 = new GameObject(rightHandPinky1ConstraintTargetName);
                GameObject rightPinky2 = new GameObject(rightHandPinky2ConstraintTargetName);
                GameObject rightPinky3 = new GameObject(rightHandPinky3ConstraintTargetName);

                GameObject rightRing1 = new GameObject(rightHandRing1ConstraintTargetName);
                GameObject rightRing2 = new GameObject(rightHandRing2ConstraintTargetName);
                GameObject rightRing3 = new GameObject(rightHandRing3ConstraintTargetName);

                GameObject rightThumb1 = new GameObject(rightHandThumb1ConstraintTargetName);
                GameObject rightThumb2 = new GameObject(rightHandThumb2ConstraintTargetName);
                GameObject rightThumb3 = new GameObject(rightHandThumb3ConstraintTargetName);

                // Set parents for children
                leftIndex1.transform.parent = leftHandIKTarget.transform;
                leftIndex2.transform.parent = leftIndex1.transform;
                leftIndex3.transform.parent = leftIndex2.transform;

                leftMiddle1.transform.parent = leftHandIKTarget.transform;
                leftMiddle2.transform.parent = leftMiddle1.transform;
                leftMiddle3.transform.parent = leftMiddle2.transform;

                leftPinky1.transform.parent = leftHandIKTarget.transform;
                leftPinky2.transform.parent = leftPinky1.transform;
                leftPinky3.transform.parent = leftPinky2.transform;

                leftRing1.transform.parent = leftHandIKTarget.transform;
                leftRing2.transform.parent = leftRing1.transform;
                leftRing3.transform.parent = leftRing2.transform;

                leftThumb1.transform.parent = leftHandIKTarget.transform;
                leftThumb2.transform.parent = leftThumb1.transform;
                leftThumb3.transform.parent = leftThumb2.transform;


                rightIndex1.transform.parent = rightHandIKTarget.transform;
                rightIndex2.transform.parent = rightIndex1.transform;
                rightIndex3.transform.parent = rightIndex2.transform;

                rightMiddle1.transform.parent = rightHandIKTarget.transform;
                rightMiddle2.transform.parent = rightMiddle1.transform;
                rightMiddle3.transform.parent = rightMiddle2.transform;

                rightPinky1.transform.parent = rightHandIKTarget.transform;
                rightPinky2.transform.parent = rightPinky1.transform;
                rightPinky3.transform.parent = rightPinky2.transform;

                rightRing1.transform.parent = rightHandIKTarget.transform;
                rightRing2.transform.parent = rightRing1.transform;
                rightRing3.transform.parent = rightRing2.transform;

                rightThumb1.transform.parent = rightHandIKTarget.transform;
                rightThumb2.transform.parent = rightThumb1.transform;
                rightThumb3.transform.parent = rightThumb2.transform;

                // Set Created Hands IK Target To Hands IK Target Reference.
                // Left Hand
                weaponBaseClass.handsRotationConstraintTransforms.leftHandIKTransform = leftHandIKTarget.transform;

                weaponBaseClass.handsRotationConstraintTransforms.leftHandIndex1ConstraintTransform = leftIndex1.transform;
                weaponBaseClass.handsRotationConstraintTransforms.leftHandIndex2ConstraintTransform = leftIndex2.transform;
                weaponBaseClass.handsRotationConstraintTransforms.leftHandIndex3ConstraintTransform = leftIndex3.transform;

                weaponBaseClass.handsRotationConstraintTransforms.leftHandMiddle1ConstraintTransform = leftMiddle1.transform;
                weaponBaseClass.handsRotationConstraintTransforms.leftHandMiddle2ConstraintTransform = leftMiddle2.transform;
                weaponBaseClass.handsRotationConstraintTransforms.leftHandMiddle3ConstraintTransform = leftMiddle3.transform;

                weaponBaseClass.handsRotationConstraintTransforms.leftHandPinky1ConstraintTransform = leftPinky1.transform;
                weaponBaseClass.handsRotationConstraintTransforms.leftHandPinky2ConstraintTransform = leftPinky2.transform;
                weaponBaseClass.handsRotationConstraintTransforms.leftHandPinky3ConstraintTransform = leftPinky3.transform;

                weaponBaseClass.handsRotationConstraintTransforms.leftHandRing1ConstraintTransform = leftRing1.transform;
                weaponBaseClass.handsRotationConstraintTransforms.leftHandRing2ConstraintTransform = leftRing2.transform;
                weaponBaseClass.handsRotationConstraintTransforms.leftHandRing3ConstraintTransform = leftRing3.transform;

                weaponBaseClass.handsRotationConstraintTransforms.leftHandThumb1ConstraintTransform = leftThumb1.transform;
                weaponBaseClass.handsRotationConstraintTransforms.leftHandThumb2ConstraintTransform = leftThumb2.transform;
                weaponBaseClass.handsRotationConstraintTransforms.leftHandThumb3ConstraintTransform = leftThumb3.transform;

                // Right Hand
                weaponBaseClass.handsRotationConstraintTransforms.rightHandIKTransform = rightHandIKTarget.transform;

                weaponBaseClass.handsRotationConstraintTransforms.rightHandIndex1ConstraintTransform = rightIndex1.transform;
                weaponBaseClass.handsRotationConstraintTransforms.rightHandIndex2ConstraintTransform = rightIndex2.transform;
                weaponBaseClass.handsRotationConstraintTransforms.rightHandIndex3ConstraintTransform = rightIndex3.transform;

                weaponBaseClass.handsRotationConstraintTransforms.rightHandMiddle1ConstraintTransform = rightMiddle1.transform;
                weaponBaseClass.handsRotationConstraintTransforms.rightHandMiddle2ConstraintTransform = rightMiddle2.transform;
                weaponBaseClass.handsRotationConstraintTransforms.rightHandMiddle3ConstraintTransform = rightMiddle3.transform;

                weaponBaseClass.handsRotationConstraintTransforms.rightHandPinky1ConstraintTransform = rightPinky1.transform;
                weaponBaseClass.handsRotationConstraintTransforms.rightHandPinky2ConstraintTransform = rightPinky2.transform;
                weaponBaseClass.handsRotationConstraintTransforms.rightHandPinky3ConstraintTransform = rightPinky3.transform;

                weaponBaseClass.handsRotationConstraintTransforms.rightHandRing1ConstraintTransform = rightRing1.transform;
                weaponBaseClass.handsRotationConstraintTransforms.rightHandRing2ConstraintTransform = rightRing2.transform;
                weaponBaseClass.handsRotationConstraintTransforms.rightHandRing3ConstraintTransform = rightRing3.transform;

                weaponBaseClass.handsRotationConstraintTransforms.rightHandThumb1ConstraintTransform = rightThumb1.transform;
                weaponBaseClass.handsRotationConstraintTransforms.rightHandThumb2ConstraintTransform = rightThumb2.transform;
                weaponBaseClass.handsRotationConstraintTransforms.rightHandThumb3ConstraintTransform = rightThumb3.transform;
            }

            // Set the parent for both LeftHandIKTarget and RightHandIKTarget
            leftHandIKTarget.transform.parent = handsIKTargets.transform;
            rightHandIKTarget.transform.parent = handsIKTargets.transform;

            // Set Parent, Position & Rotation to parent transform.
            handsIKTargets.transform.parent = parentTransform;

            // Only Set the position and rotation if parent transform is not null
            if (parentTransform != null)
            {
                handsIKTargets.transform.position = parentTransform.position;
                handsIKTargets.transform.rotation = parentTransform.rotation;
            }

            Selection.activeGameObject = handsIKTargets;
        }

        /// <summary>
        /// Handles the creation of a Weapon SO asset & returns true if created and false if it was cancelled.
        /// </summary>
        /// <param name="folderPanelPrompt"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        private bool CreateWeaponSOAsset(string folderPanelPrompt, string assetName)
        {
            // Get the path to Create the Scriptable Object.
            string selectedPath = EditorUtility.OpenFolderPanel(folderPanelPrompt, "Assets", "");

            if (string.IsNullOrEmpty(selectedPath))
            {
                // User Canceled The Folder Selection For Creation.
                Debug.LogWarning("Folder Selection Canceled For creating a Weapon Scriptable Object.");

                // Return false because user has Canceled it.
                return false;
            }

            string relativePath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
            string baseAssetPath = relativePath + "/" + assetName;
            string assetPath = AssetDatabase.GenerateUniqueAssetPath(baseAssetPath + ".asset");

            // Make an Instance for the Weapon Scriptable Object (SO) Settings.
            WeaponSO weaponSO = CreateInstance<WeaponSO>();

            // Create The Asset.
            AssetDatabase.CreateAsset(weaponSO, assetPath);

            // Save Assets.
            AssetDatabase.SaveAssets();

            // Refresh the Asset Database.
            AssetDatabase.Refresh();

            // Set the Selection to the Weapon SO.
            Selection.activeObject = weaponSO;

            // Focus Project Window.
            EditorUtility.FocusProjectWindow();

            // Ping Object Effect.
            EditorGUIUtility.PingObject(weaponSO);

            // Set the reference to the newly created SO.
            weaponBaseClass.weaponData = weaponSO;

            // Return true because we created a New Scriptable Object.
            return true;
        }

        #endregion
    }

    #endregion

    #region Weapon Creator Window

    // Weapon Creator Window.
    public class WeaponCreator : EditorWindow
    {

        #region Variables

        // The Main Mesh of the Weapon.
        private GameObject mainMeshParent = null;

        // Name of the Weapon
        private string weaponName = "";

        #endregion

        // Show and open the window from the Menu, (Make a menu item).
        [MenuItem("HK Fps/Weapon Creator")]
        public static void ShowWindow()
        {
            GetWindow<WeaponCreator>("WeaponCreator");
        }

        // OnGUI.
        private void OnGUI()
        {
            // Title
            GUILayout.Label("Welcome to HK Fps Weapon Creator", EditorStyles.boldLabel);

            // 10 pixels space.
            GUILayout.Space(10);

            // Label, tell the user to put a reference of there main model mesh.
            GUILayout.Label("Drag your weapon's main mesh parent into the field");

            // Main Mesh GameObject Field.
            mainMeshParent = EditorGUILayout.ObjectField(mainMeshParent, typeof(GameObject), true) as GameObject;

            // 10 pixels space
            GUILayout.Space(10);

            // Label, The Name For your weapon.
            GUILayout.Label("The Name You Want For Your Weapon");

            // Weapon name
            weaponName = EditorGUILayout.TextField(weaponName);

            // 10 pixels space
            GUILayout.Space(10);

            // Draw a Button for Creating the Weapon.
            if (GUILayout.Button("Create Weapon", GUILayout.Height(25)))
            {
                // Check if the Main Mesh Parent isn't.
                if (mainMeshParent == null)
                {
                    Debug.LogWarning("Main Mesh Parent wasn't assigned, Please assign your weapons main mesh parent to this field");
                    return;
                }

                // Check if the weapon has something.
                if (weaponName != "")
                {
                    // Create Weapon.
                    // Create empty gameobject for the weapon.
                    GameObject weapon = new GameObject(weaponName);

                    // Set the Main Mesh Parent's Name
                    mainMeshParent.name = "Mesh";

                    // Set the Position & Rotation.
                    weapon.transform.position = mainMeshParent.transform.position;
                    weapon.transform.rotation = mainMeshParent.transform.rotation;

                    // Parent Handling.
                    weapon.transform.parent = mainMeshParent.transform.parent;
                    mainMeshParent.transform.parent = weapon.transform;

                    // Add WeaponBase script.
                    WeaponBase weaponBaseClass = weapon.AddComponent<WeaponBase>();

                    // Open Setup helper window.
                    SetupHelperEditorWindow.OpenWindow(weapon.transform, weaponBaseClass);

                    // Select the weapon
                    Selection.activeGameObject = weapon;

                    // Close this window as it is no longer needed.
                    Close();
                }
                else
                {
                    // Tell the user to make sure to set the weapon's name before creating.
                    Debug.LogWarning("Please Set the Weapon's Name Before Creating");
                    return;
                }

            }
        }
    }

    #endregion

    #region Custom Inspector Drawing


    public override void OnInspectorGUI()
    {
        // base Inspector
        base.OnInspectorGUI();

        // Space
        EditorGUILayout.Space();

        // Cast.
        WeaponBase weaponBaseReference = (WeaponBase)target;

        EditorGUILayout.LabelField("Weapon Setup Window", EditorStyles.boldLabel);

        // Draw Weapon Setup Window Button.
        if (GUILayout.Button("Open Weapon Setup Window"))
        {
            // Open Creation Window.
            SetupHelperEditorWindow.OpenWindow(weaponBaseReference.transform, weaponBaseReference);
        }
    }

    #endregion
}
#endif
#endregion