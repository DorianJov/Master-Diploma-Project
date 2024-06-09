using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CounterToTheSky : MonoBehaviour
{
    [Header("Camera")]
    public GameObject MainCamera;

    [Header("Player")]
    public GameObject Player;
    ParticleSystem myParticleSystem;
    private int limuleCounter = 0;
    private AudioSource[] audioSources;

    [Header("Limules Counter")]
    public GameObject prefabToSpawn;
    public GameObject spawnPointObject;
    private Transform spawnPoint;
    public int prefabsSpawns = 1;
    public float angleIncrement = 10f; // Angle increment between prefabs
    public float circleRadiusX = 0f;
    public float circleRadiusY = 0f;
    public float circleRadiusZ = 0f;
    public float rotationAngleX = 0f;
    public float rotationAngleY = 0f;
    public float rotationAngleZ = 0f;
    public int singleCountRot = 0;
    bool gospawn = false;
    bool ChangeOffsetOnce = true;

    private Animator animator;
    private float offseteffect = 1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        spawnPoint = spawnPointObject.transform;
        audioSources = GetComponents<AudioSource>();
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            audioSources[1].Play();
            gospawn = true;
        }

        if (gospawn)
        {
            if (limuleCounter < 360)
            {
                LimuleCounter(prefabsSpawns);
                //myParticleSystem.Play();
            }
            else
            {
                if (ChangeOffsetOnce)
                {
                    //ChangeOffset();
                    ChangeOffsetOnce = false;
                }
            }
        }
    }


    public void TurnONLightOfspawnPointObject()
    {
        //spawnPointObject.GetComponentInChildren<Light>().enabled = true;
    }


    public void ActivateLimuleCounter()
    {
        gospawn = true;
        ChangeOffset();
        ChangeFOV();
        CameraVibration();
    }

    public void PlayAudioCounter()
    {
        audioSources[1].Play();

    }

    public void LaunchJumpAnim()
    {
        animator.SetBool("JumpSky", true);

        //TurnONLightOfspawnPointObject();    
        //audioSources[1].Stop();
        CameraShake();

    }
    public void ChangeFOV()
    {
        if (MainCamera != null)
        {
            CameraFollow target = MainCamera.GetComponent<CameraFollow>();
            if (target != null)
            {
                target.fovTransitionDuration = 2f;
                target.fovTargetThree += 20;


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
    public void ChangeOffset()
    {
        if (MainCamera != null)
        {
            CameraFollow target = MainCamera.GetComponent<CameraFollow>();
            if (target != null)
            {
                target.offset = new Vector3(0f, 0.05f, -1.47f);
                target.CallSmoothSpeed2Transition(1f, 0.2f, 2f);
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

    public void RotationCamera(float angle)
    {
        if (MainCamera != null)
        {
            CameraFollow target = MainCamera.GetComponent<CameraFollow>();
            if (target != null)
            {
                target.transform.rotation = Quaternion.Euler(-8.37f, 0f, angle);
                //target.SmoothTransitionRotation(Quaternion.Euler(-8.37f, 0f, angle), 0.5f);

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


    public void LimuleGoUP()
    {
        if (Player != null)
        {
            MoveSphereTunnel moveSphereTunnel = Player.GetComponent<MoveSphereTunnel>();
            if (moveSphereTunnel != null)
            {
                moveSphereTunnel.Speed2 = 4f;

            }
            else
            {
                Debug.LogError("CameraFollow component not found on PlayerLimuleTunnel.");
            }
        }
        else
        {
            Debug.LogError("Player is null.");
        }
    }

    private void LimuleCounter(int unit)
    {
        // Calculate the number of prefabs required for one full revolution

        for (int i = 0; i < unit; i++)
        {
            float angle = limuleCounter * angleIncrement;
            //RotationCamera(angle);
            // Calculate the number of revolutions based on the angle
            if (angle >= 360f)
            {
                print("Made-a-revolution");
                LaunchJumpAnim();
                gospawn = false;
            }

            // Calculate the position of the prefab around the circle using trigonometry
            float spawnX = spawnPoint.position.x + circleRadiusX * Mathf.Cos(angle * Mathf.Deg2Rad);
            float spawnY = spawnPoint.position.y + circleRadiusY * Mathf.Sin(angle * Mathf.Deg2Rad);
            float spawnZ = spawnPoint.position.z + circleRadiusZ * Mathf.Sin(angle * Mathf.Deg2Rad);

            // Create a rotation based on the current angle
            Quaternion rotation = Quaternion.Euler(0f, 0f, -angle + singleCountRot);

            // Create an empty GameObject as a child of spawnPoint
            GameObject rot = new GameObject("rot");
            rot.transform.SetParent(spawnPoint);
            // Set the position of the rot GameObject to (0, 0, 0) relative to its parent
            rot.transform.localPosition = Vector3.zero;

            // Spawn the prefab at the calculated position and rotation
            GameObject counterObj = Instantiate(prefabToSpawn, new Vector3(spawnX, spawnY, spawnPoint.position.z), rotation, rot.transform);
            // Rotate the rot GameObject
            rot.transform.Rotate(new Vector3(rotationAngleX, rotationAngleY, rotationAngleZ));

            // Randomly decide if this prefab should be black
            bool isBlack = Random.Range(0, 2) == 0;

            if (isBlack)
            {
                // Get the Renderer component from the prefab
                Renderer renderer = counterObj.GetComponent<Renderer>();

                if (renderer != null)
                {
                    // Clone the material to avoid changing all prefabs
                    Material clonedMaterial = new Material(renderer.material);
                    renderer.material = clonedMaterial;

                    // Set material base color to black
                    clonedMaterial.SetColor("_BaseColor", Color.black);
                    // Set material emissive color to black
                    clonedMaterial.SetColor("_EmissiveColor", Color.black);
                    // Set emissive intensity to 0
                    clonedMaterial.SetFloat("_EmissiveIntensity", 0f);
                }
            }

            limuleCounter++;
        }
    }

    public void CameraVibration()
    {

        if (MainCamera != null)
        {
            CameraFollow target = MainCamera.GetComponent<CameraFollow>();
            if (target != null)
            {
                //set target to limuleTunnel
                target.ShakeCamera(2f, 0.001f);
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
                target.offset = new Vector3(0f, 0.05f - offseteffect / 10, -1f - offseteffect / 10);
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
        target.offset = new Vector3(0f, 0.05f, -1.47f);
        //StartCoroutine(testfalling(0.2f));
        //target.offset = new Vector3(0, 0.1f, -1.37f);


    }



}
