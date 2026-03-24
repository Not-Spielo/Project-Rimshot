/*=============================================================================
Script Name:    CameraManager.cs
Last Edited:    2026-03-24
Contributors:   Grant Harvey
Description:    Camera manager to basically just hold data on cameras
=============================================================================*/
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("Main Camera Settings")]
    public Camera mainCamera;
    public Camera actionCameraFollow;
    public Camera actionCameraSky;
    public Camera actionCameraClose;

    private Camera[] allCameras;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        // Store cameras in an array to make mass-disabling easier
        allCameras = new Camera[] { mainCamera, actionCameraFollow, actionCameraSky, actionCameraClose };

        // Initialize: Only Main is on
        SwitchToCamera(mainCamera);
    }

    /* GH - Switch Active Camera */
    public void SwitchToCamera(Camera targetCamera)
    {
        if (targetCamera == null) return;

        // Disable all cameras first
        foreach (Camera cam in allCameras)
        {
            if (cam != null) cam.enabled = false;

            // Toggle AudioListener if the camera has one
            AudioListener listener = cam.GetComponent<AudioListener>();
            if (listener != null)
            {
                listener.enabled = (cam == targetCamera);
            }
        }

        // Enable the one we want
        targetCamera.enabled = true;
    }
}
