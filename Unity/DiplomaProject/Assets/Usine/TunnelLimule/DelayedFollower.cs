using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedFollower : MonoBehaviour
{
    public Transform target; // The target object to follow
    public float delay = 0.01f; // Delay time in seconds
    public float zOffset = 0.0f; // Z-axis offset for the follower
    public float smoothTransitionSpeed = 0.1f; // Speed at which to smoothly transition to the new position
    public float positionTolerance = 0.01f; // Tolerance for position comparison

    private Queue<Vector3> positionHistory; // Queue to store the target's position history with timestamps
    private Queue<float> timeHistory; // Queue to store the corresponding times

    private float currentDelay;

    public bool canChangeDelay = true;

    private bool tigeScene = true;

    bool ismoving = false;

    public ParticleSystem particleSystems1;
    public ParticleSystem particleSystems2;

    bool canMove = true;
    bool canPlayAudio = false;

    Rigidbody m_Rigidbody;
    private Vector3 previousPosition;

    bool FollowerIsDead = false;

    private AudioSource audioSourceMouvement;
    private AudioSource DyingSound01;
    private AudioSource DyingSound02;
    private AudioSource DyingSound03;

    private AudioSource impactSound;

    private AudioSource hitfloor;

    AudioSource[] audioSources;

    void Start()
    {
        positionHistory = new Queue<Vector3>();
        timeHistory = new Queue<float>();
        currentDelay = delay;
        smoothTransitionSpeed = 0.1f;
        m_Rigidbody = GetComponent<Rigidbody>();
        audioSources = GetComponents<AudioSource>();

        // Ensure at least one Particle System is found
        if (particleSystems1 == null & particleSystems2 == null)
        {
            Debug.LogError("No Particle Systems found on the GameObject.");
            return;
        }

        TurnOnParticleSystem(0);

        audioSourceMouvement = audioSources[0];
        previousPosition = transform.position;

    }

    void Update()
    {
        if (canMove)
        {
            // Add the target's current position to the queue with the Z offset applied
            Vector3 targetPositionWithOffset = target.position;
            targetPositionWithOffset.z += zOffset;
            positionHistory.Enqueue(targetPositionWithOffset);
            timeHistory.Enqueue(Time.time);

            // Smoothly update the current delay value
            currentDelay = Mathf.Lerp(currentDelay, delay, Time.deltaTime * smoothTransitionSpeed);

            // Remove positions from the queue if the delay time has passed
            while (timeHistory.Count > 0 && Time.time - timeHistory.Peek() > currentDelay)
            {
                positionHistory.Dequeue();
                timeHistory.Dequeue();
            }

            // Interpolate between the oldest and the next position in the queue
            if (positionHistory.Count > 1)
            {
                Vector3 startPosition = positionHistory.Peek();
                Vector3 endPosition = positionHistory.ToArray()[1];
                float startTime = timeHistory.Peek();
                float endTime = timeHistory.ToArray()[1];
                float t = (Time.time - startTime) / (endTime - startTime);

                transform.position = Vector3.Lerp(startPosition, endPosition, t);
            }
            else if (positionHistory.Count == 1)
            {
                transform.position = positionHistory.Peek();
            }

            // Calculate the velocity
            Vector3 currentPosition = transform.position;
            float velocity = (currentPosition - previousPosition).magnitude / Time.deltaTime;
            previousPosition = currentPosition;

            // Map the velocity to the audio volume (e.g., assuming a max velocity of 10 for full volume)
            if (canPlayAudio & !FollowerIsDead)
            {
                float maxVelocity = 10f;
                float targetVolume = velocity / maxVelocity * 0.025f; // Adjusted max volume to 0.01
                audioSourceMouvement.volume = Mathf.Clamp01(targetVolume);
            }
            else if (!canPlayAudio & !FollowerIsDead)
            {
                audioSourceMouvement.volume = 0;
            }
            else if (FollowerIsDead)
            {
                print("IM DEAD");
                audioSourceMouvement.Stop();
            }



            // Check if the follower is at the target position
            if (tigeScene)
            {
                //print("tigeScene IS TRUE");
                if (IsAtTargetPosition())
                {
                    this.gameObject.tag = "FollowerNoTrigger";
                    ismoving = false;
                    // Add logic to be executed when the follower is at the target position
                    //Debug.Log("Follower is at the target position");
                }
                else
                {
                    this.gameObject.tag = "Player";
                    //Debug.Log("ismoving");
                    ismoving = true;
                }

            }
            else
            {
                this.gameObject.tag = "Player";
            }
        }
        else
        {

        }

        //print("delayed follower state: " + "CanChangeDelay: " + canChangeDelay + "  " + "isMoveing: " + ismoving + "TigeScene: " + tigeScene);
    }


    private bool IsAtTargetPosition()
    {
        Vector3 targetPositionWithOffset = target.position;
        targetPositionWithOffset.z += zOffset;
        return Vector3.Distance(transform.position, targetPositionWithOffset) <= positionTolerance;
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "MoveSphere")
        {
            canPlayAudio = true;
        }

        if (canChangeDelay & ismoving)
        {
            if (other.CompareTag("tigeInsideUsine"))
            {
                tigeScene = true;
                //TouchedWall++;
                //print("Should change delay-Once");
                delay = Random.Range(0.1f, 10f);
                smoothTransitionSpeed = Random.Range(0.1f, 0.05f);
            }

        }

        if (other.CompareTag("secondfloorbutton"))
        {
            canPlayAudio = true;
            tigeScene = false;
            canChangeDelay = true;
        }

        if (other.CompareTag("Guetteur"))
        {
            if (!FollowerIsDead)
            {
                TurnOnParticleSystem(1);
                PlayRandomDyingSound();
                //Destroy(this.gameObject, 1f);
                StopLimule();
                StartCoroutine(waitForKill(1f));
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Guetteur"))
        {
            //TurnOFFParticleSystem(1);
            //StopCoroutine("waitForKill");

        }
    }
    void StopLimule()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            sphereCollider.isTrigger = false;
        }
        canMove = false;
        m_Rigidbody.useGravity = true;
    }
    IEnumerator waitForKill(float delay)
    {
        yield return new WaitForSeconds(delay);


        DeactivateLightAndChangeColor();
    }

    public void DeactivateLightAndChangeColor()
    {
        FollowerIsDead = true;
        // Find and deactivate the light component if it exists
        Light lightComponent = GetComponentInChildren<Light>();
        if (lightComponent != null)
        {
            lightComponent.enabled = false;
        }

        // Change the material color to black
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (renderer != null && renderer.material != null)
            {
                // Change the base color to black
                renderer.material.SetColor("_BaseColor", Color.black);

                // Change the emissive color to black
                renderer.material.SetColor("_EmissiveColor", Color.black);

                // Set the emissive intensity to 0
                renderer.material.SetFloat("_EmissiveIntensity", 0f);
            }
        }
        TurnOFFParticleSystem(1);
        //Destroy(gameObject, 5f);
    }

    public void KillMeNow()
    {
        Destroy(gameObject, 2f);
    }
    public void TurnOnParticleSystem(int index)
    {
        if (index == 1)
        {
            particleSystems1.Play();
        }
        else
        {
            particleSystems2.Play();
        }
    }

    public void TurnOFFParticleSystem(int index)
    {
        if (index == 1)
        {
            particleSystems1.Stop();
        }
        else
        {
            particleSystems2.Stop();
        }
    }

    public void PlayRandomDyingSound()
    {

        // Randomly select an index between 5 and 7
        int randomIndex = Random.Range(6, 12); // Range is inclusive for the lower bound and exclusive for the upper bound

        // Play the selected audio source
        if (audioSources[randomIndex] != null)
        {
            audioSources[randomIndex].Play();
        }
        else
        {
            Debug.LogWarning($"Audio source at index {randomIndex} is null.");
        }

    }
}
