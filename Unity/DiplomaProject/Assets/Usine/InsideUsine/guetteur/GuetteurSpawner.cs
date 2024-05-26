using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuetteurSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // Prefab to be spawned
    public int numberOfPrefabs = 10; // Number of prefabs to spawn
    public float xIncrement = 1.0f; // The X increment between each spawned prefab

    private List<GameObject> spawnedGuetteur = new List<GameObject>(); // List to store references to spawned prefabs
    private GameObject chosenPrefab; // Reference to the chosen prefab

    AudioSource[] sources;
    AudioSource Harmonic01;
    AudioSource Harmonic02;
    AudioSource Harmonic03;
    AudioSource Harmonic04;

    // Start is called before the first frame update
    void Start()
    {
        sources = GetComponents<AudioSource>();
        Harmonic01 = sources[0];
        Harmonic02 = sources[1];
        Harmonic03 = sources[2];
        Harmonic04 = sources[3];

        SpawnPrefabs();
        ChooseRandomPrefab();
    }

    void SpawnPrefabs()
    {
        Vector3 startPosition = transform.position;
        Quaternion rotation = Quaternion.Euler(0, 180, 0); // 180-degree rotation around Y-axis

        for (int i = 0; i < numberOfPrefabs; i++)
        {
            // Calculate the new position for the prefab
            float newX = startPosition.x + (i * xIncrement);
            Vector3 newPosition = new Vector3(newX, startPosition.y, startPosition.z);

            // Instantiate the prefab at the calculated position with the specified rotation
            GameObject guetteurPrefab = Instantiate(prefabToSpawn, newPosition, rotation);
            guetteurPrefab.GetComponent<GuetterGuetApen>().SetID(i); // Assign a unique ID
            spawnedGuetteur.Add(guetteurPrefab); // Store reference to the spawned prefab
        }
    }

    void ChooseRandomPrefab()
    {
        if (spawnedGuetteur.Count == 0)
        {
            Debug.LogError("No prefabs have been spawned.");
            return;
        }

        // Select a random prefab from the list
        int randomIndex = Random.Range(0, spawnedGuetteur.Count);
        chosenPrefab = spawnedGuetteur[randomIndex];
        chosenPrefab.GetComponent<GuetterGuetApen>().SetChosen(true); // Mark it as chosen
    }

    public void LaunchRandomHarmonic(float timeTolaunch)
    {
        if (spawnedGuetteur.Count == 0)
        {
            Debug.LogError("No prefabs have been spawned.");
            return;
        }

        // Select a random prefab from the list
        int randomIndex = Random.Range(0, spawnedGuetteur.Count);
        GameObject randomGuetteurPrefab = spawnedGuetteur[randomIndex];

        // Get the GuetterGuetApen script and call the lauchAnim function
        GuetterGuetApen script = randomGuetteurPrefab.GetComponent<GuetterGuetApen>();
        if (script != null)
        {
            print("Harmonic will Launch From Spawner");
            script.LaunchHarmonic(timeTolaunch);
        }
        else
        {
            Debug.LogError("GuetterGuetApen script not found on the selected prefab.");
        }
    }

    public void LaunchHarmonicForAll(float timeToLaunch)
    {
        if (spawnedGuetteur.Count == 0)
        {
            Debug.LogError("No prefabs have been spawned.");
            return;
        }

        foreach (GameObject guetteurPrefab in spawnedGuetteur)
        {
            GuetterGuetApen script = guetteurPrefab.GetComponent<GuetterGuetApen>();
            if (script != null)
            {
                print("Harmonic will Launch From Spawner");
                script.LaunchHarmonic(timeToLaunch);
            }
            else
            {
                Debug.LogError("GuetterGuetApen script not found on the prefab.");
            }
        }
    }

    public void TurnONEveryGuetteur()
    {
        if (spawnedGuetteur.Count == 0)
        {
            Debug.LogError("No prefabs have been spawned.");
            return;
        }

        foreach (GameObject guetteurPrefab in spawnedGuetteur)
        {
            GuetterGuetApen script = guetteurPrefab.GetComponent<GuetterGuetApen>();
            if (script != null)
            {
                print("Harmonic will Launch From Spawner");
                script.TurnONOpenEye();
            }
            else
            {
                Debug.LogError("GuetterGuetApen script not found on the prefab.");
            }
        }
    }
}
