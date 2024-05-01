using UnityEngine;

public class InstantiatePrefabOnKeyPress : MonoBehaviour
{
    public GameObject prefab;
    public GameObject prefabSound;
    public float speed = 2f;
    public int spawnCount = 1;
    public float lifeDuration = 1.2f;
    public float minYOffset = 0.1f;
    public float maxYOffset = 0.2f;
    //AudioSource[] audioSources;
    void Start()
    {
        //audioSources = GetComponents<AudioSource>();

    }
    void Update()
    {
        // Check for key press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < spawnCount; i++)
            {
                // Calculate a random Y position within the specified range
                float randomYOffset = Random.Range(minYOffset, maxYOffset);
                Vector3 spawnPosition = transform.position + new Vector3(0f, randomYOffset, 0f);

                // Instantiate the prefab at the calculated position
                GameObject newObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
                GameObject newObjectSound = Instantiate(prefabSound, spawnPosition, Quaternion.identity);

                // Get the rigidbody component of the instantiated object
                Rigidbody rb = newObject.GetComponent<Rigidbody>();

                // Check if the Rigidbody component exists
                if (rb != null)
                {
                    // Set velocity to move in the positive x direction
                    rb.velocity = transform.right * speed;
                }

                // Destroy the prefab after a certain duration
                Destroy(newObject, lifeDuration);
            }


            /*if (audioSources != null && audioSources.Length > 0)
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
            }*/

        }
    }
}
