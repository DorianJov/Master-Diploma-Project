using UnityEngine;
using System.Collections.Generic;

public class InstantiatePrefabOnKeyPress : MonoBehaviour
{
    public GameObject prefab;
    //public GameObject prefabSound;
    public float Minspeed = 5f;
    public float Maxspeed = 10f;
    public int spawnCount = 1;
    public float lifeDuration = 1.2f;
    public float minYOffset = 0.1f;
    public float maxYOffset = 0.2f;

    public bool spawnerIsActive = false;

    ParticleSystem myParticleSystem;

    public int limuleCounter = 0;


    public AudioSource[] audioSources;

    private List<int> shuffledIndexes = new List<int>();
    private int currentIndex = 0;

    [Header("Limules Counter")]
    public GameObject prefabToSpawn;
    public Transform spawnPoint;
    public float circleRadius = 0.01f;
    //public int numberOfPrefabs = 12; // Number of prefabs to spawn
    public float angleIncrement = 10f; // Angle increment between prefabs
    public int circleOffset = 0;

    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
        // Shuffle the indexes initially
        ShuffleIndexes();

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
                LimuleCounter(i);

                myParticleSystem.Play();
                Destroy(newObject, lifeDuration);
            }


            // Check if all sounds have been played
            if (currentIndex >= audioSources.Length)
            {
                // Reshuffle the indexes
                ShuffleIndexes();
                // Reset currentIndex to start playing sounds from the beginning
                currentIndex = 0;
            }

            // Play the sound at the current index
            if (audioSources != null && audioSources.Length > 0)
            {
                int indexToPlay = shuffledIndexes[currentIndex];
                AudioSource selectedAudioSource = audioSources[indexToPlay];

                if (selectedAudioSource != null)
                {
                    selectedAudioSource.Play();
                    Debug.Log("Playing sound: " + selectedAudioSource.clip.name); // Print the name of the played audio clip
                    currentIndex++; // Move to the next index for the next frame
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

    private void ShuffleIndexes()
    {
        // Populate shuffledIndexes with numbers from 0 to audioSources.Length - 1
        shuffledIndexes.Clear();
        for (int i = 0; i < audioSources.Length; i++)
        {
            shuffledIndexes.Add(i);
        }

        // Shuffle the indexes
        for (int i = 0; i < shuffledIndexes.Count; i++)
        {
            int temp = shuffledIndexes[i];
            int randomIndex = Random.Range(i, shuffledIndexes.Count);
            shuffledIndexes[i] = shuffledIndexes[randomIndex];
            shuffledIndexes[randomIndex] = temp;
        }
    }

    public void TriggerSpawner()
    {
        // Check if pinceObject is not null and has the pinceScript component
        spawnerIsActive = true;
    }

    private void LimuleCounter(int i)
    {


        float angle = limuleCounter * angleIncrement;

        // Calculate the position of the prefab around the circle using trigonometry
        float spawnX = spawnPoint.position.x + circleRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float spawnY = spawnPoint.position.y + circleRadius * Mathf.Sin(angle * Mathf.Deg2Rad);

        // Create a rotation based on the current angle
        Quaternion rotation = Quaternion.Euler(0f, 0f, -angle);

        // Spawn the prefab at the calculated position and rotation
        Instantiate(prefabToSpawn, new Vector3(spawnX, spawnY, spawnPoint.position.z), rotation);

        limuleCounter++;


    }
}
