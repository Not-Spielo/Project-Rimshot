/*=============================================================================
Script Name:    Player.cs
Last Edited:    2026-03-24
Contributors:   Grant Harvey
Description:    Manages the player's behavior and interactions including camera rotation and disc throwing mechanics
=============================================================================*/
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [Header("Character Settings")]
    [SerializeField] private float characterHeightOffset = 1.0f;
    [SerializeField] private float mouseLookSpeed = 2.0f;
    [SerializeField] private float keyboardLookSpeed = 80.0f;
    [SerializeField] private float minVerticalAngle = -80f;
    [SerializeField] private float maxVerticalAngle = 80f;
    private float rotationY;
    private float rotationX;

    [Header("Disc Settings")]
    [SerializeField] private Transform handTransform;
    [SerializeField] private float maxChargeTime = 2.0f;
    [SerializeField] private float minThrowForce = 5f;
    [SerializeField] private float maxThrowForce = 25f;
    [SerializeField] private float minReleaseSpeed = 8f;
    [SerializeField] private float maxReleaseSpeed = 27f;
    [SerializeField] private float baseSpinImpulse = 18f;
    [SerializeField] private float maxSpinImpulse = 55f;
    private float chargeTime = 0f;
    private bool isCharging = false;
    [HideInInspector] public GameObject disc;

    [Header("Disc Settings Driver")]
    [SerializeField] private GameObject discDriver;
    [SerializeField] private float driverSpeed = 60.0f;         // yards per second
    [SerializeField] private float driverMaxDistance = 100.0f;  // feet

    [Header("Disc Settings MidRange")]
    [SerializeField] private GameObject discMidRange;
    [SerializeField] private float midRangeSpeed = 40.0f;       // yards per second
    [SerializeField] private float midRangeMaxDistance = 60.0f; // feet

    [Header("Disc Settings Putter")]
    [SerializeField] private GameObject discPutter;
    [SerializeField] private float putterSpeed = 20.0f;         // yards per second
    [SerializeField] private float putterMaxDistance = 20.0f;   // feet

    [Header("Roguelike Possible Upgrades")]
    [SerializeField] private int strokesLostPerThrow = 1;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /* GH - Runs every frame, controls player input */
    void Update()
    {
        /* If the disc has not been thrown, set location to disc, allow it to be thrown, and make sure camera is parented correctly */
        if (false == GameplayManager.Instance.diskInFlight)
        {
            // this should only proc in a small window where we set the disc movement to be done and before it gets destroyed.
            if (disc != null)
                TeleportToDisc();

            HandleCameraRotation();

            #region GH - Hold Down ThrowDisc button for power
            if (Input.GetButtonDown("ThrowDisc"))
            {
                isCharging = true;
                chargeTime = 0f;
            }

            if (isCharging && Input.GetButton("ThrowDisc"))
            {
                chargeTime += Time.deltaTime;
            }

            if (isCharging && Input.GetButtonUp("ThrowDisc"))
            {
                isCharging = false;
                ThrowDisc(chargeTime);
            }
            #endregion GH - Hold Down ThrowDisc button for power
        }
    }

    /* GH - Simple Camera Rotation based on mouse movement or keyboard input */
    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float horizontal = Input.GetAxis("Horizontal");
        float verical = Input.GetAxis("Vertical");

        rotationY += (mouseX * mouseLookSpeed) + (horizontal * keyboardLookSpeed *Time.deltaTime);
        rotationX += (mouseY * mouseLookSpeed) + (verical * keyboardLookSpeed *Time.deltaTime); 

        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        transform.rotation = Quaternion.Euler(-rotationX, rotationY, 0f);
    }

    /* GH - Teleport player to disc location and ensure they are standing on the ground */
    private void TeleportToDisc()
    {
        Vector3 targetPos = disc.transform.position;

        // Raycast downward to find the exact ground point
        // We start the ray slightly above the disc in case it's clipping into the grass
        Ray ray = new Ray(targetPos + Vector3.up * 0.5f, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2.0f))
            targetPos = hit.point;

        targetPos.y += characterHeightOffset;
        this.transform.position = targetPos;
    }

    /* GH - instantiate a disc prefab and apply a force and torque to it */
    private void ThrowDisc(float charge)
    {
        #region Disc Power and Accuracy

        GameplayManager.Instance.diskInFlight = true;
        float normalized = Mathf.Clamp01(charge / maxChargeTime);

        // Power curve
        float force = Mathf.Lerp(minThrowForce, maxThrowForce, normalized);
        float releaseSpeed = Mathf.Lerp(minReleaseSpeed, maxReleaseSpeed, normalized);
        float spinImpulse = Mathf.Lerp(baseSpinImpulse, maxSpinImpulse, normalized);

        // Accuracy penalty (overcharging hurts)
        float accuracy = 1f - Mathf.Abs(normalized - 0.75f); // sweet spot at 75%


        float spread;
        if (accuracy < 0.8f)
            spread = Mathf.Lerp(1f, 0f, accuracy); // degrees of randomness
        else
            spread = Mathf.Lerp(30f, 2f, accuracy); // degrees of randomness
        
        Vector3 throwDir = handTransform.forward;

        // Apply inaccuracy
        throwDir = Quaternion.Euler(
            Random.Range(-spread, spread),
            Random.Range(-spread, spread),
            0
        ) * throwDir;

        disc = Instantiate(discDriver, handTransform.position, handTransform.rotation);

        Rigidbody rb = disc.GetComponent<Rigidbody>();
        DiscFlight discFlight = disc.GetComponent<DiscFlight>();

        rb.linearVelocity = throwDir * releaseSpeed;
        rb.AddForce(throwDir * force, ForceMode.Impulse);
        rb.AddTorque(handTransform.up * spinImpulse, ForceMode.Impulse);

        discFlight.InitializeFlight(normalized);

        #endregion Disc Power and Accuracy

        // Give all camera scripts the disc transform
        CameraManager.Instance.actionCameraFollow.GetComponent<FollowCamera>().SetTargetDisc(disc.transform);
        CameraManager.Instance.actionCameraSky.GetComponent<LookAtDiscCamera>().SetTargetDisc(disc.transform);
        CameraManager.Instance.actionCameraClose.GetComponent<LookAtDiscCamera>().SetTargetDisc(disc.transform);

        // Update Stroke Loss
        GameplayManager.Instance.UpdateStrokes(strokesLostPerThrow);
    }
}
