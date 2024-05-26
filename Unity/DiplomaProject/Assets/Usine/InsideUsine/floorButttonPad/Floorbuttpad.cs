using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floorbuttpad : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    //private AudioSource[] audioSources;

    private floorbuttSpawner spawner;

    bool Once = true;
    void Start()
    {
        animator = GetComponent<Animator>();
        // Find the floorbuttSpawner script in the parent hierarchy
        spawner = GetComponentInParent<floorbuttSpawner>();
        if (spawner == null)
        {
            Debug.LogError("floorbuttSpawner script not found in parent hierarchy.");
        }

        //floorSwitchSound = audios[0];
    }

    // Update is called once per frame


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Once)
            {
                if (spawner != null)
                {
                    spawner.PlayRandomAudio(); // Call the method to play a random audio from the spawner
                }
                animator.SetBool("turnColor", true);
                //Play one audioclip randomly between all audios clip attached to this gameobject prefab

                Once = false;
            }
        }
        //this.gameObject.tag = "Lamp";
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //animator.SetBool("turnColor", false);
            //floorSwitchSound.Play();
            //Once = true;
        }
        //this.gameObject.tag = "Lamp";
    }

}
