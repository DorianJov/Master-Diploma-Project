using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class ChangeTargetAndDelPrev : MonoBehaviour

{
    public GameObject MainCamera;
    public GameObject InsideUsineLevel;
    public GameObject Tunnel;
    public GameObject SpotLightToTurnOff;
    public GameObject SlapScene;
    bool once = true;
    private float offseteffect = 1f;
    private AudioSource audioSource;




    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void CameraToSlapPhase()
    {
        // Check if pinceObject is not null and has the pinceScript component
        if (MainCamera != null)
        {
            CameraFollow target = MainCamera.GetComponent<CameraFollow>();
            if (target != null)
            {
                //check smooth switch ?

                target.SwitchCamTarget(5);
                target.CallSmoothSpeed2Transition(1f, 0.2f, 0.2f);
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
        // Check if pinceObject is not null and has the pinceScript component
        if (MainCamera != null)
        {
            CameraFollow target = MainCamera.GetComponent<CameraFollow>();
            if (target != null)
            {
                target.smoothSpeed2 = 0.05f;
                target.offset = new Vector3(0f, 0.05f + offseteffect / 30, -1.47f);
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

    void DeleteAllpreviousLevels()
    {
        StartCoroutine(UnloadAssetsCoroutine(11f));
        Destroy(InsideUsineLevel, 10f);
        Destroy(Tunnel);

    }

    IEnumerator UnloadAssetsCoroutine(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Unload unused assets after the delay
        Resources.UnloadUnusedAssets();
    }

    void ActivateSlapScene()
    {
        SlapScene.SetActive(true);

    }

    void DeactivateSpotLightInstant()
    {
        SpotLightToTurnOff.GetComponent<Light>().enabled = false;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //add entering hole sound_effect();
            if (once)
            {
                audioSource.Play();
                StartCoroutine(WaitForShake(0.1f));
                StartCoroutine(CallFunctionsIn(1f));
                once = false;
            }
        }

    }


    IEnumerator WaitForShake(float delay)
    {
        yield return new WaitForSeconds(delay);
        CameraShake();

    }

    IEnumerator CallFunctionsIn(float delay)
    {
        yield return new WaitForSeconds(delay);
        DeactivateSpotLightInstant();
        CameraToSlapPhase();
        DeleteAllpreviousLevels();
        ActivateSlapScene();
    }
}
