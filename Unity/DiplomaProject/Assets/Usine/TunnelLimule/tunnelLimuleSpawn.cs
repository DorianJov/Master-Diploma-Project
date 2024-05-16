using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tunnelLimuleSpawn : MonoBehaviour
{
    private bool limuleSpawned = false;
    private Rigidbody rb;

    private MoveSphereTunnel moveSphereTunnel; // Reference to the other script

    private AudioSource tunnelCamSwitch;

    //detroy all previous gameobject.
    public GameObject Begin;
    public GameObject Intro;
    public GameObject UsineDeversing;

    // Start is called before the first frame update
    void Start()
    {
        // Cache the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Check if the Rigidbody component is found
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on the GameObject.");
        }

        // Cache the limuleMoveTunnel component
        moveSphereTunnel = GetComponent<MoveSphereTunnel>();

        // Check if the limuleMoveTunnel component is found
        if (moveSphereTunnel == null)
        {
            Debug.LogError("limuleMoveTunnel component not found on the GameObject.");
        }

        // Get the AudioSource component attached to the GameObject
        AudioSource[] audios = GetComponents<AudioSource>();
        tunnelCamSwitch = audios[3];


        // Check if the AudioSource component is found
        if (tunnelCamSwitch == null)
        {
            Debug.LogError("AudioSource component is missing from the GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (limuleSpawned && rb != null)
        {
            // Set velocity to move in the positive x direction
            float randomSpeed = 0.1f;
            rb.velocity = transform.right * randomSpeed;
            rb.useGravity = true;

            limuleSpawned = false;
            PlayAudio();
            Destroy(Intro, 1f);
            Destroy(Begin, 1f);
            Destroy(UsineDeversing, 6f);
            // Start a coroutine to unload unused assets after the delay
            StartCoroutine(UnloadAssetsCoroutine(6.1f));
        }
    }

    public void ActivateLimuleSpawn()
    {
        limuleSpawned = true;
        // Set canMove to true in the limuleMoveTunnel script
        if (moveSphereTunnel != null)
        {
            moveSphereTunnel.SphereCanSpawn = true;
        }

    }

    public void PlayAudio()
    {
        if (tunnelCamSwitch != null)
        {
            tunnelCamSwitch.Play();
        }
    }

    IEnumerator UnloadAssetsCoroutine(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Unload unused assets after the delay
        Resources.UnloadUnusedAssets();
    }
}
