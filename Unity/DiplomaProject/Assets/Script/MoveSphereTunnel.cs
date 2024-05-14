using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSphereTunnel : MonoBehaviour
{
    // Start is called before the first frame update


    float Speed = 0.0f; //Don’t touch this
    float Speed2 = 0.0f; //Don’t touch this

    float t = 0;

    Rigidbody m_Rigidbody;
    public float MaxDashSpeed = 5f;
    public float DashsmoothSpeed = 0.5f;
    private float dashSpeed = 1f; //Don’t touch this
    public float MaxSpeed = 0.3f; //This is the maximum speed that the object will achieve
    public float Acceleration = 2f; //How fast will object reach a maximum speed
    public float Deceleration = 0.1f; //How fast will object reach a speed of 0

    public float speedUpDown = 1;
    public float distanceUpDown = 1;

    public float speedUpDownUP = 1;
    public float distanceUpDownUP = 1;



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

    public bool SphereCanMove = false;
    public bool falling = true;

    public int TouchedWall = 0;

    bool boostOnSpawn = true;
    bool SpeedwasAt0 = false;
    bool CanTouchInputs = true;

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
        if (SphereCanMove)
        {
            MoveSphere();
        }
        else
        {

        }

    }


    void MoveSphere()
    {
        //Only Once when camera follow.
        if (boostOnSpawn)
        {
            Speed = 0.5f;
            Deceleration = 0.1f;
            boostOnSpawn = false;
        }

        if (Speed != 0 || Speed2 != 0)
        {
            emissionModule.enabled = true;
        }
        else
        {
            SpeedwasAt0 = true;
            Deceleration = 2f;
            if (falling)
            {
                emissionModule.enabled = true;
            }
            else
            {
                emissionModule.enabled = false;
            }
        }

        if (SpeedwasAt0 & falling)
        {
            Acceleration = 0.3f;
            MaxSpeed = 0.5f;
        }

        if (CanTouchInputs)
        {
            if (Input.GetKey("a") & SpeedwasAt0)
            {
                if (Speed > -MaxSpeed) Speed -= Acceleration * Time.deltaTime;

            }
            else if (Input.GetKey("d") & SpeedwasAt0)
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

            if (Input.GetKey("s") & SpeedwasAt0 & !falling)
            {
                if (Speed2 > -MaxSpeed) Speed2 -= Acceleration * Time.deltaTime;

            }
            else if (Input.GetKey("w") & SpeedwasAt0 & !falling)
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
        }


        Vector3 controlKeysMovement = new(Speed * Time.deltaTime * dashSpeed, Speed2 * Time.deltaTime * dashSpeed, 0f);
        m_Rigidbody.MovePosition(transform.position += controlKeysMovement);

        if (!falling)
        {
            m_Rigidbody.MovePosition(transform.position += transform.right * Mathf.Sin(speedUpDown * Time.time) * Time.deltaTime * distanceUpDown);
            m_Rigidbody.MovePosition(transform.position += transform.up * Mathf.Sin(speedUpDownUP * Time.time) * Time.deltaTime * distanceUpDownUP);
        }
        if (TouchedWall >= 1)
        {
            m_Rigidbody.velocity = Vector3.zero;
        }

        // Map the current speed to the volume of the audio
        float volume = Mathf.Lerp(minVolume, maxVolume, Mathf.InverseLerp(minSpeed, maxSpeed, Mathf.Abs(Speed)));
        float volume2 = Mathf.Lerp(minVolume, maxVolume, Mathf.InverseLerp(minSpeed, maxSpeed, Mathf.Abs(Speed2)));

        // Set the volume of the audio source
        movementXAudio.volume = volume;
        movementYAudio.volume = volume2;

        //Debug.Log("Current Speed: " + Speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("tunnel"))
        {
            TouchedWall++;
        }

    }
}




