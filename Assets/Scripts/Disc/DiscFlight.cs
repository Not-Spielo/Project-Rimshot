/*=============================================================================
Script Name:    DiscFlight.cs
Last Edited:    2026-03-22
Contributors:   Grant Harvey
Description:    Manages the flight behavior of the disc
=============================================================================*/
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

	public void InitializeFlight(float normalizedPower)
	{
		throwPower = Mathf.Clamp01(normalizedPower);
	}

    /* GH - runs every frame, manages the flight behavior of the disc */
	private void FixedUpdate()
	{
		Vector3 velocity = rb.linearVelocity;
		float speed = velocity.magnitude;
		if (speed < 0.25f)
		{
			return;
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
}
