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

    public float fovTransitionDuration = 0.5f; // Duration of FOV transition

    float fovValueAnim = 0f;
    bool animationFOV = false;

    bool applyoffsetOnceTarget3 = true;
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
                if (applyoffsetOnceTarget3)
                {
                    offset = new Vector3(0.12f, 0.3f, -1.37f);
                    fovTransitionDuration = 0.5f;
                    minY = -20f;
                    maxY = 3f;
                    applyoffsetOnceTarget3 = false;
                }


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
        Vector3 desiredPosition = target.position + offset;
        // Add the zOffset to the desired position's z-coordinate
        desiredPosition.z += zOffset;

        // Check if Y values are negative
        if (minY < 0 && maxY < 0)
        {
            // Clamp the desired position along the Y-axis for negative values
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, maxY, minY);
        }
        else
        {
            // Clamp the desired position along the Y-axis for positive values
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }

        // Check if X values are negative
        if (minX < 0 && maxX < 0)
        {
            // Clamp the desired position along the X-axis for negative values
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, maxX, minX);
        }
        else
        {
            // Clamp the desired position along the X-axis for positive values
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        }

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

    public void CallSmoothSpeed2Transition(float startSpeed, float endSpeed, float duration)
    {
        StartCoroutine(SmoothSpeed2Transition(startSpeed, endSpeed, duration)); // Start speed: 1, End speed: 0.125, Duration: 1 second
    }

    IEnumerator SmoothSpeed2Transition(float startSpeed, float endSpeed, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float smoothSpeed2 = Mathf.Lerp(startSpeed, endSpeed, elapsedTime / duration);
            this.smoothSpeed2 = smoothSpeed2;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Ensure the smoothSpeed is set to the end value after the duration
        this.smoothSpeed2 = endSpeed;
    }


    void SmoothTransitionFOV(float targetFOV)
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, Time.deltaTime / fovTransitionDuration);
    }

    public void SmoothTransitionRotation(Quaternion targetRotation, float duration)
    {
        StartCoroutine(SmoothRotationCoroutine(targetRotation, duration));
    }

    private IEnumerator SmoothRotationCoroutine(Quaternion targetRotation, float duration)
    {
        Quaternion initialRotation = transform.rotation;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        //smoothSpeed2 = 0.2f;
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


