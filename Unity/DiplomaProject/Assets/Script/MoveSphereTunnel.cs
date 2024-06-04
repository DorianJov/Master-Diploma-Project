using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MoveSphereTunnel : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator animator;
    public float Speed = 0.0f; //Don’t touch this
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

    private tunnelLimuleSpawn tunnelLimuleSpawnScript;

    public ParticlesInsideUsine particlesInsideUsine;

    private GlowingAttack glowingAttack;
    public ScreenFader screenFader;



    public ParticleSystem myParticleSystem;
    ParticleSystem.EmissionModule emissionModule;
    ParticleSystem.EmissionModule emissiondeathParticles;
    public ParticleSystem deathParticles;

    [Header("Audio Parameters")]
    // Define minimum and maximum speeds for the audio volume mapping
    AudioSource movementXAudio;
    AudioSource movementYAudio;

    AudioSource AccerelationTunnelAudio;

    AudioSource limulemaxSpeedSound;

    [Header("Speed")]
    public float minSpeed = 0f;
    public float maxSpeed = 2f;
    public float minVolume = 0f; // Minimum volume when speed is 0
    public float maxVolume = 1f; // Maximum volume when speed is at MaxSpeed

    [Header("States")]
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

    bool canJump = false;

    float lastSpeed = 0f;

    bool limuleIsJumpingPhase = false;

    private float dKeyPressDuration;
    private float holdThreshold = 0.05f;


    [Header("Followers")]
    public GameObject limuleFollowerPrefab; // The prefab to instantiate
    public Transform defaultTarget; // The default target for all followers
    public int numberOfFollowers = 10; // Number of followers to instantiate
    public float delayIncrement = 0.45f; // Increment of delay for each follower
    public float zOffsetIncrement = 0.7f; // Increment for the Z offset of each follower
    private List<GameObject> followers = new List<GameObject>();

    [Header("Teleport Inside Usine")]
    public bool dev = false;

    bool calledOnceRandomDelayLimule = false;
    AudioSource[] audios;

    bool PlayerWillDie = false;

    bool LimuleFunctionCanPlay = false;

    private Renderer playerRenderer;
    private Light playerLight;

    private Color originalBaseColor;
    private Color originalEmissiveColor;
    private float originalEmissiveIntensity;

    bool ToTheSkyPhase = false;


    void Start()
    {

        m_Rigidbody = GetComponent<Rigidbody>();

        tunnelLimuleSpawnScript = GetComponent<tunnelLimuleSpawn>();
        if (tunnelLimuleSpawnScript == null)
        {
            Debug.LogError("tunnelLimuleSpawn script not found on the same GameObject.");
        }

        glowingAttack = GetComponent<GlowingAttack>();
        // Get the system and the emission module.
        //myParticleSystem = GetComponentInChildren<ParticleSystem>();
        emissionModule = myParticleSystem.emission;
        emissiondeathParticles = deathParticles.emission;
        //var emission = ps.emission;
        animator = GetComponentInChildren<Animator>();

        //myParticleSystem2 = GetComponent<ParticleSystem>();
        audios = GetComponents<AudioSource>();
        movementXAudio = audios[0];
        movementYAudio = audios[1];
        AccerelationTunnelAudio = audios[4];
        limulemaxSpeedSound = audios[5];

        AccerelationTunnelAudio.Play();
        AccerelationTunnelAudio.Stop();

        SpawnFollowers();

        playerRenderer = GetComponent<Renderer>();
        playerLight = GetComponent<Light>();

        if (playerRenderer != null)
        {
            // Save the original material properties
            originalBaseColor = playerRenderer.material.GetColor("_BaseColor");
            originalEmissiveColor = playerRenderer.material.GetColor("_EmissiveColor");
            originalEmissiveIntensity = playerRenderer.material.GetFloat("_EmissiveIntensity");
        }


    }


    void Update()
    {


        if (Input.GetKeyUp("t"))
        {
            dev = true;
            m_Rigidbody.useGravity = false;
            tunnelLimuleSpawnScript.DestroyGameObjects();

            this.transform.position = new Vector3(-17.40f, -4.86f, 0.996f);
        }
        else
        {

        }

        if (dev)
        {
            if (Input.GetKeyUp("g"))
            {
                this.transform.position = new Vector3(0.29f, -4.77f, 0.996f);
            }

            if (Input.GetKeyUp("j"))
            {
                this.transform.position = new Vector3(10f, -4.77f, 0.996f);
            }
        }

        if (!dev)
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

        }

        if (LimuleCanMoveFreely & !limuleIsJumpingPhase || dev & !limuleIsJumpingPhase)
        {
            MoveSphere();
        }

        if (limuleIsJumpingPhase & !ToTheSkyPhase)
        {
            if (!calledOnceRandomDelayLimule)
            {
                UpdateDelayForGuetteurScene(0.1f, 5f);
                calledOnceRandomDelayLimule = true;
            }

            LimuleJumping();

        }
        if (ToTheSkyPhase)
        {
            ToTheSkyMove();

        }
        //print("acceleration: " + Acceleration);
        //print("Decerelation: " + Deceleration);
        //MoveSphereOnlyD
        //deceleration

    }

    void MoveSphereBeginTunnel()
    {
        //print("MoveSphereBeginTunnel" + Speed);
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


        //print("MoveSphereSlidingTunnel" + Speed);
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
            //print("PROUUUUT");
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
        //print("MoveSphereFalling");
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
        //print("MoveSphere");
        //dev power
        if (Input.GetMouseButton(0))
        {
            // Your code here to handle the left mouse button being held down
            MaxSpeed = 2f;
            Debug.Log("Left mouse button is being held down.");
        }
        else
        {
            MaxSpeed = 0.3f;
        }

        Acceleration = 2f;
        Deceleration = 2f;

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

        //disabled dash

        /*if (Input.GetKeyDown(KeyCode.Mouse0) & dashSpeed == 1)
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
        */


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
        lastSpeed = Speed;
    }

    void ToTheSkyMove()
    {
        MaxSpeed = 0.3f;
        if (Input.GetKey("w"))
        {
            //Deceleration = 0f;
            //Deceleration = 2f;
            Speed2 = 2f;
        }

        if (Input.GetKey("s"))
        {
            //Deceleration = 2f;
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


        if (Speed2 > Deceleration * Time.deltaTime) Speed2 -= Deceleration * Time.deltaTime;
        else if (Speed2 < -Deceleration * Time.deltaTime) Speed2 += Deceleration * Time.deltaTime;
        else
            Speed2 = 0;

        Vector3 controlKeysMovement = new(Speed * Time.deltaTime * dashSpeed, Speed2 * Time.deltaTime * dashSpeed, 0f);
        m_Rigidbody.MovePosition(transform.position += controlKeysMovement);


        m_Rigidbody.MovePosition(transform.position += transform.right * Mathf.Sin(speedUpDown * Time.time) * Time.deltaTime * distanceUpDown);
        m_Rigidbody.MovePosition(transform.position += transform.up * Mathf.Sin(speedUpDownUP * Time.time) * Time.deltaTime * distanceUpDownUP);

        m_Rigidbody.velocity = Vector3.zero;

        float volume = Mathf.Lerp(minVolume, maxVolume, Mathf.InverseLerp(minSpeed, maxSpeed, Mathf.Abs(Speed)));
        float volume2 = Mathf.Lerp(minVolume, maxVolume, Mathf.InverseLerp(minSpeed, maxSpeed, Mathf.Abs(Speed2)));

        movementXAudio.volume = volume;
        movementYAudio.volume = volume2;

        lastSpeed = Speed;
    }



    void UpdateKeyPressDuration(string key)
    {
        if (Input.GetKey(key))
        {
            dKeyPressDuration += Time.deltaTime;
        }
        else
        {
            dKeyPressDuration = 0f;
        }
    }

    bool IsKeyHeld(string key)
    {
        return dKeyPressDuration >= holdThreshold;
    }

    void LimuleJumping()
    {
        //print("LIMULEJUMNPING");
        UpdateKeyPressDuration("d");
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {

        }

        //dev power
        if (Input.GetMouseButton(0))
        {
            // Your code here to handle the left mouse button being held down
            MaxSpeed = 2f;
            Debug.Log("Left mouse button is being held down.");
        }
        else
        {
            //MaxSpeed = 0.3f;
        }

        Acceleration = 10f;
        if (LimuleFunctionCanPlay)
        {
            Deceleration = 5f;
        }

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



        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            canJump = true;
        }

        if (Input.GetKey("d") & canJump & !PlayerWillDie! & !IsKeyHeld("d") & LimuleFunctionCanPlay)
        {
            if (Speed <= 0.005f)
            {
                //Speed = 0.8f;
                //Speed2 = 0.7f;
                //canJump = false;

                Speed = 1.1f;
                Speed2 = 0.7f;
                canJump = false;
            }
            else
            {
                Deceleration = 50f;
                Speed2 = 0;

            }

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
                limulemaxSpeedSound.pitch = 3;
                limulemaxSpeedSound.volume = 0.01f;
                //LimuleCanMoveFreely = true;

                MaxSpeed = 0.3f;
                Acceleration = 2f;
                Deceleration = 2f;
                StartCoroutine(LaunchIncrementationIN((numberOfFollowers - 1) * delayIncrement + 0.5f, 0.1f));
                StartCoroutine(playHitfloorAgainIn(numberOfFollowers * delayIncrement + 0.1f));



                //(numberOfFollowers - i) * delayIncrement + 0.1f - (i * 0.1f)
                //animator.SetBool("PlayAnim", true);
                //StartCoroutine(turnOFFLightIn(0.05f));
                triggerColliderMoveSphereOnce = false;
            }
        }

        if (other.CompareTag("tigeInsideUsine"))
        {
            //TouchedWall++;

            UpdateDelayIncrementRandom(0.1f, 10f);
        }

        if (other.CompareTag("specialPadButton"))
        {
            //TouchedWall++;

            ResetDelayIncrement(0.1f);
            //ResetDelay(0.01f);

        }

        if (other.CompareTag("Guetteur") & !PlayerWillDie)
        {
            TurnOnDeathParticles();
            PlayRandomDyingSound();
            PlayerWillDie = true;
            animator.SetBool("PlayAnimShort", true);
            StartCoroutine(turnOFFLightIn(0.05f));
            StartCoroutine(WaitBeforeRespawn(5f));
            FadeScreenToBlack();

        }

        if (other.CompareTag("ToTheSkyButton"))
        {
            //glowingAttack.CameraToTheSkyPhase();
            ToTheSkyPhase = true;
            Speed = 0f;
            Deceleration = 0;
            Speed2 = 1.5f;

        }


    }

    public void DeactivateLightAndChangeToBlack()
    {
        if (playerRenderer != null)
        {
            // Change the material properties to black
            playerRenderer.material.SetColor("_BaseColor", Color.black);
            playerRenderer.material.SetColor("_EmissiveColor", Color.black);
            playerRenderer.material.SetFloat("_EmissiveIntensity", 0f);
        }

        if (playerLight != null)
        {
            // Deactivate the light component
            playerLight.enabled = false;
        }
    }

    public void ReactivateLightAndRestoreMaterial()
    {
        if (playerRenderer != null)
        {
            // Restore the original material properties
            playerRenderer.material.SetColor("_BaseColor", originalBaseColor);
            playerRenderer.material.SetColor("_EmissiveColor", originalEmissiveColor);
            playerRenderer.material.SetFloat("_EmissiveIntensity", originalEmissiveIntensity);
        }

        if (playerLight != null)
        {
            // Reactivate the light component
            playerLight.enabled = true;
        }
    }


    IEnumerator WaitBeforeRespawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        KillAllFollowersForRespawn();
    }

    void FadeScreenToBlack()
    {
        // Start fading to black
        screenFader.FadeOut();

        // Wait for the fade duration and respawn the player
        StartCoroutine(UnFadeScreen());
    }

    IEnumerator UnFadeScreen()
    {
        yield return new WaitForSeconds(6f);

        // Respawn logic here

        // Start fading to clear
        screenFader.FadeIn();
    }

    public void LimuleIsFalling()
    {
        //dev = false;
        //falling = true;
        //LimuleCanMoveFreely = false;
        //
        //ResetDelay(0.01f);
        limuleIsJumpingPhase = true;
        Deceleration = 3f;
        StartCoroutine(TurnOnLimuleFunctionCanPlay(2f));
        m_Rigidbody.useGravity = true;
        particlesInsideUsine.SetVelocityOverLifetimeY(-3.0f); // Example to set Y velocity to 5.0
        particlesInsideUsine.DeactivateEmission(); // Deactivate emission
        Speed = lastSpeed * 2;
        Speed2 = -2f;


    }

    IEnumerator TurnOnLimuleFunctionCanPlay(float delay)
    {
        yield return new WaitForSeconds(delay);
        LimuleFunctionCanPlay = true;
    }

    IEnumerator turnOFFLightIn(float seconds)
    {
        // wait for 1 second
        //Debug.Log("turnOFFLight in 1 sec");
        yield return new WaitForSeconds(seconds);
        animator.SetBool("PlayAnimShort", false);
        animator.SetBool("PlayAnim", false);

        //Debug.Log("coroutine has stopped");
    }

    void SpawnFollowers()
    {
        for (int i = 0; i < numberOfFollowers; i++)
        {
            // Instantiate the prefab
            GameObject follower = Instantiate(limuleFollowerPrefab, transform.position, transform.rotation);

            // Create a new material instance for each renderer in the follower
            Renderer[] renderers = follower.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderers)
            {
                rend.material = new Material(rend.material);
            }

            // Set the delay for the DelayedFollower script
            DelayedFollower delayedFollower = follower.GetComponent<DelayedFollower>();
            if (delayedFollower != null)
            {
                delayedFollower.delay = i * delayIncrement;
                // Adjust zOffset based on the index
                if (i == 0)
                {
                    delayedFollower.zOffset = -0.7f;
                }
                else if (i == 1)
                {
                    delayedFollower.zOffset = 0.7f;
                }
                else
                {
                    delayedFollower.zOffset = i * zOffsetIncrement;
                }

                delayedFollower.target = defaultTarget; // Set the default target
            }

            GlowingFollower glowingFollower = follower.GetComponent<GlowingFollower>();

            if (glowingFollower != null)
            {
                // Last follower should have the first delay increment value which is 0.1
                float initialTimeToPlaySound = (numberOfFollowers - i) * delayIncrement + 0.1f - (i * 0.05f);

                glowingFollower.TimeToPlaySoundAgain = initialTimeToPlaySound;

                // Deactivate the light component and "make me glow" script after the 12th follower
                if (i >= 12)
                {
                    Light lightComponent = follower.GetComponentInChildren<Light>();
                    if (lightComponent != null)
                    {
                        lightComponent.enabled = false;
                    }

                    glowingFollower.FollowerWithLightComponent = false;
                }
            }

            // Add the follower to the list
            followers.Add(follower);
        }
    }




    IEnumerator LaunchIncrementationIN(float seconds, float incrementation)
    {
        // wait for 1 second

        //Debug.Log("turnOFFLight in 1 sec");
        yield return new WaitForSeconds(seconds);
        UpdateDelayIncrement(incrementation);


        //Debug.Log("delay applystopped");
    }

    // Function to update the delay increment
    public void UpdateDelayIncrement(float newDelayIncrement)
    {
        delayIncrement = newDelayIncrement;

        // Update the delay for each follower
        for (int i = 0; i < followers.Count; i++)
        {
            DelayedFollower delayedFollower = followers[i].GetComponent<DelayedFollower>();
            if (delayedFollower != null)
            {

                delayedFollower.canChangeDelay = true;
                delayedFollower.delay = (i + 1) * delayIncrement;
                delayedFollower.smoothTransitionSpeed = 5f;
            }
        }
    }


    public void ResetDelayIncrement(float newDelayIncrement)
    {
        delayIncrement = newDelayIncrement;

        // Update the delay for each follower
        for (int i = 0; i < followers.Count; i++)
        {
            DelayedFollower delayedFollower = followers[i].GetComponent<DelayedFollower>();
            if (delayedFollower != null)
            {
                delayedFollower.canChangeDelay = false;
                delayedFollower.delay = (i + 1) * delayIncrement;
                delayedFollower.smoothTransitionSpeed = 5f;
            }
        }

        Play_My_Sync_Sound();
        StartCoroutine(PlayDelayIncrementSound(newDelayIncrement));
    }

    public void ResetDelay(float newDelayIncrement)
    {
        delayIncrement = newDelayIncrement;

        // Update the delay for each follower
        for (int i = 0; i < followers.Count; i++)
        {
            DelayedFollower delayedFollower = followers[i].GetComponent<DelayedFollower>();
            if (delayedFollower != null)
            {
                delayedFollower.canChangeDelay = false;
                delayedFollower.delay = delayIncrement;
                delayedFollower.smoothTransitionSpeed = 5f;
            }
        }

        Play_My_Sync_Sound();
        StartCoroutine(PlayDelayIncrementSound(newDelayIncrement));
    }

    IEnumerator PlayDelayIncrementSound(float newDelayIncrement)
    {


        // Update the delay for each follower
        for (int i = 0; i < followers.Count; i++)
        {
            GlowingFollower glowingFollower = followers[i].GetComponent<GlowingFollower>();
            if (glowingFollower != null)
            {
                yield return new WaitForSeconds(newDelayIncrement);
                glowingFollower.Play_Sync_Sound();

            }
        }
    }

    public void UpdateDelayIncrementRandom(float min, float max)
    {
        // Update the delay for each follower with a random value within the range
        for (int i = 0; i < followers.Count; i++)
        {
            DelayedFollower delayedFollower = followers[i].GetComponent<DelayedFollower>();
            if (delayedFollower != null)
            {
                delayedFollower.canChangeDelay = true;
                delayedFollower.delay = Random.Range(min, max);
                delayedFollower.smoothTransitionSpeed = 0.1f;
            }
        }
    }

    public void UpdateDelayForGuetteurScene(float min, float max)
    {
        // Update the delay for each follower with a random value within the range
        for (int i = 0; i < followers.Count; i++)
        {
            DelayedFollower delayedFollower = followers[i].GetComponent<DelayedFollower>();
            if (delayedFollower != null)
            {
                if (i == 0)
                {
                    delayedFollower.delay = 2f;
                    delayedFollower.smoothTransitionSpeed = 0.1f;
                }
                else
                {
                    delayedFollower.delay = Random.Range(min, max);
                    delayedFollower.smoothTransitionSpeed = 0.1f;
                }
            }
        }
    }

    public void KillAllFollowersForRespawn()
    {
        // Update the delay for each follower with a random value within the range
        for (int i = 0; i < followers.Count; i++)
        {
            DelayedFollower delayedFollower = followers[i].GetComponent<DelayedFollower>();
            if (delayedFollower != null)
            {
                delayedFollower.KillMeNow();
            }
        }
        transform.position = new Vector3(1.7534f, -4.768f, 0.996f);


        //deathParticles.Stop();
        emissiondeathParticles.enabled = false;
        emissionModule.enabled = true;
        m_Rigidbody.useGravity = false;
        limuleIsJumpingPhase = false;
        LimuleFunctionCanPlay = false;
        canJump = false;
        calledOnceRandomDelayLimule = false;
        particlesInsideUsine.SetVelocityOverLifetimeY(0); // Example to set Y velocity to 5.0
        particlesInsideUsine.ActivateEmission();
        ResetCamera();
        glowingAttack.CallKillAllGuetteurAndReset();
        glowingAttack.CallResetFloorPadButts();
        glowingAttack.calledGuetteurOnce = false;
        PlayerWillDie = false;
        // Clear the list
        followers.Clear();
        ReactivateLightAndRestoreMaterial();
        SpawnFollowers();
        UpdateDelayIncrementRandom(0.1f, 10f);

    }

    void ResetCamera()
    {
        glowingAttack.ResetCamera();

    }

    IEnumerator playHitfloorAgainIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        limulemaxSpeedSound.Play();
        //GlowingAttack glowingAttack = GetComponent<GlowingAttack>();
        //glowingAttack.fallingSound.Stop();
        LimuleCanMoveFreely = true;
        animator.SetBool("PlayAnimShort", true);
        StartCoroutine(turnOFFLightIn(0.05f));

    }

    public void Play_My_Sync_Sound()
    {
        limulemaxSpeedSound.Play();
        animator.SetBool("PlayAnimShort", true);
        StartCoroutine(turnOFFLightIn(0.05f));
    }

    public void TurnOnDeathParticles()
    {
        deathParticles.Play();
        emissiondeathParticles.enabled = true;
        StartCoroutine(StopDeathParticles(1f));
    }

    IEnumerator StopDeathParticles(float delay)
    {
        yield return new WaitForSeconds(delay);
        emissiondeathParticles.enabled = false;
        DeactivateLightAndChangeToBlack();
    }


    public void PlayRandomDyingSound()
    {

        // Randomly select an index between 10 and 12
        int randomIndex = Random.Range(10, 13);

        // Play the selected audio source if it exists
        if (randomIndex >= 0 && randomIndex < audios.Length && audios[randomIndex] != null)
        {
            audios[randomIndex].Play();
        }
        else
        {
            Debug.LogWarning($"Audio source at index {randomIndex} is null or out of range.");
        }

    }
}






