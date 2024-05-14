using UnityEngine;
using System.Collections;
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Transform target2;
    public GameObject bacParentRotation;

    public Transform target3;

    public Transform target4;

    public float smoothSpeed = 0.5f;
    public float smoothSpeed2 = 0.5f;

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
    public float fovTargetThree = 80f; // FOV value for target three
    public float fovTargetFour = 60f; // FOV value for target four

    public float fovTransitionDuration = 1.0f; // Duration of FOV transition

    float fovValueAnim = 0f;
    bool animationFOV = false;
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
                FollowTarget(target, smoothSpeed);
                SmoothTransitionFOV(fovTargetOne);
                offset = new Vector3(0, 0.1f, -0.69f);
                minY = 0.28f;
                maxY = 0.75f;
                break;
            case 2:
                FollowTarget(target2, smoothSpeed);
                controlFOV();
                SmoothTransitionFOV(fovTargetTwo);
                offset = new Vector3(0.12f, 0.3f, -1.37f);
                minY = 0.28f;
                maxY = 3f;
                break;

            case 3:
                FollowTarget(target3, smoothSpeed2);
                SmoothTransitionFOV(fovTargetThree);
                offset = new Vector3(0.12f, 0.3f, -1.37f);
                minY = -20f;
                maxY = 3f;
                break;

            case 4:
                FollowTarget(target4, smoothSpeed);
                SmoothTransitionFOV(fovTargetFour);
                offset = new Vector3(0.12f, 0.3f, -1.37f);
                minY = -1.2f;
                maxY = 3f;
                break;
        }

    }

    void FollowTarget(Transform target, float smoothSpeed)
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

    public void SwitchCamTarget(int target)
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

    private void controlFOV()
    {
        // Check if the spotlightToTurnOff is not null and has a Light component
        if (bacParentRotation != null)
        {
            pinceScript pinceScript = bacParentRotation.GetComponent<pinceScript>();

            // Check if the spotlight has a Light component
            if (pinceScript != null)
            {
                if (pinceScript.currenteuleurAngles >= 1)
                {
                    animationFOV = true;
                }
                // Disable the light component
                fovTargetTwo = MapValue(pinceScript.currenteuleurAngles, oldMin, oldMax, newMin, newMax);
                //Debug.Log("fovTargetTwoMapped =  " + fovTargetTwo);
                //Camera.main.fieldOfView = fovValueAnim;
            }
            else
            {
                Debug.LogError("No pinceScript component found on bacParentRotation GameObject.");
            }
        }
        else
        {
            Debug.LogError("bacParentRotation GameObject is null.");
        }
    }

    // Assuming currenteuleurAngles is in the range of 0 to 90
    // Define the ranges
    float oldMin = 0f;
    float oldMax = 90f;
    float newMin = 100f;
    float newMax = 60f;

    // MapValue function to map a value from one range to another
    float MapValue(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
    }

}


