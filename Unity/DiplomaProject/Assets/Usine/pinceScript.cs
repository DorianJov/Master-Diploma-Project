using UnityEngine;

public class pinceScript : MonoBehaviour
{
    public float rotationSpeed = 0.1f; // Time interval between rotations in seconds
    public float stepSize = 5f; // Size of each rotation step
    public KeyCode rotateClockwiseKey = KeyCode.D; // Key to rotate clockwise
    public KeyCode rotateCounterClockwiseKey = KeyCode.A; // Key to rotate counter-clockwise
    //AudioSource audioSource; // Reference to the AudioSource component
    public GameObject otherObjectToRotate; // Reference to the other GameObject to rotate
    AudioSource[] audioSources;
    public GameObject spotlightToTurnOff; // Reference to the other GameObject to rotate
    public GameObject spotlightToTurnOff2; // Reference to the other GameObject to rotate

    public GameObject limulesSpawner;

    public GameObject MainCamera;

    public Renderer planeRendererOFF; // Drag the Plane Renderer component here in the Inspector

    public Renderer lit00; // Drag the Plane Renderer component here in the Inspector

    public Renderer lit01; // Drag the Plane Renderer component here in the Inspector

    public Renderer lit02; // Drag the Plane Renderer component here in the Inspector

    private bool rotateClockwise = false;
    private bool rotateCounterClockwise = false;
    private float nextRotationTime = 0f;
    private float minRotationInterval = 0.001f; // Minimum time interval between rotations
    public bool canRotate = false;

    int counter = 0;
    public int counterModulo = 2;

    public float currenteuleurAngles = 0f;
    private bool rotationLightsOffAudioPlayed = false;


    private void Start()
    {
        // Get reference to the AudioSource component
        audioSources = GetComponents<AudioSource>();
        //audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (canRotate)
        {
            // Check if the "D" key is pressed and Z rotation is not yet 90
            if (Input.GetKeyDown(rotateClockwiseKey) && currenteuleurAngles < 90f)
            {
                rotateClockwise = true;
                nextRotationTime = Time.time; // Start rotation immediately

            }
            // Check if the "A" key is pressed and Z rotation is not yet 0
            else if (Input.GetKeyDown(rotateCounterClockwiseKey) && currenteuleurAngles > 0f)
            {
                rotateCounterClockwise = true;
                nextRotationTime = Time.time; // Start rotation immediately

            }

            // Check if rotation keys are released
            if (Input.GetKeyUp(rotateClockwiseKey))
            {
                rotateClockwise = false;
            }

            if (Input.GetKeyUp(rotateCounterClockwiseKey))
            {
                rotateCounterClockwise = false;
            }

            // If rotation keys are pressed, rotate the object based on rotation speed
            if (rotateClockwise && Time.time >= nextRotationTime)
            {
                RotateClockwise(transform);
                //RotateClockwise(otherObjectToRotate.transform);
                nextRotationTime = Time.time + Mathf.Max(rotationSpeed, minRotationInterval); // Update next rotation time
                PlayRotationSound();
                counter++;
                if (counter % counterModulo == 0)
                {
                    Switchlit00(true);
                    Switchlit01(true);
                    Switchlit02(true);
                }
                else
                {
                    Switchlit00(false);
                    Switchlit01(false);
                    Switchlit02(false);

                }
            }
            else if (rotateCounterClockwise && Time.time >= nextRotationTime)
            {
                RotateCounterClockwise(transform);
                //RotateCounterClockwise(otherObjectToRotate.transform);
                nextRotationTime = Time.time + Mathf.Max(rotationSpeed, minRotationInterval); // Update next rotation time
                PlayRotationSound();
                counter++;
                if (counter % counterModulo == 0)
                {
                    Switchlit00(true);
                    Switchlit01(true);
                    Switchlit02(true);
                }
                else
                {
                    Switchlit00(false);
                    Switchlit01(false);
                    Switchlit02(false);

                }
            }


        }
        //Debug.Log("currenteuleurAngles" + currenteuleurAngles);

        if (currenteuleurAngles == 90)
        {
            canRotate = false;
            TurnOffSpotlight();
            TurnOffSpotlight2();
            TurnOffPlaneRenderer();
            PlayRotationLightsOFF();
            ActivateLimuleSpawner();
            CamTargetLimuleSpawner();
            Switchlit00(false);
            Switchlit01(false);
            Switchlit02(false);
        }
    }

    public void canRotateTrue()
    {
        canRotate = true;
    }

    private void RotateClockwise(Transform target)
    {
        float currentRotation = currenteuleurAngles;
        float newRotation = currentRotation + stepSize;

        // Clamp rotation to 90 degrees

        newRotation = Mathf.Clamp(newRotation, 0f, 90f);
        currenteuleurAngles = newRotation;

        target.localRotation = Quaternion.Euler(0f, 0f, -newRotation);
        otherObjectToRotate.transform.localRotation = Quaternion.Euler(0f, 0f, -newRotation);
        //Debug.Log("RotateClockwise" + -newRotation);

        // Stop rotation if reached 90 degrees
        if (newRotation <= -90f)
        {
            rotateClockwise = false;
        }
    }

