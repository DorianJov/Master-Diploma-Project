using System.Collections.Generic;
using UnityEngine;

public class DelayedFollower : MonoBehaviour
{
    public Transform target; // The target object to follow
    public float delay = 0.01f; // Delay time in seconds
    public float zOffset = 0.0f; // Z-axis offset for the follower

    private Queue<Vector3> positionHistory; // Queue to store the target's position history with timestamps
    private Queue<float> timeHistory; // Queue to store the corresponding times

    void Start()
    {
        positionHistory = new Queue<Vector3>();
        timeHistory = new Queue<float>();
    }

    void Update()
    {
        // Add the target's current position to the queue with the Z offset applied
        Vector3 targetPositionWithOffset = target.position;
        targetPositionWithOffset.z += zOffset;
        positionHistory.Enqueue(targetPositionWithOffset);
        timeHistory.Enqueue(Time.time);

        // Remove positions from the queue if the delay time has passed
        while (timeHistory.Count > 0 && Time.time - timeHistory.Peek() > delay)
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
    }
}
