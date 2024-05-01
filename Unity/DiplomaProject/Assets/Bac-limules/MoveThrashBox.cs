using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // Added to use UnityEvents

public class MoveTrashBox : MonoBehaviour
{
    float Speed = 0.0f;
    Rigidbody m_Rigidbody;
    AudioSource audioSource;
    public UnityEvent onBoosterActivated = new UnityEvent();
    public UnityEvent CamFollowBacLimule = new UnityEvent();

    [Header("Movement Parameters")]
    public float MaxSpeed = 1f;
    public float Acceleration = 0.1f;
    public float Deceleration = 10f;

    public Transform rectangle;
    [Header("Rotation Parameters")]
    public float MaxRotation = 45f;
    public float IdleMaxSpeedRange = 0.05f;
    public float IdleRotationAmplitude = 10f;
    public float IdleRotationFrequency = 2f;

    private Coroutine boostCoroutine;
    private float originalMaxSpeed;

    [Header("Boost Parameters")]
    public float boostMaxSpeed = 2f;
    public float boostDuration = 0.5f;
    public float boostFadeDuration = 1f;

    [Header("Audio Parameters")]
    // Define minimum and maximum speeds for the audio volume mapping
    public float minSpeed = 0f;
    public float maxSpeed = 2f;
    public float minVolume = 0f; // Minimum volume when speed is 0
    public float maxVolume = 1f; // Maximum volume when speed is at MaxSpeed

    public bool gameHasStarted = true;

    public bool stopperActive = false;
    private float rotationBackDuration = 1.0f;
    public float rotationToZDuration = 0.06f;
    private Quaternion initialRotation;

