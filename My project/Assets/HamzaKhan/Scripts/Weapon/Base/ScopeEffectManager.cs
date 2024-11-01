using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using Cinemachine;
using UnityEngine;
using System;

public class ScopeEffectManager : MonoBehaviour
{ 

    #region Variables

    // Weapon Ref
    [Space]
    [Header("Weapon")]
    [SerializeField] private WeaponBase weapon;

    [Tooltip("Is There Any Animation That Plays When A Shot Occurs?")]
    [SerializeField] private bool animationOnShoot = false;

    // Camera ref.
    [Space]
    [Header("Camera")]
    [SerializeField] private Camera renderCamera;
    [SerializeField] private CinemachineVirtualCamera aimingVirtualCamera;

    [SerializeField] private AnimationCurve cameraBlendingCurve;

    [SerializeField] private CinemachineBlendDefinition.Style cameraBlendingStyle;

    [SerializeField] private float cameraBlendTime;
    [SerializeField] private float cameraRotationSmoothingSpeed = 15f;

    // Scope Renderer ref.
    [Space]
    [Header("Scope Renderer")]
    [SerializeField] private Renderer scopeRenderer;
    [Header("Material To Apply Texture")]
    [SerializeField] private Material materialToCopy;

    // Render Texture.
    [Space]
    [Header("Render Texture")]
    [SerializeField] private int height = 512;
    [SerializeField] private int width = 512;
    [SerializeField] private int depth = 24;

    // Vignette Effect.
    [Space]
    [Header("Vignette Effect")]
    [SerializeField] private float intensity = 0.758f;
    [SerializeField] private float smoothness = 0.04f;

    [Space(5)]
    [Header("Colors")]
    [SerializeField] private Color aimingVignetteColor = Color.black;
    [SerializeField] private Color notAimingVignetteColor = Color.white;

    [Space(5)]
    [Header("Effect Delay's & Durations")]
    [SerializeField] private float changeToAimingColorTime = 0.1f;
    [SerializeField] private float changeToNotAimingColorTime = 0.1f;
    [SerializeField] private float toAimingEffectDelay = 0.2f;
    [SerializeField] private float toNotAimingEffectDelay = 0f;

    // The Vignette Effect.
    private Vignette vignette;

    // Render Texture Variables.
    private RenderTexture renderTexture;
    private Material renderTextureMaterial;

    // Courotine
    private Coroutine ChangeVignetteColorCourotine;
    private Coroutine OnShootCoroutineHandler;
    private Coroutine OnReloadCoroutineHandler;

    private CoroutineState onShootCoroutineState = CoroutineState.notInProgress;
    private CoroutineState onReloadCoroutineState = CoroutineState.notInProgress;

    private enum CoroutineState
    {
        inProgress,
        notInProgress,
    }

    // Aiming.
    private bool isAiming = false;

    // Scoped.
    private bool isScoped = false;

    // Is Clipped.
    private bool isClipped = false;

    private CinemachineBrain cinemachineCameraBrain;

    #endregion

    #region General

    // Start is called before the first frame update
    void Start()
    {
        // Initialize.
        Initialize();
    }

    /// <summary>
    /// All Initialization is done in this function.
    /// </summary>
    private void Initialize()
    {
        // Setup Render Texture.
        SetupRenderTexture();

        // Get vignette Effect from volume.
        GlobalVolumeInstance.instance.GetGlobalVolumeRef().profile.TryGet(out vignette);

        // Check if we should enable camera or disable it.
        OnCheckForScopeEnable();

        // Assign Cinemachine Brain reference.
        cinemachineCameraBrain = CinemachineBrainInstance.instance.GetCinemachineBrain();

        // Listener For On Shoot Event
        weapon.onShoot.AddListener(OnShoot);

        // Listener for on reload event.
        weapon.onReloadStart.AddListener(OnReload);
    }

   

