using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSwitchAudio : MonoBehaviour
{
    AudioSource switchSounds;

    AudioSource noteAudio;

    ParticleSystem particleSystem;

    public MoveTrashBox MoveTrashBox;
    // Start is called before the first frame update
    private Animator animator;
    void Start()
    {
        //AudioSource[] audios = GetComponents<AudioSource>();
        switchSounds = GetComponent<AudioSource>();
        particleSystem = GetComponent<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();
        MoveTrashBox.onBoosterActivated.AddListener(ActivateBoost);

        //switchSounds = audios[0];
        //noteAudio = audios[1];
    }

    // Update is called once per frame
    public void play_sound()
    {
        switchSounds.Play();
        //noteAudio.Play();
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

    void ActivateBoost()
    {
        // Existing code...

        // Start the boost coroutine
        animator.SetBool("switch", true);
        StartCoroutine(turnOFFAnimation(0.1f));
    }


    IEnumerator turnOFFAnimation(float seconds)
    {
        // wait for 1 second
        Debug.Log("turnOFFSwitch in 1 sec");
        yield return new WaitForSeconds(seconds);
        animator.SetBool("switch", false);

    }

}