    private bool canRotatetoX = false;
    private float volume = 0f;



    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        originalMaxSpeed = MaxSpeed;
        audioSource = GetComponent<AudioSource>();
        transform.position = new Vector3(-72.892f, 0.356f, 1.529f);
        initialRotation = rectangle.localRotation;

    }

    void Update()
    {


        if (gameHasStarted & Input.anyKey)
        {
            if (boostCoroutine != null)
                StopCoroutine(boostCoroutine);

            onBoosterActivated.Invoke();
            boostCoroutine = StartCoroutine(BoostSpeed());
            gameHasStarted = false;
        }
        else
        {


            if (Input.GetKey("a") & !stopperActive)
            {

                if (Speed > -MaxSpeed) Speed -= Acceleration * Time.deltaTime;
            }
            else if (Input.GetKey("d") & !stopperActive)
            {
                if (Speed < MaxSpeed) Speed += Acceleration * Time.deltaTime;
            }
            else
            {
                if (Speed > Deceleration * Time.deltaTime) Speed -= Deceleration * Time.deltaTime;
                else if (Speed < -Deceleration * Time.deltaTime) Speed += Deceleration * Time.deltaTime;
                else Speed = 0;
            }


        }


        if (!stopperActive)
        {
            canRotatetoX = false;
            //rotate bac idle + speed related
            float baseRotationAngle = Mathf.Lerp(-MaxRotation, MaxRotation, Mathf.InverseLerp(-MaxSpeed, MaxSpeed, Speed));

            float idleRotationOffset = 0f;
            if (Mathf.Abs(Speed) >= (MaxSpeed - IdleMaxSpeedRange) && Mathf.Abs(Speed) <= (MaxSpeed + IdleMaxSpeedRange))
            {
                idleRotationOffset = Mathf.Sin(Time.time * IdleRotationFrequency) * IdleRotationAmplitude;
            }

            rectangle.localRotation = Quaternion.Euler(-baseRotationAngle + idleRotationOffset, 90f, 0f);
        }
        else
        {
            // Stopper is active, rotating idle frequency is stop and rotating based on speed.
            //Now The rectangle.localrotation should return to rotation 0 in seconds. So its not stuck with a random rotation since activestopper is true.


            StartCoroutine(RotateBackToInitialRotation());





        }

        Vector3 controlKeysMovement = new(Speed * Time.deltaTime, 0f, 0f);
        m_Rigidbody.MovePosition(transform.position += controlKeysMovement);

        m_Rigidbody.velocity = Vector3.zero;


        //if stopper active == true
        if (stopperActive)
        {
            StartCoroutine(FadeVolume(0f, 1f)); // Fade to volume 0 over 1 second
                                                // Map the current speed to the volume of the audio
        }
        else
        {

            //if stopper active == false
            //fade the volume slowly in 1 seconds to 0;
            volume = Mathf.Lerp(minVolume, maxVolume, Mathf.InverseLerp(minSpeed, maxSpeed, Mathf.Abs(Speed)));
        }

        // Set the volume of the audio source
        audioSource.volume = volume;

        //Debug.Log("Current Speed: " + Speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InvertSpeed"))
        {
            Speed = Speed * -0.8f;
        }

        if (other.CompareTag("stopper-switch"))
        {
            Speed = Speed * -0.05f;
            stopperActive = true;
        }

        if (other.CompareTag("FollowCamCollider"))
        {
            CamFollowBacLimule.Invoke();
        }

        if (other.CompareTag("Booster"))
        {

            if (boostCoroutine != null)
                StopCoroutine(boostCoroutine);

            onBoosterActivated.Invoke();
            boostCoroutine = StartCoroutine(BoostSpeed());
        }
    }

    IEnumerator BoostSpeed()
    {
        // Store the original max speed
        float initialMaxSpeed = MaxSpeed;

        // Calculate boost acceleration
        float boostAcceleration = (boostMaxSpeed - Speed) / boostDuration;

        // Apply boost acceleration until boost duration is reached
        float elapsedTime = 0f;
        while (elapsedTime < boostDuration)
        {
            // Adjust speed based on boost acceleration
            Speed += boostAcceleration * Time.deltaTime;

            // Ensure speed doesn't exceed boostMaxSpeed
            Speed = Mathf.Min(Speed, boostMaxSpeed);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Gradually decrease speed back to normal
        float fadeElapsedTime = 0f;
        while (fadeElapsedTime < boostFadeDuration)
        {
            // Calculate fade factor (0 to 1)
            float fadeFactor = fadeElapsedTime / boostFadeDuration;

            // Lerp between boosted speed and original speed
            Speed = Mathf.Lerp(boostMaxSpeed, originalMaxSpeed, fadeFactor);

            fadeElapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset max speed to original value
        MaxSpeed = initialMaxSpeed;

        // Reset coroutine reference
        boostCoroutine = null;
    }


    IEnumerator RotateBackToInitialRotation()
    {
        float elapsedTime = 0f;
        Quaternion currentRotation = rectangle.localRotation;

        while (elapsedTime < rotationBackDuration)
        {
            // Calculate the interpolation factor
            float t = elapsedTime / rotationBackDuration;

            // Interpolate between the current rotation and the initial rotation
            Quaternion targetRotation = Quaternion.Lerp(currentRotation, initialRotation, t);

            // Apply the interpolated rotation
            rectangle.localRotation = targetRotation;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the object reaches the initial rotation precisely
        rectangle.localRotation = initialRotation;
        // StartCoroutine(WaitForFrames(60));

    }

    IEnumerator WaitForFrames(int frameCount)
    {
        for (int i = 0; i < frameCount; i++)
        {
            yield return null; // Wait for the next frame
        }

        // Code after waiting for the specified number of frames
        //StartCoroutine(RotateParentToZRotation(-90f));
        //Debug.Log("Waited for " + frameCount + " frames.");
    }


    IEnumerator RotateParentToZRotation(float targetZRotation)
    {
        float elapsedTime = 0f;
        Quaternion currentRotation = transform.parent.localRotation; // Access the parent's rotation

        // Calculate the target rotation based on the specified Z rotation
        Quaternion targetRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, targetZRotation);

        while (elapsedTime < rotationToZDuration)
        {
            // Calculate the interpolation factor
            float t = elapsedTime / rotationToZDuration;

            // Interpolate between the current rotation and the target rotation
            Quaternion interpolatedRotation = Quaternion.Lerp(currentRotation, targetRotation, t);

            // Apply the interpolated rotation to the parent's transform
            transform.parent.localRotation = interpolatedRotation;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the object reaches the target rotation precisely
        transform.parent.localRotation = targetRotation;
    }

    IEnumerator FadeVolume(float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            // Calculate the interpolation factor
            float t = (Time.time - startTime) / duration;

            // Interpolate between the start volume and the target volume
            float newVolume = Mathf.Lerp(startVolume, targetVolume, t);

            // Set the volume of the audio source
            audioSource.volume = newVolume;

            yield return null; // Wait for the next frame
        }

        // Ensure the volume reaches the target volume precisely
        audioSource.volume = targetVolume;
    }





}
