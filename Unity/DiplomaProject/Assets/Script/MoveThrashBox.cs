using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrashBox : MonoBehaviour
{
    float Speed = 0.0f;
    Rigidbody m_Rigidbody;

    public float MaxSpeed = 1f;
    public float Acceleration = 0.1f;
    public float Deceleration = 10f;

    public Transform rectangle;
    public float MaxRotation = 45f;
    public float IdleMaxSpeedRange = 0.05f;
    public float IdleRotationAmplitude = 10f;
    public float IdleRotationFrequency = 2f;

    public bool invertspeed = false;

    private Coroutine boostCoroutine;
    private float originalMaxSpeed;

    public float boostMaxSpeed = 2f;
    public float boostDuration = 0.5f;
    public float boostFadeDuration = 1f;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        originalMaxSpeed = MaxSpeed;
    }

    void Update()
    {
        if (invertspeed)
        {
            Speed = Speed * -1;
        }

        if (Speed != 0)
        {
            //emissionModule.enabled = true;
        }
        else
        {
            //emissionModule.enabled = false;
        }

        if (Input.GetKey("a"))
        {
            if (Speed > -MaxSpeed) Speed -= Acceleration * Time.deltaTime;
        }
        else if (Input.GetKey("d"))
        {
            if (Speed < MaxSpeed) Speed += Acceleration * Time.deltaTime;
        }
        else
        {
            if (Speed > Deceleration * Time.deltaTime) Speed -= Deceleration * Time.deltaTime;
            else if (Speed < -Deceleration * Time.deltaTime) Speed += Deceleration * Time.deltaTime;
            else Speed = 0;
        }

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

        Debug.Log("Current Speed: " + Speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InvertSpeed"))
        {
            Speed = Speed * -0.8f;
        }

        if (other.CompareTag("Booster"))
        {
            if (boostCoroutine != null)
                StopCoroutine(boostCoroutine);

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
            // Increase both Speed and MaxSpeed
            Speed += boostAcceleration * Time.deltaTime;
            MaxSpeed = Mathf.Max(MaxSpeed, Speed);

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
