using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.5f;

    public Vector3 offset;
    public float zOffset = 0f; // Additional offset for the z-axis

    private Vector3 velocity = Vector3.zero;

    public float minY = -1;
    public float maxY = 0.6f;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        // Add the zOffset to the desired position's z-coordinate
        desiredPosition.z += zOffset;
        // Clamp the desired position along the Y-axis
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
    }
}


