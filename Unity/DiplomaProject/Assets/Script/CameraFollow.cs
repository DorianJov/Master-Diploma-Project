using UnityEngine;
using System.Collections;
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Transform target2;

    public Transform target3;

    public float smoothSpeed = 0.5f;

    public Vector3 offset;
    public float zOffset = 0f; // Additional offset for the z-axis

    private Vector3 velocity = Vector3.zero;

    public float minY = 0.28f;
    public float maxY = 0.75f;

    public float minX = -Mathf.Infinity; // Minimum X value (no restriction)
    public float maxX = Mathf.Infinity; // Maximum X value (no restriction)

    public bool FollowTargetOne = true;
    public int currentTargetNumber = 3;

    public MoveTrashBox MoveTrashBox;

    // FOV transition parameters
    public float fovTargetOne = 75f; // FOV value for target one
    public float fovTargetTwo = 100f; // FOV value for target two

    public float fovTargetThree = 170f; // FOV value for target two
    public float fovTransitionDuration = 1.0f; // Duration of FOV transition

    private void Start()
    {
        MoveTrashBox.onBoosterActivated.AddListener(ActivateBoost);
        MoveTrashBox.CamFollowBacLimule.AddListener(() => SwitchCamTarget(2));
        transform.position = new Vector3(-70.608f, 0.871462f, 0.179736f);
    }

    void LateUpdate()
    {
        switch (currentTargetNumber)
        {
            case 0:
                //followNothing
                break;
            case 1:
                FollowTarget(target);
                SmoothTransitionFOV(fovTargetOne);
                offset = new Vector3(0, 0.1f, -0.69f);
                minY = 0.28f;
                maxY = 0.75f;
                break;
            case 2:
                FollowTarget(target2);
                SmoothTransitionFOV(fovTargetTwo);
                offset = new Vector3(0.12f, 0.3f, -1.37f);
                minY = 0.28f;
                maxY = 1f;
                break;

            case 3:
                FollowTarget(target3);
                SmoothTransitionFOV(fovTargetThree);
                offset = new Vector3(0.12f, 0.3f, -1.37f);
                minY = -1.2f;
                maxY = 1f;
                break;
        }

    }

    void FollowTarget(Transform target)
    {
        //before setting x max position

        /*Vector3 desiredPosition = target.position + offset;
        // Add the zOffset to the desired position's z-coordinate
        desiredPosition.z += zOffset;
        // Clamp the desired position along the Y-axis
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
        */

        //after setting x max pos

        Vector3 desiredPosition = target.position + offset;
        // Add the zOffset to the desired position's z-coordinate
        desiredPosition.z += zOffset;
        // Clamp the desired position along the Y-axis
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        // Clamp the desired position along the X-axis
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX); // Add this line

        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
    }

    void ActivateBoost()
    {
        StartCoroutine(SmoothSpeedTransition(0.3f, 0.125f, 0.5f)); // Start speed: 1, End speed: 0.125, Duration: 1 second

    }

    void SwitchCamTarget(int target)
    {
        currentTargetNumber = target;

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


    void SmoothTransitionFOV(float targetFOV)
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, Time.deltaTime / fovTransitionDuration);
    }
}


