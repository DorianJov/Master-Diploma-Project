using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GlowingAttack : MonoBehaviour
{
    private Animator animator;
    private AudioSource reverseSnare;
    private AudioSource endTunnel;
    public AudioSource fallingSound;

    private AudioSource impactSound;
    public GameObject MainCamera;

    public float offseteffect = 1f;

    bool impactSoundPlayed = false;

    bool triggerColliderMoveSphereOnce = true;

    bool floorbuttonOnce = true;

    public GameObject InsideUsineSpotLight01; // Reference to the other GameObject to rotate



    void Start()
    {
        //Get default Parameters
        animator = GetComponentInChildren<Animator>();
        AudioSource[] audios = GetComponents<AudioSource>();
        //gameObject.tag = "Player";
        reverseSnare = audios[6];
        endTunnel = audios[7];
        fallingSound = audios[8];
        impactSound = audios[9];

        endTunnel.Play();
        endTunnel.Stop();
        fallingSound.Play();
        fallingSound.Stop();
    }

    // Update is called once per frame
    void Update()
    {



        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("PlayAnim", true);
            //this.gameObject.tag = "Lamp";
        }
        else
        {
            //animator.SetBool("PlayAnim", false);
            //this.gameObject.tag = "Untagged";
        }



    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("LIGHT");
        if (other.tag == "sunFlowers")
        {
            //AudioSource[] sources = this.gameObject.GetComponents<AudioSource>();
            //sources[0].Play();
            animator.SetBool("PlayAnim", true);
            StartCoroutine(turnOFFLightIn(0.1f));
            //animator.SetBool("PlayAnim", false);
            //this.gameObject.tag = "Lamp";
        }

        if (other.tag == "falling")
        {
            //AudioSource[] sources = this.gameObject.GetComponents<AudioSource>();
            //sources[0].Play();
            //endTunnel.Play();
            fallingSound.Play();
            if (!impactSoundPlayed)
            {
                impactSound.Play();
                impactSoundPlayed = true;
            }
            animator.SetBool("PlayAnim", true);
            StartCoroutine(turnOFFLightIn(0.1f));
            CamSakeEffect();
            //animator.SetBool("PlayAnim", false);
            //this.gameObject.tag = "Lamp";
        }

        if (other.tag == "reverseSnare")
        {
            //AudioSource[] sources = this.gameObject.GetComponents<AudioSource>();
            //sources[0].Play();
            reverseSnare.Play();
            //animator.SetBool("PlayAnim", false);
            //this.gameObject.tag = "Lamp";
        }

        if (other.tag == "MoveSphere")
        {
            //AudioSource[] sources = this.gameObject.GetComponents<AudioSource>();
            //sources[0].Play();
            //endTunnel.Stop();
            //fallingSound.Stop();
            if (triggerColliderMoveSphereOnce)
            {
                //TurnONSpotlightInsideUsine();
                CamAddFOV();
                animator.SetBool("PlayAnimShort", true);
                StartCoroutine(turnOFFLightIn(0.1f));
                triggerColliderMoveSphereOnce = false;
            }
            //animator.SetBool("PlayAnim", false);
            //this.gameObject.tag = "Lamp";
        }


        if (other.tag == "floorbutton")
        {
            if (floorbuttonOnce)
            {
                StartCoroutine(lauchShakeEffect(1.6f));
                floorbuttonOnce = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("PlayAnim", false);
        //this.gameObject.tag = "Lamp";
    }



    IEnumerator turnOFFLightIn(float seconds)
    {
        // wait for 1 second
        // Debug.Log("turnOFFLight in 1 sec");
        yield return new WaitForSeconds(seconds);
        animator.SetBool("PlayAnim", false);
        animator.SetBool("PlayAnimShort", false);

        //Debug.Log("coroutine has stopped");
    }

    public void CamSakeEffect()
    {
        // Check if pinceObject is not null and has the pinceScript component
        if (MainCamera != null)
        {
            CameraFollow target = MainCamera.GetComponent<CameraFollow>();
            if (target != null)
            {
                //set target to limuleTunnel
                //target.fovTargetThree += 20f;
                target.smoothSpeed2 = 0.05f;
                target.offset = new Vector3(0.12f + offseteffect, 0.3f, -1.37f);
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

    public void CamAddFOV()
    {
        // Check if pinceObject is not null and has the pinceScript component
        if (MainCamera != null)
        {
            CameraFollow target = MainCamera.GetComponent<CameraFollow>();
            if (target != null)
            {
                //set target to limuleTunnel
                target.fovTransitionDuration = 5f;
                target.fovTargetThree += 20f;
                //change max Y:
                target.minY = -4.2f;
                target.maxY = -6f;
                target.smoothSpeed2 = 0.05f;
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

    IEnumerator testfalling(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        CameraFollow target = MainCamera.GetComponent<CameraFollow>();
        target.smoothSpeed2 = 1f;
        target.offset = new Vector3(0.0f, 0.0f, -1f);
        //target.offset = new Vector3(0, 0.1f, -1.37f);


    }

    IEnumerator lauchShakeEffect(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        CamSakeEffectZ();
        TurnONSpotlightInsideUsine();


    }


    public void CamSakeEffectZ()
    {
        // Check if pinceObject is not null and has the pinceScript component
        if (MainCamera != null)
        {
            CameraFollow target = MainCamera.GetComponent<CameraFollow>();
            if (target != null)
            {
                //set target to limuleTunnel
                target.fovTransitionDuration = 2f;
                target.fovTargetThree += 20f;
                target.smoothSpeed2 = 0.05f;
                target.offset = new Vector3(0.12f - offseteffect / 10, 0.3f - offseteffect / 10, -1.37f - offseteffect / 10);
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

    private void TurnONSpotlightInsideUsine()
    {
        // Check if the spotlightToTurnOff2 is not null and has a Light component
        if (InsideUsineSpotLight01 != null)
        {
            Light spotlightLight = InsideUsineSpotLight01.GetComponent<Light>();
            LightAudio lightAudioScript = InsideUsineSpotLight01.GetComponent<LightAudio>();

            // Check if the spotlight has a Light component
            if (spotlightLight != null & lightAudioScript != null)
            {
                // Disable the light component
                lightAudioScript.PlayAudioLight();
                spotlightLight.enabled = true;
            }
            else
            {
                Debug.LogError("No Light component or script LightAudio found on InsideUsineSpotLight01 GameObject.");
            }
        }
        else
        {
            Debug.LogError("InsideUsineSpotLight01 GameObject is null.");
        }
    }

}
