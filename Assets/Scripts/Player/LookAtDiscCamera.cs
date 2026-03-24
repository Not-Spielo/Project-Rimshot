/*=============================================================================
Script Name:    FollowCamera.cs
Last Edited:    2026-03-24
Contributors:   Grant Harvey
Description:    Camera this is attached to should follow the disc and end looking down at it.
=============================================================================*/
using UnityEngine;

public class LookAtDiscCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    private Transform discTransform;
    private Vector3 velocity = Vector3.zero;

    /* GH - Call this from your Player script when you throw */
    public void SetTargetDisc(Transform newDiscTransform)
    {
        discTransform = newDiscTransform;
    }

    /* GH - Look at Disc */
    private void LateUpdate()
    {
        if (discTransform != null)
        {
            transform.LookAt(discTransform);
        }
    }
}
