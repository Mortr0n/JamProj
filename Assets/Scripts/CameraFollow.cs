using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // The player's Transform
    public Vector3 offset;              // Offset from the player's position in local space
    public float smoothSpeed = 0.125f;  // Smoothing factor for position
    public float rotationSmoothSpeed = 0.1f; // Smoothing factor for rotation
    public bool useVerticalTracking = true;  // Enable vertical camera movement
    public float minHeight = 1f;        // Minimum camera height
    public float maxHeight = 10f;       // Maximum camera height

    void LateUpdate()
    {
        // Calculate the desired position based on the player's position and rotation
        Vector3 desiredPosition = target.TransformPoint(offset);

        // Adjust the desired position's Y value based on the player's Y position
        if (useVerticalTracking)
        {
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minHeight, maxHeight);
        }
        else
        {
            // Keep the camera at a fixed height if vertical tracking is disabled
            desiredPosition.y = transform.position.y;
        }

        // Smoothly interpolate to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;

        // Smoothly rotate the camera to match the player's rotation
        Quaternion desiredRotation = Quaternion.LookRotation(target.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothSpeed);
    }
}