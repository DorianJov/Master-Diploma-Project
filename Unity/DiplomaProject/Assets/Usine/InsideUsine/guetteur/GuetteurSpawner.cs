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

    public GameObject MainCamera;

    AudioSource[] sources;
    AudioSource Harmonic01;
    AudioSource Harmonic02;
    AudioSource Harmonic03;
    AudioSource Harmonic04;

    public float offseteffect = 1f;

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
            GameObject guetteurPrefab = Instantiate(prefabToSpawn, newPosition, rotation, this.transform);
            if (i != numberOfPrefabs - 1)
            {
                guetteurPrefab.GetComponent<GuetterGuetApen>().SetID(i, false); // Assign a unique ID
            }
            else
            {
                guetteurPrefab.GetComponent<GuetterGuetApen>().SetID(i, true); // Assign a unique ID
            }
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
    public void callTurnONEveryGuetteurRoutine(float seconds)
    {
        StartCoroutine(TurnONEveryGuetteurIN(seconds));
    }
    public IEnumerator TurnONEveryGuetteurIN(float seconds)
    {
        print("OpenEye In progress");
        yield return new WaitForSeconds(seconds);
        print("OpenEye Openened");
        TurnONEveryGuetteur();
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

    public void ResetAnimGuetteurToFalse()
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
                script.ResetAnimationVariables();
            }
            else
            {
                Debug.LogError("GuetterGuetApen script not found on the prefab.");
            }
        }
    }

    public void CameraShake()
    {

        if (MainCamera != null)
        {
            CameraFollow target = MainCamera.GetComponent<CameraFollow>();
            if (target != null)
            {
                //set target to limuleTunnel
                target.fovTransitionDuration = 2f;
                //target.fovTargetThree += 20f;
                target.smoothSpeed2 = 0.05f;
                target.offset = new Vector3(0.12f, 0.3f - offseteffect / 10, -1.37f - offseteffect / 10);
                StartCoroutine(restoreOffset(0.1f));
            }
            else
            {
                Debug.LogError("CameraFollow component not found on pinceObject.");
            }
        }
        else
        {
            Debug.LogError("MainCamera is null.");
        }

    }

    IEnumerator restoreOffset(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        CameraFollow target = MainCamera.GetComponent<CameraFollow>();
        target.smoothSpeed2 = 0.2f;
        target.offset = new Vector3(0.12f, 0.3f, -1.37f);
        //StartCoroutine(testfalling(0.2f));
        //target.offset = new Vector3(0, 0.1f, -1.37f);


    }

    public void LightUpSequence()
    {
        StartCoroutine(LightUpPrefabsSequentially());
    }

    private IEnumerator LightUpPrefabsSequentially()
    {
        for (int i = spawnedGuetteur.Count - 1; i >= 0; i--)
        {
            GuetterGuetApen script = spawnedGuetteur[i].GetComponent<GuetterGuetApen>();
            if (script != null)
            {
                yield return new WaitForSeconds(0.1f);
                //print($"Wait time for prefab {i}: {(spawnedGuetteur.Count - i) * 0.05f}");
                //script.Play_BlinkRed_Sound();
                script.TurnONOpenEye();
            }
        }
        print("coroutine ended: AH");
    }

    public void CallAllBlinkRed()
    {
        CallAllBlinkRedPrefabs();
    }

    private void CallAllBlinkRedPrefabs()
    {
        for (int i = spawnedGuetteur.Count - 1; i >= 0; i--)
        {
            GuetterGuetApen script = spawnedGuetteur[i].GetComponent<GuetterGuetApen>();
            if (script != null)
            {
                script.ActivatBlinkRed();
            }
        }
    }


    public void CallKillMode()
    {
        StartCoroutine(ActivateKillModeSequentially());
    }

    private IEnumerator ActivateKillModeSequentially()
    {
        for (int i = spawnedGuetteur.Count - 1; i >= 0; i--)
        {
            GuetterGuetApen script = spawnedGuetteur[i].GetComponent<GuetterGuetApen>();
            if (script != null)
            {
                yield return new WaitForSeconds(2.5f);
                script.LetsGoKillMode();
            }
        }
        print("coroutine ended: AH");
    }






}