    void Update()
    {
        // Return if this isn't the current weapon.
        if (weapon.IsCurrentWeapon() == false) return;

        // Set The Aiming Virtual Camera Rotation
        aimingVirtualCamera.transform.rotation = Quaternion.Slerp(
            aimingVirtualCamera.transform.rotation, weapon.transform.rotation *
            Quaternion.Euler(0f, 0f, -weapon.transform.rotation.eulerAngles.z), cameraRotationSmoothingSpeed * Time.deltaTime);

        // Check if the Bool that we have stored is different from the actual one.
        if (isClipped != weapon.GetPlayer().IsClipped())
        {
            // Set our bool to the actual one.
            isClipped = weapon.GetPlayer().IsClipped();

            // Check if the new value is false, This means that it was true, Now it is false.
            if (isClipped == false)
            {
                // Invoke Function On Clipped Exit.
                OnClippedExit();
            }
        }

        // Check if we are scoped and not clipped and Both the coroutine's are not in progress.
        if (isScoped == true && weapon.GetPlayer().IsClipped() == false
            && (onShootCoroutineState == CoroutineState.notInProgress && onReloadCoroutineState == CoroutineState.notInProgress))
        {
            // Enable Aiming Virtual Camera.
            aimingVirtualCamera.gameObject.SetActive(true);

            // Enable Vignette Effect.
            vignette.active = true;
        }
        else if (isScoped == true && weapon.GetPlayer().IsClipped() == true)
        {
            // Disable Aiming Virtual Camera.
            aimingVirtualCamera.gameObject.SetActive(false);

            // Disable Vignette Effect.
            vignette.active = false;
        }
        else
        {
            // Disable Aiming Virtual Camera.
            aimingVirtualCamera.gameObject.SetActive(false);
        }

        // Vignette Effect Values.
        vignette.intensity.value = intensity;
        vignette.smoothness.value = smoothness;

        // Camera Blending Values.
        cinemachineCameraBrain.m_DefaultBlend.m_Time = cameraBlendTime;
        cinemachineCameraBrain.m_DefaultBlend.m_Style = cameraBlendingStyle;
        cinemachineCameraBrain.m_DefaultBlend.m_CustomCurve = cameraBlendingCurve;
    }

    #endregion

    #region Scoped Pausing System

    // On Shoot Event.
    private void OnShoot()
    {
        // Return if we don't have any animation to play after a shot.
        if (animationOnShoot == false) return;

        // Check if needed to stop coroutine.
        if (OnShootCoroutineHandler != null) StopCoroutine(OnShootCoroutineHandler);

        // Start OnShoot Coroutine.
        OnShootCoroutineHandler = StartCoroutine(OnShootCoroutine(weapon.weaponData.timeBetweenShot - (cameraBlendTime * 1.25f)));
    }

    // On Reload Event
    private void OnReload()
    {
        // Check if needed to stop coroutine.
        if (OnReloadCoroutineHandler != null) StopCoroutine(OnReloadCoroutineHandler);

        // Start OnRelaod Coroutine.// CHANGES MADE
        OnReloadCoroutineHandler = StartCoroutine(OnReloadCoroutine(weapon.weaponData.reloadingAnimationTime - (cameraBlendTime * 1.25f)));
    }

    /// <summary>
    /// On Shoot Coroutine.
    /// </summary>
    /// <param name="timeBetweenShot"></param>
    /// <returns></returns>
    IEnumerator OnShootCoroutine(float timeBetweenShot)
    {
        // In Progress
        onShootCoroutineState = CoroutineState.inProgress;

        // Aim Exit.
        VignetteOnAimExitHandler();

        // Is Scoped.
        isScoped = false;

        // Delay.
        yield return new WaitForSeconds(timeBetweenShot);

        // Check if we are still aiming.
        if (isAiming == true)
        {
            // Scoped Mode On.
            isScoped = true;

            // Aiming Vignette Effect.
            VignetteOnAimHandler();
        }

        // No longer in progress.
        onShootCoroutineState = CoroutineState.notInProgress;
    }

    /// <summary>
    /// On Reload Coroutine
    /// </summary>
    /// <param name="reloadingTime"></param>
    /// <returns></returns>
    IEnumerator OnReloadCoroutine(float reloadingTime)
    {
        // In Progress
        onReloadCoroutineState = CoroutineState.inProgress;

        // Aim Exit.
        VignetteOnAimExitHandler();

        // Is Scoped.
        isScoped = false;

        // Delay.
        yield return new WaitForSeconds(reloadingTime);

        // Check if we are still aiming.
        if (isAiming == true)
        {
            // Scoped Mode On.
            isScoped = true;

            // Aiming Vignette Effect.
            VignetteOnAimHandler();
        }

        // No longer in progress.
        onReloadCoroutineState = CoroutineState.notInProgress;
    }

    #endregion

    #region Render Texture

