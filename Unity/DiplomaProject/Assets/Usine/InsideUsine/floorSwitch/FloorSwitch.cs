using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private AudioSource floorSwitchSound;

    bool Once = true;
    void Start()
    {
        animator = GetComponent<Animator>();
        AudioSource[] audios = GetComponents<AudioSource>();
        floorSwitchSound = audios[0];
    }

    // Update is called once per frame


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("turnColor", true);
            floorSwitchSound.Play();
            //Once = false;
        }
        //this.gameObject.tag = "Lamp";
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("turnColor", false);
            floorSwitchSound.Play();
            //Once = false;
        }
        //this.gameObject.tag = "Lamp";
    }

}
