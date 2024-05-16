using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GlowingFollower : MonoBehaviour
{
    private Animator animator;
    private AudioSource reverseSnare;
    private AudioSource endTunnel;

    private AudioSource impactSound;

    public GameObject MainCamera;

    bool impactSoundPlayed = false;

    void Start()
    {
        //Get default Parameters
        animator = GetComponentInChildren<Animator>();
        AudioSource[] audios = GetComponents<AudioSource>();
        //gameObject.tag = "Player";
        //reverseSnare = audios[6];
        endTunnel = audios[3];
        impactSound = audios[4];
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
            endTunnel.Play();
            if (!impactSoundPlayed)
            {
                impactSound.Play();
                impactSoundPlayed = true;
            }
            animator.SetBool("PlayAnim", true);

            StartCoroutine(turnOFFLightIn(0.1f));
            CamAddFov();
            //animator.SetBool("PlayAnim", false);
            //this.gameObject.tag = "Lamp";
        }

        if (other.tag == "reverseSnare")
        {
            //AudioSource[] sources = this.gameObject.GetComponents<AudioSource>();
            //sources[0].Play();
            //reverseSnare.Play();
            //animator.SetBool("PlayAnim", false);
            //this.gameObject.tag = "Lamp";
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

        //Debug.Log("coroutine has stopped");
    }

    public void CamAddFov()
    {
        // Check if pinceObject is not null and has the pinceScript component
        if (MainCamera != null)
        {
            CameraFollow target = MainCamera.GetComponent<CameraFollow>();
            if (target != null)
            {
                //set target to limuleTunnel
                //target.fovTargetThree += 5;
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