    private void RotateCounterClockwise(Transform target)
    {
        float currentRotation = currenteuleurAngles;
        float newRotation = currentRotation - stepSize;

        // Clamp rotation to 0 degrees

        newRotation = Mathf.Clamp(newRotation, 0f, 90f);
        currenteuleurAngles = newRotation;

        //newRotation *= -1;

        target.localRotation = Quaternion.Euler(0f, 0f, -newRotation);
        otherObjectToRotate.transform.localRotation = Quaternion.Euler(0f, 0f, -newRotation);

        // Stop rotation if reached 0 degrees
        //Debug.Log("RotateCounterClockwise" + newRotation);
        if (newRotation <= 0f)
        {
            rotateCounterClockwise = false;
        }
    }

    private void PlayRotationSound()
    {
        if (audioSources != null)
        {
            audioSources[0].Play();
        }
    }

    private void PlayRotationLightsOFF()
    {
        if (!rotationLightsOffAudioPlayed && audioSources != null && audioSources.Length >= 2 && audioSources[1] != null)
        {
            audioSources[1].Play();
            rotationLightsOffAudioPlayed = true; // Set the flag to true to indicate that the audio has been played
        }
    }

    private void TurnOffSpotlight()
    {
        // Check if the spotlightToTurnOff is not null and has a Light component
        if (spotlightToTurnOff != null)
        {
            Light spotlightLight = spotlightToTurnOff.GetComponent<Light>();

            // Check if the spotlight has a Light component
            if (spotlightLight != null)
            {
                // Disable the light component
                spotlightLight.enabled = false;
            }
            else
            {
                Debug.LogError("No Light component found on spotlightToTurnOff GameObject.");
            }
        }
        else
        {
            Debug.LogError("spotlightToTurnOff GameObject is null.");
        }
    }

    private void TurnOffSpotlight2()
    {
        // Check if the spotlightToTurnOff2 is not null and has a Light component
        if (spotlightToTurnOff2 != null)
        {
            Light spotlightLight = spotlightToTurnOff2.GetComponent<Light>();

            // Check if the spotlight has a Light component
            if (spotlightLight != null)
            {
                // Disable the light component
                spotlightLight.enabled = false;
            }
            else
            {
                Debug.LogError("No Light component found on spotlightToTurnOff2 GameObject.");
            }
        }
        else
        {
            Debug.LogError("spotlightToTurnOff2 GameObject is null.");
        }
    }

    private void TurnOffPlaneRenderer()
    {
        // Check if the planeRendererOFF is not null
        if (planeRendererOFF != null)
        {
            // Disable the renderer
            planeRendererOFF.enabled = false;
        }
        else
        {
            Debug.LogError("planeRendererOFF is null.");
        }
    }

    private void Switchlit00(bool activate)
    {
        // Check if the planeRendererOFF is not null
        if (lit00 != null)
        {
            // Disable the renderer
            if (!activate)
            {
                lit00.enabled = false;
            }
            else
            {
                lit00.enabled = true;
            }
        }
        else
        {
            Debug.LogError("planeRendererOFF is null.");
        }
    }

    private void Switchlit01(bool activate)
    {
        // Check if the planeRendererOFF is not null
        if (lit01 != null)
        {
            // Disable the renderer
            if (!activate)
            {
                lit01.enabled = false;
            }
            else
            {
                lit01.enabled = true;
            }
        }
        else
        {
            Debug.LogError("planeRendererOFF is null.");
        }
    }


    private void Switchlit02(bool activate)
    {
        // Check if the planeRendererOFF is not null
        if (lit02 != null)
        {
            // Disable the renderer
            if (!activate)
            {
                lit02.enabled = false;
            }
            else
            {
                lit02.enabled = true;
            }
        }
        else
        {
            Debug.LogError("planeRendererOFF is null.");
        }
    }




    public void ActivateLimuleSpawner()
    {
        // Check if pinceObject is not null and has the pinceScript component
        if (limulesSpawner != null)
        {
            InstantiatePrefabOnKeyPress spawner = limulesSpawner.GetComponent<InstantiatePrefabOnKeyPress>();
            if (spawner != null)
            {
                spawner.TriggerSpawner();
            }
            else
            {
                Debug.LogError("InstantiatePrefabOnKeyPress component not found on pinceObject.");
            }
        }
        else
        {
            Debug.LogError("limulesSpawner is null.");
        }
    }

    public void CamTargetLimuleSpawner()
    {
        // Check if pinceObject is not null and has the pinceScript component
        if (MainCamera != null)
        {
            CameraFollow target = MainCamera.GetComponent<CameraFollow>();
            if (target != null)
            {
                target.SwitchCamTarget(4);
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

}
