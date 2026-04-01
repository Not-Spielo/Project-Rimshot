/*=============================================================================
Script Name:    FollowCamera.cs
Last Edited:    2026-03-24
Contributors:   Grant Harvey
Description:    Camera this is attached to should follow the disc and end looking down at it.
=============================================================================*/
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime;

    private Transform discTransform;
    private Vector3 velocity = Vector3.zero;

    /* GH - Call this from your Player script when you throw */
    public void SetTargetDisc(Transform newDiscTransform)
    {
        discTransform = newDiscTransform;
    }

    /* GH - snap to the disc offset instantly */
    private void OnEnable()
    {
        if (discTransform != null)
        {
            cameraTransform.position = discTransform.position + offset;
            velocity = Vector3.zero;
        }
    }

    /* GH - Follow disc */
    private void LateUpdate()
    {
        if (discTransform != null)
        {
            Vector3 targetPosition = discTransform.position + offset;
            cameraTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            transform.LookAt(discTransform);
        }
    }
}