    /// <summary>
    /// Creates And Set's up a render texture for the scope system.
    /// </summary>
    private void SetupRenderTexture()
    {
        // Initialize a new Render Texture
        renderTexture = new RenderTexture(height, width, depth);
        renderTexture.Create();

        // Initialize a new Material For Applying the render texture.
        renderTextureMaterial = Instantiate(materialToCopy, transform);
        renderTextureMaterial.mainTexture = renderTexture;

        // Initialize the camera by setting the target texture and enabling it.
        renderCamera.targetTexture = renderTexture;
        renderCamera.gameObject.SetActive(true);

        // Set the Renderer's Material.
        scopeRenderer.material = renderTextureMaterial;
    }

    #endregion

    #region Events

    /// <summary>
    /// Invoked When We were clipped and just stopped clipping.
    /// </summary>
    private void OnClippedExit()
    {
        // Check if we are aiming.
        if (isAiming == true)
        {
            // Enable Vignette and Scoped.
            vignette.active = true;
            isScoped = true;

            // Handle On Aim Vignette Effect.
            VignetteOnAimHandler();
        }
    }

    /// <summary>
    /// On Aim Event, Call Through the Weapon Base Unity Event.
    /// </summary>
    public void OnAim()
    {
        // Is Aiming is true.
        isAiming = true;

        // Check if the coroutine isn't running.
        if (onShootCoroutineState == CoroutineState.notInProgress && onReloadCoroutineState == CoroutineState.notInProgress)
        {
            // Scoped Mode On.
            isScoped = true;

            // Vignette Aim Effect.
            VignetteOnAimHandler();
        }
    }

    /// <summary>
    /// On Aim Exit Event, Call Through the Weapon Base Unity Event.
    /// </summary>
    public void OnAimExit()
    {
        // Is Aiming is False.
        isAiming = false;

        // Check if the coroutine isn't running.
        if (onShootCoroutineState == CoroutineState.notInProgress && onReloadCoroutineState == CoroutineState.notInProgress)
        {
            // Scoped Mode disabled.
            isScoped = false;

            // Exit Vignette Effect.
            VignetteOnAimExitHandler();
        }
    }

    /// <summary>
    /// On Aim Vignette Effect Handler.
    /// </summary>
    private void VignetteOnAimHandler()
    {
        if (weapon.GetPlayer().IsClipped() == true) return;

        // Stop the coroutine if already started previously.
        if (ChangeVignetteColorCourotine != null) StopCoroutine(ChangeVignetteColorCourotine);

        // Start Change Vignette Coroutine For Aiming Effect.
        ChangeVignetteColorCourotine =
            StartCoroutine(ChangeVignetteColor(toAimingEffectDelay, aimingVignetteColor, changeToAimingColorTime, true));
    }

    /// <summary>
    /// On Aim Exit Vignette Effect Handler.
    /// </summary>
    private void VignetteOnAimExitHandler()
    {
        if (weapon.GetPlayer().IsClipped() == true) return;

        // Stop the coroutine if already started previously.
        if (ChangeVignetteColorCourotine != null) StopCoroutine(ChangeVignetteColorCourotine);

        // Start Change Vignette Coroutine For Aiming Exit Effect.
        ChangeVignetteColorCourotine =
            StartCoroutine(ChangeVignetteColor(toNotAimingEffectDelay, notAimingVignetteColor, changeToNotAimingColorTime, false));
    }

    public void OnCheckForScopeEnable()
    {
        if (weapon.IsCurrentWeapon() == true)
        {
            renderCamera.enabled = true;
        }
        else
        {
            renderCamera.enabled = false;
        }
    }

    #endregion

    #region Vignette Effect

    /// <summary>
    /// Change Vignette Effect Colour.
    /// </summary>
    /// <param name="effectStartDelay"></param>
    /// <param name="color"></param>
    /// <param name="time"></param>
    /// <param name="vignetteActiveAfter"></param>
    /// <returns></returns>
    IEnumerator ChangeVignetteColor(float effectStartDelay, Color color, float time, bool vignetteActiveAfter)
    {
        // Start Delay.
        yield return new WaitForSeconds(effectStartDelay);

        // Ensure Vignette Effect is enabled.
        vignette.active = true;

        // Store the initial color.
        Color initialColor = vignette.color.value;

        // ElapsedTime.
        float elapsedTime = 0f;

        // While we still have time left.
        while (elapsedTime < time)
        {
            // Add time to the elapsed amount.
            elapsedTime += Time.deltaTime;

            // Lerp the Vignette Color.
            vignette.color.value = Color.Lerp(initialColor, color, elapsedTime / time);

            // Return.
            yield return null;
        }

        // Ensure the final Color is Set.
        vignette.color.value = color;

        // Disable or enable vignette based on given info.
        vignette.active = vignetteActiveAfter;
    }

    #endregion
}