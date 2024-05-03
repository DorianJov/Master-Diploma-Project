using UnityEngine;

public class InstantiatePrefabOnKeyPress : MonoBehaviour
{
    public GameObject prefab;
    public GameObject prefabSound;
    public float Minspeed = 5f;
    public float Maxspeed = 10f;
    public int spawnCount = 1;
    public float lifeDuration = 1.2f;
    public float minYOffset = 0.1f;
    public float maxYOffset = 0.2f;

    public bool spawnerIsActive = false;

    ParticleSystem myParticleSystem;


    AudioSource[] audioSources;
    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        myParticleSystem = GetComponentInChildren<ParticleSystem>();

    }
    void Update()
    {
        // Check for key press
        if (Input.GetKeyDown(KeyCode.D) && spawnerIsActive)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                // Calculate a random Y position within the specified range
                float randomYOffset = Random.Range(minYOffset, maxYOffset);
                Vector3 spawnPosition = transform.position + new Vector3(0f, randomYOffset, 0f);

                // Instantiate the prefab at the calculated position
                GameObject newObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
                //GameObject newObjectSound = Instantiate(prefabSound, spawnPosition, Quaternion.identity);

                // Get the rigidbody component of the instantiated object
                Rigidbody rb = newObject.GetComponent<Rigidbody>();

                // Check if the Rigidbody component exists
                if (rb != null)
                {
                    // Set velocity to move in the positive x direction
                    float randomSpeed = Random.Range(Minspeed, Maxspeed);
                    rb.velocity = transform.right * randomSpeed;
                }

                // Destroy the prefab after a certain duration
                myParticleSystem.Play();
                Destroy(newObject, lifeDuration);
            }


            if (audioSources != null && audioSources.Length > 0)
            {
                int randomIndex = Random.Range(0, audioSources.Length);
                AudioSource selectedAudioSource = audioSources[randomIndex];

                if (selectedAudioSource != null)
                {
                    selectedAudioSource.Play();
                }
                else
                {
                    Debug.LogError("One of the AudioSource components in the array is null.");
                }
            }
            else
            {
                Debug.LogError("No AudioSource components assigned to the array.");
            }

        }
    }

    public void TriggerSpawner()
    {
        // Check if pinceObject is not null and has the pinceScript component
        spawnerIsActive = true;
    }
}
