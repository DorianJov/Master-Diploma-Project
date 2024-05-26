using System.Collections.Generic;
using UnityEngine;

public class DelayedFollower : MonoBehaviour
{
    public Transform target; // The target object to follow
    public float delay = 0.01f; // Delay time in seconds
    public float zOffset = 0.0f; // Z-axis offset for the follower
    public float smoothTransitionSpeed = 0.1f; // Speed at which to smoothly transition to the new position
    public float positionTolerance = 0.01f; // Tolerance for position comparison

    private Queue<Vector3> positionHistory; // Queue to store the target's position history with timestamps
    private Queue<float> timeHistory; // Queue to store the corresponding times

    private float currentDelay;

    public bool canChangeDelay = true;

    public bool tigeScene = true;

    bool ismoving = false;

    void Start()
    {
        positionHistory = new Queue<Vector3>();
        timeHistory = new Queue<float>();
        currentDelay = delay;
    }

    void Update()
    {
        // Add the target's current position to the queue with the Z offset applied
        Vector3 targetPositionWithOffset = target.position;
        targetPositionWithOffset.z += zOffset;
        positionHistory.Enqueue(targetPositionWithOffset);
        timeHistory.Enqueue(Time.time);

        // Smoothly update the current delay value
        currentDelay = Mathf.Lerp(currentDelay, delay, Time.deltaTime * smoothTransitionSpeed);

        // Remove positions from the queue if the delay time has passed
        while (timeHistory.Count > 0 && Time.time - timeHistory.Peek() > currentDelay)
        {
            positionHistory.Dequeue();
            timeHistory.Dequeue();
        }

        // Interpolate between the oldest and the next position in the queue
        if (positionHistory.Count > 1)
        {
            Vector3 startPosition = positionHistory.Peek();
            Vector3 endPosition = positionHistory.ToArray()[1];
            float startTime = timeHistory.Peek();
            float endTime = timeHistory.ToArray()[1];
            float t = (Time.time - startTime) / (endTime - startTime);

            transform.position = Vector3.Lerp(startPosition, endPosition, t);
        }
        else if (positionHistory.Count == 1)
        {
            transform.position = positionHistory.Peek();
        }

        // Check if the follower is at the target position
        if (tigeScene)
        {
            if (IsAtTargetPosition())
            {
                this.gameObject.tag = "FollowerNoTrigger";
                ismoving = false;
                // Add logic to be executed when the follower is at the target position
                //Debug.Log("Follower is at the target position");
            }
            else
            {
                this.gameObject.tag = "Player";
                //Debug.Log("ismoving");
                ismoving = true;
            }

        }
        else
        {
            this.gameObject.tag = "Player";
        }
    }

    private bool IsAtTargetPosition()
    {
        Vector3 targetPositionWithOffset = target.position;
        targetPositionWithOffset.z += zOffset;
        return Vector3.Distance(transform.position, targetPositionWithOffset) <= positionTolerance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (canChangeDelay)
        {
            if (ismoving)
            {
                if (other.CompareTag("tigeInsideUsine"))
                {
                    tigeScene = true;
                    //TouchedWall++;
                    print("Should change delay");
                    delay = Random.Range(0.1f, 10f);
                    //smoothTransitionSpeed = Random.Range(0.1f, 5f);
                }
            }
        }

        if (other.CompareTag("secondfloorbutton"))
        {
            tigeScene = false;
            canChangeDelay = true;
        }
    }
}
