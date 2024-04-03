using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrashBox : MonoBehaviour
{
    // Start is called before the first frame update

    float Speed = 0.0f; //Don’t touch this

    Rigidbody m_Rigidbody;

    public float MaxSpeed = 1f; //This is the maximum speed that the object will achieve
    public float Acceleration = 0.1f; //How fast will object reach a maximum speed
    public float Deceleration = 10f; //How fast will object reach a speed of 0

    public Transform rectangle; // Reference to the rectangle object
    public float MaxRotation = 45f; // Maximum rotation angle of the rectangle

    public float IdleMaxSpeedRange = 0.05f; // Range of speed within which the idle rotation effect occurs
    public float IdleRotationAmplitude = 10f; // Amplitude of the idle rotation
    public float IdleRotationFrequency = 2f; // Frequency of the idle rotation



    void Start()
    {

        m_Rigidbody = GetComponent<Rigidbody>();

    }


    void Update()
    {

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
            else
                Speed = 0;

        }


        // Calculate base rotation angle based on the speed of the sphere
        float baseRotationAngle = Mathf.Lerp(-MaxRotation, MaxRotation, Mathf.InverseLerp(-MaxSpeed, MaxSpeed, Speed));

        // Calculate idle rotation only when speed is within the defined range
        float idleRotationOffset = 0f;
        if (Mathf.Abs(Speed) >= (MaxSpeed - IdleMaxSpeedRange) && Mathf.Abs(Speed) <= (MaxSpeed + IdleMaxSpeedRange))
        {
            idleRotationOffset = Mathf.Sin(Time.time * IdleRotationFrequency) * IdleRotationAmplitude;
        }

        // Apply rotation to the rectangle with the idle rotation offset
        rectangle.localRotation = Quaternion.Euler(-baseRotationAngle + idleRotationOffset, 90f, 0f); // Invert rotation around the X-axis

        Vector3 controlKeysMovement = new(Speed * Time.deltaTime, 0f, 0f);
        m_Rigidbody.MovePosition(transform.position += controlKeysMovement);


        m_Rigidbody.velocity = Vector3.zero;

        Debug.Log("Current Speed: " + Speed);
    }

}
