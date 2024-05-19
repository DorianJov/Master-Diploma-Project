using Unity.VisualScripting;
using UnityEngine;

public class floorbuttSpawner : MonoBehaviour
{
    public GameObject prefab; // The prefab to spawn
    public int totalPrefabs = 100; // Total number of prefabs to spawn
    public int prefabsPerRow = 14; // Maximum number of prefabs per row along the Z axis
    public float zIncrement = 0.7f; // Fixed increment for Z position
    public Vector2 xRange = new Vector2(1, 3); // Random range for X position increment
    public AudioClip[] audioClips; // Array to hold audio clips

    void Start()
    {
        if (audioClips.Length == 0)
        {
            Debug.LogError("Please assign at least one audio clip.");
            return;
        }
        else
        {
            AddAudioSources();
        }

        SpawnPrefabs();
    }

    void SpawnPrefabs()
    {
        Vector3 startPosition = transform.position;
        int rowCount = 0; // Track the number of rows
        int specialButtonCount = 0; // Track the number of special buttons spawned

        for (int i = 0; i < totalPrefabs; i++)
        {
            // Calculate the new position for the prefab
            float randomXIncrement = Random.Range(xRange.x, xRange.y) * rowCount;
            float newX = startPosition.x + randomXIncrement;
            float newZ = startPosition.z + (i % prefabsPerRow) * zIncrement;
            Vector3 newPosition = new Vector3(newX, startPosition.y, newZ);

            // Instantiate the prefab at the calculated position
            GameObject newPrefab = Instantiate(prefab, newPosition, Quaternion.identity, this.transform);


            if (i == 1)
            {
                newPrefab.tag = "specialPadButton";
            }
            if (i % prefabsPerRow - 1 == 0)
            {
                specialButtonCount++;

                if (specialButtonCount % 3 == 0)
                {
                    newPrefab.tag = "specialPadButton";
                }

            }





            // Increment rowCount when a row is filled
            if ((i + 1) % prefabsPerRow == 0)
            {
                rowCount++;
            }
        }
    }

    void AddAudioSources()
    {
        foreach (AudioClip clip in audioClips)
        {
            // Add an AudioSource component
            AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.playOnAwake = false; // Set the volume to 0.05
            audioSource.volume = 0.05f; // Set the volume to 0.05
        }
    }

    public void PlayRandomAudio()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>(); // Get all audio sources attached to the spawner
        if (audioSources.Length > 0)
        {
            int randomIndex = Random.Range(0, audioSources.Length); // Get a random index within the range of audio sources
            audioSources[randomIndex].Play(); // Play the audio source at the random index
        }
        else
        {
            Debug.LogWarning("No audio sources found on the floorbuttSpawner.");
        }
    }
}
