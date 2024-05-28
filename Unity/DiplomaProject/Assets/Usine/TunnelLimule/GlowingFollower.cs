using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GlowingFollower : MonoBehaviour
{
    private Animator animator;
    private AudioSource reverseSnare;
    private AudioSource endTunnel;

    private AudioSource impactSound;

    private AudioSource hitfloor;

    public GameObject MainCamera;

    bool impactSoundPlayed = false;

    public bool FollowerWithLightComponent = true;

    public float TimeToPlaySoundAgain = 0.1f;

    bool playtwoTimesFloor = true;

    void Start()
    {
        //Get default Parameters
        animator = GetComponentInChildren<Animator>();
        AudioSource[] audios = GetComponents<AudioSource>();
        //gameObject.tag = "Player";
        //reverseSnare = audios[6];
        endTunnel = audios[3];
        impactSound = audios[4];
        hitfloor = audios[5];
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("LIGHT");
        if (other.tag == "sunFlowers")
        {
            //AudioSource[] sources = this.gameObject.GetComponents<AudioSource>();
            //sources[0].Play();

            playAnim(true);

            //animator.SetBool("PlayAnim", false);
            //this.gameObject.tag = "Lamp";
        }

        if (other.tag == "falling")
        {
            //AudioSource[] sources = this.gameObject.GetComponents<AudioSource>();
            //sources[0].Play();
            //endTunnel.Play();
            if (!impactSoundPlayed)
            {
                //impactSound.Play();
                //impactSoundPlayed = true;
            }

            playAnim(true);

            //CamAddFov();
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

        if (other.tag == "floorbutton")
        {
            playAnim(true);
        }

        if (other.tag == "MoveSphere")
        {

            if (playtwoTimesFloor)
            {
                playAnim(true);
                hitfloor.Play();
                hitfloor.pitch = 3;
                hitfloor.volume = 0.01f;
                StartCoroutine(playHitfloorAgainIn(TimeToPlaySoundAgain));
                playtwoTimesFloor = false;
            }
        }
    }

    IEnumerator playHitfloorAgainIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        playAnim(true);
        hitfloor.Play();
    }

    public void Play_Sync_Sound()
    {
        playAnim(true);
        hitfloor.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (FollowerWithLightComponent)
        {
            //playAnim(true);
        }
        //this.gameObject.tag = "Lamp";
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

    void playAnim(bool Switch)
    {
        if (FollowerWithLightComponent)
        {
            animator.SetBool("PlayAnim", Switch);
            if (Switch)
            {
                StartCoroutine(turnOFFLightIn(0.1f));
            }
        }

    }


    IEnumerator turnOFFLightIn(float seconds)
    {
        // wait for 1 second
        // Debug.Log("turnOFFLight in 1 sec");
        yield return new WaitForSeconds(seconds);
        playAnim(false);

        //Debug.Log("coroutine has stopped");
    }

}
