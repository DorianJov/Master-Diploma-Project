using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine;

public class SwitchAudioAndParticles : MonoBehaviour
{
    AudioSource switchSounds;
    //ParticleSystem particleSystem;

    public Material newEmissiveMaterial; // Drag the Material149 here in the Inspector
    public Renderer planeRenderer; // Drag the Plane Renderer component here in the Inspector

    public bool oppositeRails = false;
    public MoveTrashBox MoveTrashBox;
    // Start is called before the first frame update
    private Animator animator;

    public GameObject childObject;
    public Transform newParent;
    void Start()
    {

        switchSounds = GetComponent<AudioSource>();
        //particleSystem = GetComponent<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();
        MoveTrashBox.onBoosterActivated.AddListener(ActivateBoost);

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
        //particleSystem.Play();
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
        //animator.SetBool("switch", true);
        StartCoroutine(turnOFFAnimation(0.1f));
    }

    public void SetParent()
    {
        if (childObject != null && newParent != null)
        {
            // Store the original scale of the childObject
            //Vector3 originalScale = childObject.transform.localScale;

            // Set the childObject's parent to the newParent
            childObject.transform.parent = newParent;

            // Reset the scale of the childObject to its original scale
            //childObject.transform.localScale = originalScale;
        }
        else
        {
            Debug.LogError("One or both GameObjects are not assigned. Cannot set parent.");
        }
    }


    IEnumerator turnOFFAnimation(float seconds)
    {
        // wait for 1 second
        Debug.Log("turnOFFSwitch in 1 sec");
        yield return new WaitForSeconds(seconds);
        animator.SetBool("switch", false);

    }



    // Function to change emissive map to 149.png
    // Function to change emissive map to 149.png
    public void ChangeEmissive()
    {
        // Check if the renderer and new material are assigned

        // Change the material of the plane renderer
        if (!oppositeRails)
        {
            planeRenderer.material = newEmissiveMaterial;
        }

    }


}
