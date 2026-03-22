/*=============================================================================
Script Name:    Player.cs
Last Edited:    2026-03-22
Contributors:   Grant Harvey
Description:    Manages the player's behavior and interactions including camera rotation and disc throwing mechanics
=============================================================================*/
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float mouseLookSpeed = 2.0f;
    [SerializeField] private float keyboardLookSpeed = 80.0f;
    private float rotationY;

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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /* GH - Runs every frame, controls player input */
    void Update()
    {
        HandleCameraRotation();

        #region GH - Hold Down ThrowDisc button for power
        if ( Input.GetButtonDown("ThrowDisc") )
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

    /* GH - Simple Camera Rotation based on mouse movement or keyboard input */
    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float horizontal = Input.GetAxis("Horizontal");

        rotationY += (mouseX * mouseLookSpeed) + (horizontal * keyboardLookSpeed *Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
    }

    /* GH - instantiate a disc prefab and apply a force and torque to it */
    private void ThrowDisc(float charge)
    {
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

        GameObject disc = Instantiate(discDriver, handTransform.position, handTransform.rotation);

        Rigidbody rb = disc.GetComponent<Rigidbody>();
        DiscFlight discFlight = disc.GetComponent<DiscFlight>();

        rb.linearVelocity = throwDir * releaseSpeed;
        rb.AddForce(throwDir * force, ForceMode.Impulse);
        rb.AddTorque(handTransform.up * spinImpulse, ForceMode.Impulse);

        if (discFlight != null)
        {
            discFlight.InitializeFlight(normalized);
        }

        Debug.Log($"Charge Time: {chargeTime} \nAccuracy: {accuracy} \nThrow Force: {force} \nRelease Speed: {releaseSpeed}");
    }
}
