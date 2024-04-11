using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeadThrashBox : MonoBehaviour
{
    float Speed = 0.0f;
    Rigidbody m_Rigidbody;
    AudioSource audioSource;

    public MoveTrashBox MoveTrashBox;

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

    public bool invertspeed = false;

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

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        originalMaxSpeed = MaxSpeed;
        audioSource = GetComponent<AudioSource>();
        MoveTrashBox.onBoosterActivated.AddListener(ActivateBoost);
    }

    void Update()
    {
        Speed = Speed * -1;


        if (Speed > Deceleration * Time.deltaTime) Speed -= Deceleration * Time.deltaTime;
        else if (Speed < -Deceleration * Time.deltaTime) Speed += Deceleration * Time.deltaTime;
        else Speed = 0;


        float baseRotationAngle = Mathf.Lerp(-MaxRotation, MaxRotation, Mathf.InverseLerp(-MaxSpeed, MaxSpeed, Speed));

        float idleRotationOffset = 0f;
        if (Mathf.Abs(Speed) >= (MaxSpeed - IdleMaxSpeedRange) && Mathf.Abs(Speed) <= (MaxSpeed + IdleMaxSpeedRange))
        {
            idleRotationOffset = Mathf.Sin(Time.time * IdleRotationFrequency) * IdleRotationAmplitude;
        }

        rectangle.localRotation = Quaternion.Euler(-baseRotationAngle + idleRotationOffset, 90f, 0f);

        Vector3 controlKeysMovement = new(Speed * Time.deltaTime, 0f, 0f);
        m_Rigidbody.MovePosition(transform.position += controlKeysMovement);

        m_Rigidbody.velocity = Vector3.zero;

        // Map the current speed to the volume of the audio
        float volume = Mathf.Lerp(minVolume, maxVolume, Mathf.InverseLerp(minSpeed, maxSpeed, Mathf.Abs(Speed)));

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

        /* if (other.CompareTag("Booster"))
         {
             if (boostCoroutine != null)
                 StopCoroutine(boostCoroutine);

             boostCoroutine = StartCoroutine(BoostSpeed());
         }*/
    }

    void ActivateBoost()
    {
        // Existing code...

        // Start the boost coroutine
        if (boostCoroutine != null)
            StopCoroutine(boostCoroutine);

        boostCoroutine = StartCoroutine(BoostSpeed());
    }

    IEnumerator BoostSpeed()
    {
        // Store the original max speed
        float initialMaxSpeed = MaxSpeed;

        // Calculate boost acceleration
        float speedChange = boostMaxSpeed - Speed;
        float boostAcceleration = speedChange / boostDuration;

        // Apply boost acceleration until boost duration is reached
        float elapsedTime = 0f;
        while (elapsedTime < boostDuration)
        {
            // Adjust speed based on boost acceleration
            Speed += boostAcceleration * Time.deltaTime;

            // Ensure speed doesn't exceed boostMaxSpeed
            Speed = Mathf.Clamp(Speed, -boostMaxSpeed, boostMaxSpeed);

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



}
