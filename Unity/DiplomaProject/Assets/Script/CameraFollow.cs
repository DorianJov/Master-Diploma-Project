using UnityEngine;
using System.Collections;
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Transform target2;
    public float smoothSpeed = 0.5f;

    public Vector3 offset;
    public float zOffset = 0f; // Additional offset for the z-axis

    private Vector3 velocity = Vector3.zero;

    public float minY = 0.28f;
    public float maxY = 0.75f;

    public bool FollowTargetOne = true;

    public MoveTrashBox MoveTrashBox;

    private void Start()
    {
        MoveTrashBox.onBoosterActivated.AddListener(ActivateBoost);
    }

    void LateUpdate()
    {

        if (FollowTargetOne)
        {
            FollowTarget(target);
            offset = new Vector3(0, 0.1f, -0.69f);
            minY = 0.28f;
            maxY = 0.75f;
        }
        else
        {
            FollowTarget(target2);
            offset = new Vector3(0.12f, 0.3f, -1.37f);
            minY = 0.28f;
            maxY = 1f;
        }
    }

    void FollowTarget(Transform target)
    {

        Vector3 desiredPosition = target.position + offset;
        // Add the zOffset to the desired position's z-coordinate
        desiredPosition.z += zOffset;
        // Clamp the desired position along the Y-axis
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
    }

    void ActivateBoost()
    {
        StartCoroutine(SmoothSpeedTransition(0.3f, 0.125f, 0.5f)); // Start speed: 1, End speed: 0.125, Duration: 1 second

    }

    IEnumerator SmoothSpeedTransition(float startSpeed, float endSpeed, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float smoothSpeed = Mathf.Lerp(startSpeed, endSpeed, elapsedTime / duration);
            this.smoothSpeed = smoothSpeed;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Ensure the smoothSpeed is set to the end value after the duration
        this.smoothSpeed = endSpeed;
    }
}


