using UnityEngine;

public class pinceScript : MonoBehaviour
{
    public float rotationSpeed = 0.1f; // Time interval between rotations in seconds
    public float stepSize = 5f; // Size of each rotation step
    public KeyCode rotateClockwiseKey = KeyCode.D; // Key to rotate clockwise
    public KeyCode rotateCounterClockwiseKey = KeyCode.A; // Key to rotate counter-clockwise
    AudioSource audioSource; // Reference to the AudioSource component

    private bool rotateClockwise = false;
    private bool rotateCounterClockwise = false;
    private float nextRotationTime = 0f;
    private float minRotationInterval = 0.1f; // Minimum time interval between rotations

    private void Start()
    {
        // Get reference to the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        // Check if the "D" key is pressed and Z rotation is not yet 90
        if (Input.GetKeyDown(rotateClockwiseKey) && transform.rotation.eulerAngles.z < 90f)
        {
            rotateClockwise = true;
            nextRotationTime = Time.time; // Start rotation immediately

        }
        // Check if the "A" key is pressed and Z rotation is not yet 0
        else if (Input.GetKeyDown(rotateCounterClockwiseKey) && transform.rotation.eulerAngles.z > 0f)
        {
            rotateCounterClockwise = true;
            nextRotationTime = Time.time; // Start rotation immediately

        }

        // Check if rotation keys are released
        if (Input.GetKeyUp(rotateClockwiseKey))
        {
            rotateClockwise = false;
        }

        if (Input.GetKeyUp(rotateCounterClockwiseKey))
        {
            rotateCounterClockwise = false;
        }

        // If rotation keys are pressed, rotate the object based on rotation speed
        if (rotateClockwise && Time.time >= nextRotationTime)
        {
            RotateClockwise();
            nextRotationTime = Time.time + Mathf.Max(rotationSpeed, minRotationInterval); // Update next rotation time
            PlayRotationSound();
        }
        else if (rotateCounterClockwise && Time.time >= nextRotationTime)
        {
            RotateCounterClockwise();
            nextRotationTime = Time.time + Mathf.Max(rotationSpeed, minRotationInterval); // Update next rotation time
            PlayRotationSound();
        }
    }

    private void RotateClockwise()
    {
        float currentRotation = transform.rotation.eulerAngles.z;
        float newRotation = currentRotation + stepSize;

        // Clamp rotation to 90 degrees
        newRotation = Mathf.Clamp(newRotation, 0f, 90f);

        transform.rotation = Quaternion.Euler(0f, 0f, newRotation);

        // Stop rotation if reached 90 degrees
        if (newRotation >= 90f)
        {
            rotateClockwise = false;
        }
    }

    private void RotateCounterClockwise()
    {
        float currentRotation = transform.rotation.eulerAngles.z;
        float newRotation = currentRotation - stepSize;

        // Clamp rotation to 0 degrees
        newRotation = Mathf.Clamp(newRotation, 0f, 90f);

        transform.rotation = Quaternion.Euler(0f, 0f, newRotation);

        // Stop rotation if reached 0 degrees
        if (newRotation <= 0f)
        {
            rotateCounterClockwise = false;
        }
    }

    private void PlayRotationSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
