/*=============================================================================
Script Name:    Cameramanager.cs
Last Edited:    2026-03-22
Contributors:   Grant Harvey
Description:    Camera manager that follows the player
=============================================================================*/
using UnityEngine;

public class Cameramanager : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 _offset;
    [SerializeField] private float smoothTime;
    private Vector3 _currentVelocity = Vector3.zero;

    private void Awake()
    {
        _offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        var targetPosition = target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
    }
}
