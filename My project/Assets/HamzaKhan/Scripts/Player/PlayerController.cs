using UnityEngine.Animations.Rigging;
using System.Collections;
using UnityEngine;
using Cinemachine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    // input
    #region
    [Space]
    [Header("Input")]
    [SerializeField] private float smoothInputSpeed = 0.1f;

    // private var
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    [HideInInspector] public PlayerInputActions Input;
    #endregion

    // movement
    #region
    [Space]
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3;
    [SerializeField] private float walkBackSpeed = 2;
    [SerializeField] private float runSpeed = 7, runBackSpeed = 5;
    [SerializeField] private float crouchSpeed = 2, crouchBackSpeed = 1;
    [SerializeField] private float crouchHeight = 1.2f;
    [SerializeField] private float crouchCenterY = 0.6f;
    [SerializeField] private float crouchToStandCheckDistance = 2f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float standingCenterY = 1f;

    [SerializeField] private float controllerRadius = 0.5f;

    [Header("Gravity Push of ledge Settings")]
    [SerializeField] private float radiusChangeTime = 0.4f;
    [SerializeField] private float gSphereCheckRadius = 0.2f;
    [SerializeField] private float gSphereCheckYOffset = 0f;

    // private var
    // Movement
    private CharacterController characterController;
    private Vector3 moveDirection;
    private float currentSpeed = 3;
    private bool isWalking;
    private bool isSprinting;
    private bool isCrouching;

    private bool hasFinishedChangingRadius = true;
    #endregion

    // animator
    #region
    [Space]
    [Header("Animator")]
    [SerializeField] private Animator playerAnimator;
    #endregion

    // gravity
    #region
    [Space]
    [Header("Gravity")]
    [SerializeField] private float gravityMultiplyer = 3f;

    // private var
    private float gravity = -9.81f;
    private float velocityY;
    #endregion

    // jump
    #region
    [Space]
    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;

    [SerializeField] private float groundDistance = 0.1f;
    [SerializeField] private float groundCheckRadius = 0.5f;
    [SerializeField] private LayerMask groundLayerMask; 

    [SerializeField] private float jumpBumpAmount;
    [SerializeField] private float jumpBumpAmountDuration;

    [SerializeField] private float HitTheGroundbumpAmount;
    [SerializeField] private float HitTheGroundbumpAmountDuration;

    [SerializeField] private CinemachineImpulseSource bumpImpulseSource;

    private bool inAir;

    // private var
    private bool nearGround;
    #endregion

    // look
    #region
    [Space]
    [Header("Look")]
    [SerializeField] private Transform centerSpinePos;

    [SerializeField] private float sensY;
    [SerializeField] private float sensX;
    [SerializeField] private float maxLookUpDownOnFeet = 90f;
    [SerializeField] private float maxLookUpDownMovingOrInAir = 40f;
    [SerializeField] private float lookMaxChangeSpeed = 40f;
    [SerializeField] private float rotatingSlerpSpeed = 20f;

    private float maxLookUpDown;

    // private var
    private float xRotation;
    private float yRotation;
    #endregion

    // camera
    #region
    [Space]
    [Header("Camera")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Transform cameraHolderNotAiming;
    [SerializeField] private Transform cameraHolderAiming;
    [SerializeField] private Transform cameraHolderClipped;

    // private var
    #endregion

    // Weapon
    #region
    [Space]
    [Header("Weapon")]
    [SerializeField] private Transform rifleHolder;
    [SerializeField] private Transform pistolHolder;
    [SerializeField] private GameObject crossHairUIGameObject;
    [SerializeField] private float leftHandIKSmoothWeightSpeed = 20f;

    [Header("Hands Transform Constraints")]
    [SerializeField] private ConstraintsWeightModifier iKBasedFingersWeightModifier;
    [SerializeField] private ConstraintsWeightModifier rotationConstraitBasedFingersWeightModifier;

    [SerializeField] private HandsIKTransform handsIKFollowers;
    [SerializeField] private HandsRotationConstraintTransforms handsConstraintsFollowers;

    // aiming
    private bool isAiming;

    // shooting.
    private float gunShotTimer;
    private bool canShoot;

    // reloading
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    private bool isReloading;

    // weapon
    private WeaponBase currentWeapon;

    // Clip prevention
    [Space]
    [Header("Clip Prevention Settings")]
    [SerializeField] private Transform gunHolder;
    [SerializeField] private Transform clipProjectorPos;
    [SerializeField] private Transform clipProjector;
    [SerializeField] private Transform clipVisual;

    [SerializeField] private float spineOffsetChangeSpeed = 5f;
    [SerializeField] private float lerpPosChangeSpeed = 5f;

    [SerializeField] private LayerMask clipCheckMask;

    [SerializeField] private MultiRotationConstraint spineOneConstraint;
    [SerializeField] private MultiRotationConstraint spineTwoConstraint;

    // TODO, REMOVE IF NOT USED>
    private Vector3 spineOneConstraintOffset;
    private Vector3 spineTwoConstraintOffset;

    private Vector3 initialPosition;

    private bool isClipped = false;

    private float lerpPos;

    #endregion

    // interactions
    #region
    [Space]
    [Header("Interactions")]
    [SerializeField] private float interactMaxDistance = 1;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private GameObject interactUI;
    #endregion

    #region General

    // initialization...
    void Awake()
    {
        Input = new PlayerInputActions();
    }

    void Start()
    {
        // Get the controller.
        characterController = GetComponent<CharacterController>();

        // CurrentWeaponRef
        currentWeapon = GetComponentInChildren<WeaponBase>();

        // Set it to default;
        inAir = false;

        // Get the initial position.
        initialPosition = gunHolder.localPosition;

        // Set the isCurrentWeapon bool.
        currentWeapon.SetCurrentWeapon(true);
        currentWeapon.SetControllerReference(this);
        
        // Set the current weapons Hand IK Targets, So we don't have to set them even manually...
        SetAllIKFollowersTargetToTargets(currentWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        // Call all methods
        Gravity();
        Jump();
        HandleSpeedsAndPlayerHeight();
        HandleInputAndMove();
        HandleAnim();
        Look();
        HandleShooting();
        HandleAiming();
        HandleInteractions();
        HandleReloadingAndLeftHandIK();
        HandleWeaponClipping();
    }

    #endregion

    #region player Camera

    private void LateUpdate()
    {
        SetCameraPositionAndRotation();
    }

    private void SetCameraPositionAndRotation()
    {
        if (isClipped)
        {
            cameraHolder.position = Vector3.Lerp(cameraHolder.position, cameraHolderClipped.position, 10f * Time.deltaTime);
            cameraHolder.rotation = Quaternion.Lerp(cameraHolder.rotation, cameraHolderClipped.rotation, 10f * Time.deltaTime);
        }
        else if (isAiming)
        {
            if (cameraHolder.position != cameraHolderAiming.position)
            {
                cameraHolder.position = Vector3.Lerp(cameraHolder.position, cameraHolderAiming.position, 10f * Time.deltaTime);
            }
            else
            {
                cameraHolder.position = cameraHolderAiming.position;
            }

            cameraHolder.rotation = Quaternion.Lerp(cameraHolder.rotation, cameraHolderAiming.rotation, 10f * Time.deltaTime);
        }
        else
        {
            if (cameraHolder.position != cameraHolderNotAiming.position)
            {
                cameraHolder.position = Vector3.Lerp(cameraHolder.position, cameraHolderNotAiming.position, 10f * Time.deltaTime);
            }
            else
            {
                cameraHolder.position = cameraHolderNotAiming.position;
            }

            cameraHolder.rotation = Quaternion.Lerp(cameraHolder.rotation, cameraHolderNotAiming.rotation, 10f * Time.deltaTime);
        }

        // Position the camera POV'S.
        cameraHolderNotAiming.localPosition = currentWeapon.weaponData.cameraNormalPosition;
        cameraHolderAiming.localPosition = currentWeapon.weaponData.cameraAimingPosition;
        cameraHolderClipped.localPosition = currentWeapon.weaponData.cameraClippedPosition;
    }

    #endregion

    #region player Movement

    private void Gravity()
    {
        if (characterController.isGrounded && velocityY < 0.0f)
        {
            velocityY = -1f;
        }
        else
        {
            velocityY += gravity * Time.deltaTime * gravityMultiplyer;
        }
    }

    private void Jump()
    {
        // CheckSphere for ground check.
        Vector3 groundCheckPosition = transform.position + new Vector3(0f, groundDistance, 0f);
        nearGround = Physics.CheckSphere(groundCheckPosition, groundCheckRadius, groundLayerMask);

        if (Input.Player.Jump.IsPressed() && characterController.isGrounded && !Physics.Raycast(transform.position, Vector3.up, crouchToStandCheckDistance, groundLayerMask))
        {
            playerAnimator.SetTrigger("Jump");
            velocityY = jumpForce;

            bumpImpulseSource.m_ImpulseDefinition.m_ImpulseDuration = jumpBumpAmountDuration;
            bumpImpulseSource.GenerateImpulse(new Vector3(0f, jumpBumpAmount, 0f));
        }


        if (inAir == nearGround)
        {
            inAir = !nearGround;

            if (nearGround == true)
            {
                bumpImpulseSource.m_ImpulseDefinition.m_ImpulseDuration = HitTheGroundbumpAmountDuration;
                bumpImpulseSource.GenerateImpulse(new Vector3(0f, HitTheGroundbumpAmount * -velocityY, 0f));
            }
        }
    }

    private void HandleSpeedsAndPlayerHeight()
    {
        // get the inputVector
        Vector2 inputVector = Input.Player.Move.ReadValue<Vector2>();

        // set the bools
        if (Input.Player.Sprint.triggered && !Physics.Raycast(transform.position, Vector3.up, crouchToStandCheckDistance, groundLayerMask))
        {
            isSprinting = !isSprinting;
            isCrouching = false;
        }
        if (Input.Player.Crouch.triggered && !Physics.Raycast(transform.position, Vector3.up, crouchToStandCheckDistance, groundLayerMask))
        {
            isCrouching = !isCrouching;
            isSprinting = false;
        }
        if (!isSprinting && !isCrouching) isWalking = true;
        else isWalking = false;

        if (inputVector == Vector2.zero) isSprinting = false;

        // Handle the speeds
        if (isWalking) currentSpeed = walkSpeed;
        if (isSprinting) currentSpeed = runSpeed;
        if (isCrouching) currentSpeed = crouchSpeed;
        if (isSprinting && inputVector.y > 0.0f) currentSpeed = runSpeed;
        else if (isSprinting && inputVector.y < 0.0f) currentSpeed = runBackSpeed;
        if (isWalking && inputVector.y > 0.0f) currentSpeed = walkSpeed;
        else if (isWalking && inputVector.y < 0.0f) currentSpeed = walkBackSpeed;
        if (isCrouching && inputVector.y > 0.0f) currentSpeed = crouchSpeed;
        else if (isCrouching && inputVector.y < 0.0f) currentSpeed = crouchBackSpeed;

        // Handle the player's height
        if (isCrouching)
        {
            characterController.height = crouchHeight;
            characterController.center = new Vector3(0f, crouchCenterY, 0f);
        }
        else
        {
            characterController.height = standingHeight;
            characterController.center = new Vector3(0f, standingCenterY, 0f);
        }
    }

    private void HandleInputAndMove()
    {
        // Input Vector
        Vector2 inputVector = Input.Player.Move.ReadValue<Vector2>();

        // Check if we should change the radius.
        if (characterController.isGrounded == true && inputVector == Vector2.zero && Physics.CheckSphere(transform.position + new Vector3(0, gSphereCheckYOffset, 0), gSphereCheckRadius, groundLayerMask) == false)
        {
            // Check if we aren't already changing the radius.
            if (hasFinishedChangingRadius == true)
            {
                // Start the radius change coroutine.
                StartCoroutine(ChangeRadiusCoroutine(gSphereCheckRadius - 0.05f, radiusChangeTime));
            }
        }

        // input
        currentInputVector = Vector2.SmoothDamp(currentInputVector, inputVector, ref smoothInputVelocity, smoothInputSpeed);

        // Movement
        moveDirection = (currentInputVector.y * transform.forward) + (currentInputVector.x * transform.right);
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
        characterController.Move(transform.up * velocityY * Time.deltaTime);
    }

    private IEnumerator ChangeRadiusCoroutine(float targetRadius, float time)
    {
        hasFinishedChangingRadius = false;

        float elapsedTime = 0f;
        float initialRadius = characterController.radius;

        // Smoothly set the radius to the target radius.
        while (elapsedTime < time)
        {
            characterController.radius = Mathf.Lerp(initialRadius, targetRadius, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        characterController.radius = targetRadius;

        // Smoothly revert the radius back to the original value
        elapsedTime = 0f;
        while (elapsedTime < time)
        {
            characterController.radius = Mathf.Lerp(targetRadius, controllerRadius, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        characterController.radius = controllerRadius;

        hasFinishedChangingRadius = true;
    }

    private void HandleAnim()
    {
        // Movement bools
        playerAnimator.SetBool("IsSprinting", isSprinting);
        playerAnimator.SetBool("IsWalking", isWalking);
        playerAnimator.SetBool("IsCrouching", isCrouching);

        // Jumping bool
        playerAnimator.SetBool("NearGround", nearGround);

        // Aiming bool
        playerAnimator.SetBool("IsAiming", isAiming);

        // Movement floats
        playerAnimator.SetFloat("VelocityZ", currentInputVector.x);
        playerAnimator.SetFloat("VelocityY", currentInputVector.y);

        // Weapon type
        playerAnimator.SetFloat("WeaponType", currentWeapon.weaponData.weaponType == WeaponSO.WeaponType.rifle ? 0 : 1);
    }

    private void Look()
    {
        // get the lookVector and set it up
        Vector2 lookVector = Input.Player.Look.ReadValue<Vector2>();
        float mouseX = lookVector.x * Time.deltaTime * sensX;
        float mouseY = lookVector.y * Time.deltaTime * sensY;
        yRotation += mouseX;
        xRotation -= mouseY;

        // check if we should not allow 90 angle.
        if (Input.Player.Move.ReadValue<Vector2>() != Vector2.zero || nearGround == false)
        {
            maxLookUpDown = Mathf.Lerp(maxLookUpDown, maxLookUpDownMovingOrInAir, lookMaxChangeSpeed * Time.deltaTime);
        }
        else
        {
            maxLookUpDown = Mathf.Lerp(maxLookUpDown, maxLookUpDownOnFeet, lookMaxChangeSpeed * Time.deltaTime);
        }

        // Clamp X Rot.
        xRotation = Mathf.Clamp(xRotation, -maxLookUpDown, maxLookUpDown);

        // setting the values to the objects
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yRotation, 0), rotatingSlerpSpeed * Time.deltaTime);

        //// Check if we are reloading.
        if (isReloading == true)
        {
            // Set Spine Followers Rotation Accordingly.
            SetSpineFollowersRotation(0f, 0f, rotatingSlerpSpeed);
        }
        else if (isClipped == true) // Check if is clipped is true
        {
            // Set Spine Followers Rotation Accordingly.
            SetSpineFollowersRotation(0f, xRotation, rotatingSlerpSpeed);
        }
        else
        {
            // Set Spine Followers Rotation Accordingly.
            SetSpineFollowersRotation(xRotation, xRotation, rotatingSlerpSpeed);
        }
    }

    /// <summary>
    /// Helpful Function for setting spine followers target rotation X.
    /// </summary>
    /// <param name="centerSpineTargetX"></param>
    /// <param name="clipProjTargetX"></param>
    /// <param name="slerpingSpeed"></param>
    private void SetSpineFollowersRotation(float centerSpineTargetX, float clipProjTargetX, float slerpingSpeed)
    {
        clipProjectorPos.localRotation = Quaternion.Slerp(clipProjectorPos.localRotation, Quaternion.Euler(clipProjTargetX, 0f, 0f), slerpingSpeed * Time.deltaTime);
        centerSpinePos.localRotation = Quaternion.Slerp(centerSpinePos.localRotation, Quaternion.Euler(centerSpineTargetX, 0f, 0f), slerpingSpeed * Time.deltaTime);
    }

    #endregion

    #region player Weapon

    // Handle shooting
    private void HandleShooting()
    {
        // check if we dont have bullets
        if (currentWeapon.currentAmmo == 0 || isReloading == true)
        {
            // if we dont then canShoot is false
            canShoot = false;
        }
        else
        {
            // if we have then canShoot is true
            canShoot = true;
        }

        if (Input.Player.Shoot.IsPressed() && gunShotTimer <= 0f && canShoot)
        {
            Shoot(currentWeapon.firePoint.transform.position, currentWeapon.firePoint.transform.forward);

            gunShotTimer = currentWeapon.weaponData.timeBetweenShot;
        }

        if (gunShotTimer >= 0f)
        {
            gunShotTimer -= Time.deltaTime;
        }
    }

    // Shoot!
    private void Shoot(Vector3 position, Vector3 direction)
    {
        // deplete ammo.
        currentWeapon.currentAmmo -= 1;

        // play effect.
        currentWeapon.muzzleFlashEffect.Play();

        // play audio.
        currentWeapon.audioSource.Play();

        // call Shoot.
        currentWeapon.Shoot();

        if (currentWeapon.weaponData.firingType == WeaponSO.FiringType.raycast)
        {
            // check if we hit something.
            if (Physics.Raycast(position, direction, out RaycastHit hit, currentWeapon.weaponData.maxShootRange))
            {
                // get the IHitable interface reference.
                IHitable iHitable = hit.transform.GetComponent<IHitable>();

                // check if hitable is not null.
                if (iHitable != null)
                {
                    // Hit.
                    iHitable.Hit(hit.transform.gameObject, hit.point, hit.normal);
                }
            }
        }
        else if (currentWeapon.weaponData.firingType == WeaponSO.FiringType.projectile)
        {
            GameObject projectile = Instantiate(currentWeapon.weaponData.projectilePrefab, currentWeapon.firePoint.position, currentWeapon.firePoint.rotation);
            projectile.GetComponent<Projectile>().Fire(currentWeapon.weaponData.projectileForce);
        }
    }

    // aiming
    private void HandleAiming()
    {
        // check for input aim.
        if (Input.Player.Aim.triggered)
        {
            // toggle is aiming.
            isAiming = !isAiming;

            if (isAiming == true)
            {
                currentWeapon.OnAimEnter();
            }
            else
            {
                currentWeapon.OnAimExit();
            }
        }

        // turn on, off the cross hair...
        crossHairUIGameObject.SetActive(!isAiming);
    }

    // HandleReloading
    private void HandleReloadingAndLeftHandIK()
    {
        // condition check if we need to reload
        if (Input.Player.Reload.triggered && currentWeapon.currentAmmo < currentWeapon.weaponData.magazineSize &&
            isReloading == false && isClipped == false && gunShotTimer <= 0f
            && !Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, interactMaxDistance, interactableLayerMask))
        {
            if (currentWeapon.totalAmmo != 0)
            {
                // Weapon Reload
                currentWeapon.StartReloading();
                StartCoroutine(ReloadingTime());
            }
        }

        // Left Hand IK
        leftHandIK.weight = Mathf.Lerp(leftHandIK.weight, 1f, Time.deltaTime * leftHandIKSmoothWeightSpeed);
    }

    private IEnumerator ReloadingTime()
    {
        isReloading = true;
        yield return new WaitForSeconds(currentWeapon.weaponData.reloadingAnimationTime);
        isReloading = false;
    }

    // prevent the weapon from clipping;
    private void HandleWeaponClipping()
    {
        // Clip projector position.
        clipProjector.localPosition = currentWeapon.weaponData.clipProjectorPosition;

        // for better visualizing.
        if (isClipped == true)
        {
            clipVisual.transform.localScale = currentWeapon.weaponData.boxCastClippedSize;
        }
        else
        {
            clipVisual.transform.localScale = currentWeapon.weaponData.boxCastSize;
        }

        // check box for checking whether our weapon is clipping.
        if (Physics.CheckBox(clipProjector.transform.position, currentWeapon.weaponData.boxCastSize / 2f, clipProjector.transform.rotation, clipCheckMask))
        {
            SetSpineConstraintsOffset(true);
        }
        else if (isClipped == true)
        {
            if (Physics.CheckBox(clipProjector.transform.position, currentWeapon.weaponData.boxCastClippedSize / 2f, clipProjector.transform.rotation, clipCheckMask))
            {
                SetSpineConstraintsOffset(true);
            }
            else
            {
                SetSpineConstraintsOffset(false);
            }
        }
        else
        {
            SetSpineConstraintsOffset(false);
        }

        // Set the constraint's rotation offset values
        spineOneConstraint.data.offset = spineOneConstraintOffset;
        spineTwoConstraint.data.offset = spineTwoConstraintOffset;

        // Set constraints Weights
        if (currentWeapon.handsConstraintType == HandsConstraintType.IKBasedFingers)
        {
            iKBasedFingersWeightModifier.SetWeight(Mathf.Lerp(iKBasedFingersWeightModifier.GetWeight(), 1f, 10 * Time.deltaTime));
            rotationConstraitBasedFingersWeightModifier.SetWeight(Mathf.Lerp(rotationConstraitBasedFingersWeightModifier.GetWeight(), 0f, 10 * Time.deltaTime));
        }
        else
        {
            iKBasedFingersWeightModifier.SetWeight(Mathf.Lerp(iKBasedFingersWeightModifier.GetWeight(), 0f, 10 * Time.deltaTime));
            rotationConstraitBasedFingersWeightModifier.SetWeight(Mathf.Lerp(rotationConstraitBasedFingersWeightModifier.GetWeight(), 1f, 10 * Time.deltaTime));
        }

        // Set the Local Position Offset for the weapon.
        currentWeapon.transform.localPosition = currentWeapon.weaponData.weaponPositionOffset;

        // Clamp LerpPos.
        Mathf.Clamp01(lerpPos);

        // Set the local rotation
        gunHolder.localRotation =
            Quaternion.Lerp(
                Quaternion.Euler(0f, -currentWeapon.weaponData.spineConstraintOffsetY, 0f),
                Quaternion.Euler(currentWeapon.weaponData.newRotation),
                lerpPos
                );

        // Set the local position
        gunHolder.localPosition =
            Vector3.Lerp(
                initialPosition,
                currentWeapon.weaponData.newPosition,
                lerpPos
                );
    }

    private void SetSpineConstraintsOffset(bool clipped)
    {
        if (clipped == false)
        {
            // Set is clipped to false.
            isClipped = false;

            // Set lerpPos.
            lerpPos = Mathf.Lerp(lerpPos, 0f, lerpPosChangeSpeed * Time.deltaTime);

            // Set the Two Constraints offsets.

            // Constraint One
            spineOneConstraintOffset = new Vector3(
            Mathf.Lerp(spineOneConstraint.data.offset.x, 0f, spineOffsetChangeSpeed * Time.deltaTime),
            Mathf.Lerp(spineOneConstraint.data.offset.y, currentWeapon.weaponData.spineConstraintOffsetY, spineOffsetChangeSpeed * Time.deltaTime), 0f);

            // Constraint Two
            spineTwoConstraintOffset = new Vector3(
            Mathf.Lerp(spineTwoConstraint.data.offset.x, 0f, spineOffsetChangeSpeed * Time.deltaTime),
            Mathf.Lerp(spineTwoConstraint.data.offset.y, currentWeapon.weaponData.spineConstraintOffsetY, spineOffsetChangeSpeed * Time.deltaTime), 0f);
        }
        else
        {
            // Set is clipped to true.
            isClipped = true;

            // Set lerpPos.
            lerpPos = Mathf.Lerp(lerpPos, 1f, lerpPosChangeSpeed * Time.deltaTime);

            // Set the Two Constraints offsets.

            // Constraint One
            spineOneConstraintOffset = new Vector3(
            Mathf.Lerp(spineOneConstraint.data.offset.x, 0f, spineOffsetChangeSpeed * Time.deltaTime),
            Mathf.Lerp(spineOneConstraint.data.offset.y, 0f, spineOffsetChangeSpeed * Time.deltaTime), 0f);

            // Constraint Two
            spineTwoConstraintOffset = new Vector3(
            Mathf.Lerp(spineTwoConstraint.data.offset.x, 0f, spineOffsetChangeSpeed * Time.deltaTime),
            Mathf.Lerp(spineTwoConstraint.data.offset.y, 0f, spineOffsetChangeSpeed * Time.deltaTime), 0f);
        }
    }

    #endregion

    #region player Interactions
    private void HandleInteractions()
    {
        // check if we are looking at some thing in the interactMaxDistance range
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, interactMaxDistance, interactableLayerMask)
            && isReloading == false)
        {
            // To prevent our weapon from being swapped with our weapon(it would be silly, right.)
            if (hit.transform.TryGetComponent(out WeaponBase interactableWeapon) && interactableWeapon == currentWeapon)
            {
                // return , do not show any interact bar either.
                interactUI.SetActive(false);
                return;
            }
            else if (hit.transform.TryGetComponent(out interactableWeapon)
                && interactableWeapon.weaponData.weaponType != currentWeapon.weaponData.weaponType)
            {
                // return , do not show any interact bar either.
                interactUI.SetActive(false);
                return;
            }

            // check if there is an interactable object.
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                // Enable the interact UI and set the text.
                interactUI.SetActive(true);
                interactUI.GetComponentInChildren<TextMeshProUGUI>().text = interactable.Message();

                // check for input.
                if (Input.Player.Interact.triggered)
                {
                    // interact.
                    interactable.Interact(this);
                }
            }
            else
            {
                // disable interact UI.
                interactUI.SetActive(false);
            }
        }
        else
        {
            // disable interact UI.
            interactUI.SetActive(false);
        }
    }
    #endregion

    #region Exposed Methods

    // Functions
    public void SwapWeapon(WeaponBase newWeapon)
    {
        Vector3 newWeaponPickupPosition = newWeapon.transform.position;
        Quaternion newWeaponPickupRotation = newWeapon.transform.rotation;

        currentWeapon.transform.SetParent(newWeapon.transform.parent);
        currentWeapon.transform.position = newWeaponPickupPosition;
        currentWeapon.transform.rotation = newWeaponPickupRotation;
        currentWeapon.SetCurrentWeapon(false);

        if (newWeapon.weaponData.weaponType == WeaponSO.WeaponType.rifle)
        {
            newWeapon.transform.SetParent(rifleHolder);
            newWeapon.transform.position = rifleHolder.position;
            newWeapon.transform.rotation = rifleHolder.rotation;
        }
        else if (newWeapon.weaponData.weaponType == WeaponSO.WeaponType.pistol)
        {
            newWeapon.transform.SetParent(pistolHolder);
            newWeapon.transform.position = pistolHolder.position;
            newWeapon.transform.rotation = pistolHolder.rotation;
        }

        // Local Position Offsetting.
        newWeapon.transform.localPosition = newWeapon.weaponData.weaponPositionOffset;

        // Set Current Weapon Is used everywhere instead of iscurrentweapon as we made that private.
        newWeapon.SetCurrentWeapon(true);
        newWeapon.SetControllerReference(this);

        // Set all ik targets.
        SetAllIKFollowersTargetToTargets(newWeapon);

        // Set the new weapon to the current.
        currentWeapon = newWeapon;
    }

    private void SetAllIKFollowersTargetToTargets(WeaponBase weapon)
    {
        if (weapon.handsConstraintType == HandsConstraintType.IKBasedFingers)
        {
            // Left Hand IK.
            //Hand
            SetIKFollowersTargetToTarget(handsIKFollowers.leftHandIKTransform, weapon.handsIKTargets.leftHandIKTransform);

            //Fingers
            SetIKFollowersTargetToTarget(handsIKFollowers.leftHandIndexIKTransform, weapon.handsIKTargets.leftHandIndexIKTransform);
            SetIKFollowersTargetToTarget(handsIKFollowers.leftHandMiddleIKTransform, weapon.handsIKTargets.leftHandMiddleIKTransform);
            SetIKFollowersTargetToTarget(handsIKFollowers.leftHandPinkyIKTransform, weapon.handsIKTargets.leftHandPinkyIKTransform);
            SetIKFollowersTargetToTarget(handsIKFollowers.leftHandRingIKTransform, weapon.handsIKTargets.leftHandRingIKTransform);
            SetIKFollowersTargetToTarget(handsIKFollowers.leftHandThumbIKTransform, weapon.handsIKTargets.leftHandThumbIKTransform);

            // Right Hand IK.
            //Hand
            SetIKFollowersTargetToTarget(handsIKFollowers.rightHandIKTransform, weapon.handsIKTargets.rightHandIKTransform);

            //Fingers
            SetIKFollowersTargetToTarget(handsIKFollowers.rightHandIndexIKTransform, weapon.handsIKTargets.rightHandIndexIKTransform);
            SetIKFollowersTargetToTarget(handsIKFollowers.rightHandMiddleIKTransform, weapon.handsIKTargets.rightHandMiddleIKTransform);
            SetIKFollowersTargetToTarget(handsIKFollowers.rightHandPinkyIKTransform, weapon.handsIKTargets.rightHandPinkyIKTransform);
            SetIKFollowersTargetToTarget(handsIKFollowers.rightHandRingIKTransform, weapon.handsIKTargets.rightHandRingIKTransform);
            SetIKFollowersTargetToTarget(handsIKFollowers.rightHandThumbIKTransform, weapon.handsIKTargets.rightHandThumbIKTransform);
        }
        else
        {
            // Left Hand IK.
            //Hand
            SetIKFollowersTargetToTarget(handsConstraintsFollowers.leftHandIKTransform, weapon.handsRotationConstraintTransforms.leftHandIKTransform);

            //Fingers
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.leftHandIndex1ConstraintTransform, weapon.handsRotationConstraintTransforms.leftHandIndex1ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.leftHandIndex2ConstraintTransform, weapon.handsRotationConstraintTransforms.leftHandIndex2ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.leftHandIndex3ConstraintTransform, weapon.handsRotationConstraintTransforms.leftHandIndex3ConstraintTransform);

            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.leftHandMiddle1ConstraintTransform, weapon.handsRotationConstraintTransforms.leftHandMiddle1ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.leftHandMiddle2ConstraintTransform, weapon.handsRotationConstraintTransforms.leftHandMiddle2ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.leftHandMiddle3ConstraintTransform, weapon.handsRotationConstraintTransforms.leftHandMiddle3ConstraintTransform);

            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.leftHandPinky1ConstraintTransform, weapon.handsRotationConstraintTransforms.leftHandPinky1ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.leftHandPinky2ConstraintTransform, weapon.handsRotationConstraintTransforms.leftHandPinky2ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.leftHandPinky3ConstraintTransform, weapon.handsRotationConstraintTransforms.leftHandPinky3ConstraintTransform);

            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.leftHandRing1ConstraintTransform, weapon.handsRotationConstraintTransforms.leftHandRing1ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.leftHandRing2ConstraintTransform, weapon.handsRotationConstraintTransforms.leftHandRing2ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.leftHandRing3ConstraintTransform, weapon.handsRotationConstraintTransforms.leftHandRing3ConstraintTransform);

            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.leftHandThumb1ConstraintTransform, weapon.handsRotationConstraintTransforms.leftHandThumb1ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.leftHandThumb2ConstraintTransform, weapon.handsRotationConstraintTransforms.leftHandThumb2ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.leftHandThumb3ConstraintTransform, weapon.handsRotationConstraintTransforms.leftHandThumb3ConstraintTransform);

            // Right Hand IK.
            //Hand
            SetIKFollowersTargetToTarget(handsConstraintsFollowers.rightHandIKTransform, weapon.handsRotationConstraintTransforms.rightHandIKTransform);

            //Fingers
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.rightHandIndex1ConstraintTransform, weapon.handsRotationConstraintTransforms.rightHandIndex1ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.rightHandIndex2ConstraintTransform, weapon.handsRotationConstraintTransforms.rightHandIndex2ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.rightHandIndex3ConstraintTransform, weapon.handsRotationConstraintTransforms.rightHandIndex3ConstraintTransform);

            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.rightHandMiddle1ConstraintTransform, weapon.handsRotationConstraintTransforms.rightHandMiddle1ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.rightHandMiddle2ConstraintTransform, weapon.handsRotationConstraintTransforms.rightHandMiddle2ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.rightHandMiddle3ConstraintTransform, weapon.handsRotationConstraintTransforms.rightHandMiddle3ConstraintTransform);

            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.rightHandPinky1ConstraintTransform, weapon.handsRotationConstraintTransforms.rightHandPinky1ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.rightHandPinky2ConstraintTransform, weapon.handsRotationConstraintTransforms.rightHandPinky2ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.rightHandPinky3ConstraintTransform, weapon.handsRotationConstraintTransforms.rightHandPinky3ConstraintTransform);

            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.rightHandRing1ConstraintTransform, weapon.handsRotationConstraintTransforms.rightHandRing1ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.rightHandRing2ConstraintTransform, weapon.handsRotationConstraintTransforms.rightHandRing2ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.rightHandRing3ConstraintTransform, weapon.handsRotationConstraintTransforms.rightHandRing3ConstraintTransform);

            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.rightHandThumb1ConstraintTransform, weapon.handsRotationConstraintTransforms.rightHandThumb1ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.rightHandThumb2ConstraintTransform, weapon.handsRotationConstraintTransforms.rightHandThumb2ConstraintTransform);
            SetConstraintFollowersTargetToTarget(handsConstraintsFollowers.rightHandThumb3ConstraintTransform, weapon.handsRotationConstraintTransforms.rightHandThumb3ConstraintTransform);
        }
    }

    private void SetIKFollowersTargetToTarget(Transform follower, Transform target)
    {
        follower.GetComponent<FollowTransformPosAndRot>().target = target;
    }

    private void SetConstraintFollowersTargetToTarget(Transform follower, Transform target)
    {
        follower.GetComponent<FollowTransformRot>().target = target;
    }

    // Variable Returners
    public WeaponBase CurrentWeapon()
    {
        return currentWeapon;
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public bool IsSprinting()
    {
        return isSprinting;
    }

    public bool IsCrouching()
    {
        return isCrouching;
    }

    public bool IsGrounded()
    {
        return nearGround;
    }

    public float GetXRotation()
    {
        return xRotation;
    }

    public bool IsClipped()
    {
        return isClipped;
    }

    #endregion

    #region Input EnablingAndDisablingMethods
    private void OnEnable()
    {
        Input.Enable();
    }
    private void OnDisable()
    {
        Input.Disable();
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        // Set the gizmos color to red.
        Gizmos.color = Color.red;

        // Draw near ground check position gizmo.
        Vector3 groundCheckPosition = transform.position + new Vector3(0f, groundDistance, 0f);
        Gizmos.DrawWireSphere(groundCheckPosition, groundCheckRadius);

        // Draw sphere for radius changer.
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, gSphereCheckYOffset, 0), gSphereCheckRadius);
    }
    #endregion
}
// Great 1000 lines of code already...