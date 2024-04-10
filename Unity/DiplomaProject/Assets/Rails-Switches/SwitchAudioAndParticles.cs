using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSwitchAudio : MonoBehaviour
{
    AudioSource switchSounds;
    ParticleSystem particleSystem;
    // Start is called before the first frame update
    private Animator animator;
    void Start()
    {
        switchSounds = GetComponent<AudioSource>();
        particleSystem = GetComponent<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    public void play_sound()
    {
        switchSounds.Play();
    }

    public void PlayParticle()
    {
        // Play the particle system
        particleSystem.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrashBin"))
        {
            animator.SetBool("switch", true);
            StartCoroutine(turnOFFAnimation(0.1f));
        }
    }


    IEnumerator turnOFFAnimation(float seconds)
    {
        // wait for 1 second
        Debug.Log("turnOFFSwitch in 1 sec");
        yield return new WaitForSeconds(seconds);
        animator.SetBool("switch", false);

    }

}
