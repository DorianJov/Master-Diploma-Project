using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToTheSkySphere : MonoBehaviour
{
    float Speed = 0.0f; //Don’t touch this
    float Speed2 = 0.0f; //Don’t touch this

    float t = 0;

    Rigidbody m_Rigidbody;

    [Header("MovementSpeed")]
    public float MaxDashSpeed = 5f;
    public float DashsmoothSpeed = 0.5f;
    private float dashSpeed = 1f; //Don’t touch this
    public float MaxSpeed = 0.5f; //This is the maximum speed that the object will achieve
    public float Acceleration = 0.1f; //How fast will object reach a maximum speed
    public float Deceleration = 10f; //How fast will object reach a speed of 0

    [Header("Idle")]
    public float speedUpDown = 1;
    public float distanceUpDown = 1;
    public float speedUpDownUP = 1;
    public float distanceUpDownUP = 1;

    [Header("Particles")]
    public ParticleSystem myParticleSystem;
    ParticleSystem.EmissionModule emissionModule;
    ParticleSystem.EmissionModule emissiondeathParticles;
    public ParticleSystem deathParticles;

    [Header("Audio Parameters")]
    // Define minimum and maximum speeds for the audio volume mapping
    AudioSource movementXAudio;
    AudioSource movementYAudio;
    public float minSpeed = 0f;
    public float maxSpeed = 2f;
    public float minVolume = 0f; // Minimum volume when speed is 0
    public float maxVolume = 1f; // Maximum volume when speed is at MaxSpeed

    private Animator animator;

    void Start()
    {
        //animator
        //Rigibody
        m_Rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        //particles
        emissionModule = myParticleSystem.emission;
        emissiondeathParticles = deathParticles.emission;

        //audios
        AudioSource[] audios = GetComponents<AudioSource>();
        movementXAudio = audios[0];
        movementYAudio = audios[1];

    }

    public void SpawnMeAtTargetPosition()
    {

    }


    void Update()
    {



    }

    public void NormalMove()
    {
        if (Speed != 0 || Speed2 != 0)
        {

            emissionModule.enabled = true;

        }
        else
        {
            emissionModule.enabled = false;

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

        if (Input.GetKey("s"))
        {
            if (Speed2 > -MaxSpeed) Speed2 -= Acceleration * Time.deltaTime;

        }
        else if (Input.GetKey("w"))
        {
            if (Speed2 < MaxSpeed) Speed2 += Acceleration * Time.deltaTime;

        }

        else
        {
            if (Speed2 > Deceleration * Time.deltaTime) Speed2 -= Deceleration * Time.deltaTime;
            else if (Speed2 < -Deceleration * Time.deltaTime) Speed2 += Deceleration * Time.deltaTime;
            else
                Speed2 = 0;

        }


        Vector3 controlKeysMovement = new(Speed * Time.deltaTime * dashSpeed, Speed2 * Time.deltaTime * dashSpeed, 0f);
        m_Rigidbody.MovePosition(transform.position += controlKeysMovement);
        m_Rigidbody.MovePosition(transform.position += transform.right * Mathf.Sin(speedUpDown * Time.time) * Time.deltaTime * distanceUpDown);
        m_Rigidbody.MovePosition(transform.position += transform.up * Mathf.Sin(speedUpDownUP * Time.time) * Time.deltaTime * distanceUpDownUP);
        m_Rigidbody.velocity = Vector3.zero;

        // Map the current speed to the volume of the audio
        float volume = Mathf.Lerp(minVolume, maxVolume, Mathf.InverseLerp(minSpeed, maxSpeed, Mathf.Abs(Speed)));
        float volume2 = Mathf.Lerp(minVolume, maxVolume, Mathf.InverseLerp(minSpeed, maxSpeed, Mathf.Abs(Speed2)));

        // Set the volume of the audio source
        movementXAudio.volume = volume;
        movementYAudio.volume = volume2;
    }

}
