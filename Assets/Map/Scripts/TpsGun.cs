using UnityEngine;

public class TpsGun : MonoBehaviour
{
    [Tooltip("The scaling number for changing the local position Y of TpsGun when aiming angle changes.")]
    [SerializeField]
    private float localPositionYScale = 0.007f;
    [SerializeField]
    private ParticleSystem gunParticles;
    [SerializeField]
    private AudioSource gunAudio;
    [SerializeField]
    private FpsGun fpsGun;
    [SerializeField]
    private Animator animator;

    private float timer;
    private Vector3 localPosition;
    private Quaternion localRotation;
    private float smoothing = 2.0f;
    private float defaultLocalPositionY;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        defaultLocalPositionY = transform.localPosition.y;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Synchronize rotation with the FpsGun
        transform.rotation = fpsGun.transform.rotation;
    }

    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    void LateUpdate()
    {
        float deltaEulerAngle = 0f;
        if (transform.eulerAngles.x > 180)
        {
            deltaEulerAngle = 360 - transform.eulerAngles.x;
        }
        else
        {
            deltaEulerAngle = -transform.eulerAngles.x;
        }

        // Adjust local position based on aiming angle
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            defaultLocalPositionY + deltaEulerAngle * localPositionYScale,
            transform.localPosition.z
        );
    }

    /// <summary>
    /// Public function to trigger the shooting effect.
    /// </summary>
    public void TriggerShootEffect()
    {
        Shoot();
    }

    /// <summary>
    /// Function to handle shooting effects.
    /// </summary>
    private void Shoot()
    {
        gunAudio.Play();

        if (gunParticles.isPlaying)
        {
            gunParticles.Stop();
        }

        gunParticles.Play();
    }
}
