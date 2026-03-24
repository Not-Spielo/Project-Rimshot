/*=============================================================================
Script Name:    DiscFlight.cs
Last Edited:    2026-03-24
Contributors:   Grant Harvey
Description:    Manages the flight behavior of the disc
=============================================================================*/
using System.Collections;
using UnityEngine;

public class DiscFlight : MonoBehaviour
{
	[SerializeField] private Rigidbody rb;

	[Header("Flight Characteristics")]
    [SerializeField] private float glide = 1.2f;
    [SerializeField] private float turn = -1.5f;
    [SerializeField] private float fade = 2.0f;
	[SerializeField] private float spinStrength = 5f;

	[Header("Stability")]
	[SerializeField] private float minStableSpinRate = 15f;      // rad/s
	[SerializeField] private float maxStableSpinRate = 70f;      // rad/s
	[SerializeField] private float wobbleDamping = 20f;
	[SerializeField] private float selfRightingTorque = 9f;
	private float throwPower = 0.5f;


    /* GH - runs every frame, manages the flight behavior of the disc */
	private void FixedUpdate()
	{
		Vector3 velocity = rb.linearVelocity;
		float speed = velocity.magnitude;
		if (speed < 0.25f)
		{
            StartCoroutine(SetDiscLanding());
        }

		Vector3 upAxis = transform.up;
		float spinRate = Mathf.Abs(Vector3.Dot(rb.angularVelocity, upAxis));
		float spin = Mathf.InverseLerp(minStableSpinRate, maxStableSpinRate, spinRate);
		float stability = Mathf.Clamp01(spin * throwPower);

		// Dampen the wobble of disc 
		Vector3 spinVector = upAxis * Vector3.Dot(rb.angularVelocity, upAxis);
		Vector3 wobbleVector = rb.angularVelocity - spinVector;
		rb.AddTorque(-wobbleVector * wobbleDamping * stability, ForceMode.Acceleration);
	}

    /* GH - when disc lands call this */
    private IEnumerator SetDiscLanding()
    {
        // Wait to watch it land
        yield return new WaitForSeconds(2f);

        // switch camera and set variables
        GameplayManager.Instance.diskInFlight = false; 
        CameraManager.Instance.SwitchToCamera(CameraManager.Instance.mainCamera);
        yield return new WaitForSeconds(0.1f);

        Destroy(this.gameObject);
	}

    /* GH - When you throw disk, call this to initialise the disc */
    public void InitializeFlight(float normalizedPower)
    {
        throwPower = Mathf.Clamp01(normalizedPower); 
		StartCoroutine(HandleFlightCameras());
    }

    /* GH - Dynamically change cameras when disc thrown */
    private IEnumerator HandleFlightCameras()
    {
        CameraManager cm = CameraManager.Instance;

        // --- CASE 1: POWERFUL THROW (75% to 100%) ---
        if (throwPower >= 0.75f)
        {
            // start with Sky or Main
            bool myBool = Random.value > 0.1f;
            if (myBool)
                cm.SwitchToCamera(cm.actionCameraSky);
            else
                cm.SwitchToCamera(cm.actionCameraClose);

            yield return new WaitForSeconds(2f);

            // switch to Follow to see the landing
            cm.SwitchToCamera(cm.actionCameraFollow);
        }
        // --- CASE 2: MEDIUM THROW (30% to 75%) ---
        else if (throwPower >= 0.3f)
        {
            // Swap to Follow camera
            cm.SwitchToCamera(cm.actionCameraFollow);
        }
        // --- CASE 3: WEAK THROW (0% to 30%) ---
        else
        {
            // Stay on Sky or Main
            bool myBool = Random.value > 0.9f;
            if (myBool)
                cm.SwitchToCamera(cm.actionCameraSky);
            else
                cm.SwitchToCamera(cm.actionCameraClose);
        }
    }
}
