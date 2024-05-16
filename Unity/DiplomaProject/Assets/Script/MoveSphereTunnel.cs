using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MoveSphereTunnel : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator animator;
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

    AudioSource AccerelationTunnelAudio;

    AudioSource limulemaxSpeedSound;
    public float minSpeed = 0f;
    public float maxSpeed = 2f;
    public float minVolume = 0f; // Minimum volume when speed is 0
    public float maxVolume = 1f; // Maximum volume when speed is at MaxSpeed

    public bool SphereCanSpawn = false;
    public bool falling = false;

    public bool slidingTunnel = true;
    public int TouchedWall = 0;

    bool boostOnSpawn = true;
    bool SpeedwasAt0 = false;
    bool CanTouchInputs = true;

    public float pitchIncreaseRate = 0.1f;

    bool maxSpeedwasReached = false;
    bool keywasreleased = false;

    bool AudioCanReplay = false;

    bool LimuleCanMoveFreely = false;

    bool smoothTransitionPlayed = false;

    bool triggerColliderMoveSphereOnce = true;

    void Start()
    {

        m_Rigidbody = GetComponent<Rigidbody>();


        // Get the system and the emission module.
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
        emissionModule = myParticleSystem.emission;
        //var emission = ps.emission;
        animator = GetComponentInChildren<Animator>();

        myParticleSystem2 = GetComponent<ParticleSystem>();
        AudioSource[] audios = GetComponents<AudioSource>();
        movementXAudio = audios[0];
        movementYAudio = audios[1];
        AccerelationTunnelAudio = audios[4];
        limulemaxSpeedSound = audios[5];

        AccerelationTunnelAudio.Play();
        AccerelationTunnelAudio.Stop();


    }


    void Update()
    {
        if (SphereCanSpawn & !SpeedwasAt0)
        {
            MoveSphereBeginTunnel();
        }

        if (SpeedwasAt0)
        {
            if (Input.GetKeyUp("d"))
            {
                keywasreleased = true;
            }
        }
        if (SpeedwasAt0 & !falling)
        {
            MoveSphereSlidingTunnel();
        }

        if (falling & !LimuleCanMoveFreely)
        {
            MoveSphereFalling();
        }

        if (LimuleCanMoveFreely)
        {
            MoveSphere();
        }


        //MoveSphereOnlyD
        //deceleration

    }

    void MoveSphereBeginTunnel()
    {
        print("MoveSphereBeginTunnel" + Speed);
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
            //keywasreleased = true;
            Deceleration = 2f;
            //Deceleration = 0.1f;
            Acceleration = 0.1f;
            MaxSpeed = 0.6f;
            SpeedwasAt0 = true;
            //SphereCanSpawn = false;
        }

        if (Speed > Deceleration * Time.deltaTime) Speed -= Deceleration * Time.deltaTime;
        else if (Speed < -Deceleration * Time.deltaTime) Speed += Deceleration * Time.deltaTime;
        else
            Speed = 0;


        if (Speed2 > Deceleration * Time.deltaTime) Speed2 -= Deceleration * Time.deltaTime;
        else if (Speed2 < -Deceleration * Time.deltaTime) Speed2 += Deceleration * Time.deltaTime;
        else
            Speed2 = 0;

        Vector3 controlKeysMovement = new(Speed * Time.deltaTime * dashSpeed, Speed2 * Time.deltaTime * dashSpeed, 0f);
        m_Rigidbody.MovePosition(transform.position += controlKeysMovement);

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

    void MoveSphereSlidingTunnel()
    {


        print("MoveSphereSlidingTunnel" + Speed);
        if (Speed != 0 || Speed2 != 0)
        {
            emissionModule.enabled = true;
        }
        else
        {
            emissionModule.enabled = false;
        }


        if (Input.GetKey("d") & SpeedwasAt0 & !maxSpeedwasReached & keywasreleased)
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


        if (Speed2 > Deceleration * Time.deltaTime) Speed2 -= Deceleration * Time.deltaTime;
        else if (Speed2 < -Deceleration * Time.deltaTime) Speed2 += Deceleration * Time.deltaTime;
        else
            Speed2 = 0;


        Vector3 controlKeysMovement = new(Speed * Time.deltaTime * dashSpeed, Speed2 * Time.deltaTime * dashSpeed, 0f);
        m_Rigidbody.MovePosition(transform.position += controlKeysMovement);

        if (TouchedWall >= 1)
        {
            m_Rigidbody.velocity = Vector3.zero;
        }

        // Map the current speed to the volume of the audio
        float volume = Mathf.Lerp(minVolume, maxVolume, Mathf.InverseLerp(minSpeed, maxSpeed, Mathf.Abs(Speed)));
        float volume2 = Mathf.Lerp(minVolume, maxVolume, Mathf.InverseLerp(minSpeed, maxSpeed, Mathf.Abs(Speed2)));

        // Set the volume of the audio source
        movementXAudio.volume = volume;

        if (Speed == 0 & SpeedwasAt0)
        {

            AudioCanReplay = true;
            AccerelationTunnelAudio.pitch = 1f;
        }

        if (Speed > 0 & AudioCanReplay)
        {
            AccerelationTunnelAudio.Play();
            print("PROUUUUT");
            AudioCanReplay = false;
        }

        if (AccerelationTunnelAudio.pitch < 2.4f)
        {

            AccerelationTunnelAudio.pitch += pitchIncreaseRate * Time.deltaTime;
        }

        if (AccerelationTunnelAudio.pitch > 1.15f)
        {
            AccerelationTunnelAudio.volume += (pitchIncreaseRate / 2) * Time.deltaTime;
        }
        else
        {
            AccerelationTunnelAudio.volume = volume * 200;
        }

        if (Speed >= MaxSpeed)
        {
            Speed = MaxSpeed;
            Deceleration = 0;

            if (!maxSpeedwasReached)
            {
                animator.SetBool("PlayAnimShort", true);
                StartCoroutine(turnOFFLightIn(0.05f));
                limulemaxSpeedSound.Play();
            }
            maxSpeedwasReached = true;


        }

        //print(Speed);
        AccerelationTunnelAudio.volume += pitchIncreaseRate * Time.deltaTime;
        movementYAudio.volume = volume2;

        //Debug.Log("Current Speed: " + Speed);

    }

    void MoveSphereFalling()
    {
        print("MoveSphereFalling");
        emissionModule.enabled = true;
        Deceleration = 0.5f;
        Speed = 0;
        //Speed2 = -0.1f;
        AccerelationTunnelAudio.Stop();

        if (Speed > Deceleration * Time.deltaTime) Speed -= Deceleration * Time.deltaTime;
        else if (Speed < -Deceleration * Time.deltaTime) Speed += Deceleration * Time.deltaTime;
        else
            Speed = 0;

        //Speed2 = -0.2f;

        if (Speed2 > Deceleration * Time.deltaTime) Speed2 -= Deceleration * Time.deltaTime;
        else if (Speed2 < -Deceleration * Time.deltaTime) Speed2 += Deceleration * Time.deltaTime;
        else
            Speed2 = 0;


        Vector3 controlKeysMovement = new(Speed * Time.deltaTime * dashSpeed, Speed2 * Time.deltaTime * dashSpeed, 0f);
        m_Rigidbody.MovePosition(transform.position += controlKeysMovement);

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


    void MoveSphere()
    {
        if (!smoothTransitionPlayed)
        {
            m_Rigidbody.useGravity = false;
            Speed2 = -0.05f;
            smoothTransitionPlayed = true;
        }


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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("tunnel"))
        {
            TouchedWall++;
        }

        if (other.CompareTag("falling"))
        {
            //TouchedWall++;
            falling = true;
        }

        if (other.CompareTag("MoveSphere"))
        {
            //TouchedWall++;
            if (triggerColliderMoveSphereOnce)
            {
                limulemaxSpeedSound.Play();
                LimuleCanMoveFreely = true;
                MaxSpeed = 0.3f;
                Acceleration = 2f;
                Deceleration = 2f;
                triggerColliderMoveSphereOnce = false;
            }
        }

    }

    IEnumerator turnOFFLightIn(float seconds)
    {
        // wait for 1 second
        //Debug.Log("turnOFFLight in 1 sec");
        yield return new WaitForSeconds(seconds);
        animator.SetBool("PlayAnimShort", false);

        //Debug.Log("coroutine has stopped");
    }


}




