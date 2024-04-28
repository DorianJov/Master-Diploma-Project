using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSphere : MonoBehaviour
{
    // Start is called before the first frame update


    float Speed = 0.0f; //Don’t touch this
    float Speed2 = 0.0f; //Don’t touch this

    float t = 0;

    Rigidbody m_Rigidbody;
    public float MaxDashSpeed = 5f;
    public float DashsmoothSpeed = 0.5f;
    private float dashSpeed = 1f; //Don’t touch this
    public float MaxSpeed = 0.5f; //This is the maximum speed that the object will achieve
    public float Acceleration = 0.1f; //How fast will object reach a maximum speed
    public float Deceleration = 10f; //How fast will object reach a speed of 0

    public float speedUpDown = 1;
    public float distanceUpDown = 1;

    public float speedUpDownUP = 1;
    public float distanceUpDownUP = 1;
    public float arrowKeySpeed = 5f; // Speed for arrow key movement
    public float orbitSpeed = 1f; // Adjust this to control the speed of the orbit
    public float orbitRadius = 2f; // Adjust this to control the radius of the orbit
    public float noiseFrequency = 1f; // Adjust this to control the amount of noise
    public float noiseAmplitude = 0.1f; // Limit the amplitude of the noise to control the range


    ParticleSystem myParticleSystem;
    ParticleSystem.EmissionModule emissionModule;

    ParticleSystem myParticleSystem2;
    ParticleSystem.EmissionModule emissionModule2;

    [Header("Audio Parameters")]
    // Define minimum and maximum speeds for the audio volume mapping
    AudioSource movementXAudio;
    AudioSource movementYAudio;
    public float minSpeed = 0f;
    public float maxSpeed = 2f;
    public float minVolume = 0f; // Minimum volume when speed is 0
    public float maxVolume = 1f; // Maximum volume when speed is at MaxSpeed

    void Start()
    {

        m_Rigidbody = GetComponent<Rigidbody>();

        // Get the system and the emission module.
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
        emissionModule = myParticleSystem.emission;
        //var emission = ps.emission;

        myParticleSystem2 = GetComponent<ParticleSystem>();
        AudioSource[] audios = GetComponents<AudioSource>();
        movementXAudio = audios[0];
        movementYAudio = audios[1];

    }


    void Update()
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

        if (Input.GetKeyDown(KeyCode.Mouse0) & dashSpeed == 1)
        {

            dashSpeed = MaxDashSpeed;
            //Debug.Log("CLICKEDCLICKEDCLICKEDCLICKEDCLICKEDCLICKEDCLICKEDCLICKED");
        }
        else
        {
            //Debug.Log("dashSpeed : " + dashSpeed);
            if (dashSpeed != 1)
            {
                dashSpeed = Mathf.Lerp(5, 1, t += DashsmoothSpeed * Time.deltaTime);
            }
            else
            {
                t = 0;
            }
        }


        Vector3 controlKeysMovement = new(Speed * Time.deltaTime * dashSpeed, Speed2 * Time.deltaTime * dashSpeed, 0f);
        m_Rigidbody.MovePosition(transform.position += controlKeysMovement);

        /*float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the angle based on time to achieve rotation
        float angle = Time.time * orbitSpeed;

        // Calculate the noise values
        float xNoise = Mathf.PerlinNoise(Time.time * noiseFrequency, 0f) * 2f - 1f;
        float yNoise = Mathf.PerlinNoise(0f, Time.time * noiseFrequency) * 2f - 1f;

        // Limit the amplitude of the noise
        xNoise *= noiseAmplitude;
        yNoise *= noiseAmplitude;

        // Combine arrow key movement and orbital movement
        Vector3 arrowKeyMovement = new Vector3(horizontalInput, verticalInput, 0f);
        Vector3 orbitalMovement = new Vector3(Mathf.Cos(angle) * orbitRadius + xNoise, Mathf.Sin(angle) * orbitRadius + yNoise, 0f);

        // Update the position
        transform.position += (controlKeysMovement * arrowKeySpeed + orbitalMovement) * Time.deltaTime;
        */


        m_Rigidbody.MovePosition(transform.position += transform.right * Mathf.Sin(speedUpDown * Time.time) * Time.deltaTime * distanceUpDown);
        m_Rigidbody.MovePosition(transform.position += transform.up * Mathf.Sin(speedUpDownUP * Time.time) * Time.deltaTime * distanceUpDownUP);
        m_Rigidbody.velocity = Vector3.zero;

        // Map the current speed to the volume of the audio
        float volume = Mathf.Lerp(minVolume, maxVolume, Mathf.InverseLerp(minSpeed, maxSpeed, Mathf.Abs(Speed)));
        float volume2 = Mathf.Lerp(minVolume, maxVolume, Mathf.InverseLerp(minSpeed, maxSpeed, Mathf.Abs(Speed2)));

        // Set the volume of the audio source
        movementXAudio.volume = volume;
        movementYAudio.volume = volume2;

        //Debug.Log("Current Speed: " + Speed);
    }


    /*
    void Update()
    {
        float speed = 0.4f;  // ...or whatever.

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
        }
        //
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
        }
    }
    */
}
