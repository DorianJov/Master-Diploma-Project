using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingFlower : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    public bool active = false;
    bool turnOFFLight = true;
    AudioSource[] sources;

    void Start()
    {
        //Get default Parameters
        sources = this.gameObject.GetComponents<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        //gameObject.tag = "Player";
    }

    // Update is called once per frame
    void Update()
    {





    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!active)
            {
                Debug.Log("ENTER");
                active = true;
                //StopCoroutine("turnOFFAudio");
                animator.SetBool("lightUP", true);
                sources[0].volume = 0.117f;
                sources[0].Play();

            }
            else
            {
                active = false;
                Debug.Log("ENTER");

                animator.SetBool("lightUP", false);
                sources[1].Play();
                StartCoroutine(turnOFFAudio());
            }

        }
    }

    IEnumerator turnOFFAudio()
    {
        // wait for 1 second
        //Debug.Log("turnOFFLight in 1 sec");
        AudioSource[] sources = this.gameObject.GetComponents<AudioSource>();
        //Debug.Log("coroutine has stopped");

        while (sources[0].volume != 0)
        {
            if (active)
            {
                yield break; // Exit the coroutine immediately
            }

            print("volume down");
            sources[0].volume -= Time.deltaTime * 0.1f;
            // Yield execution of this coroutine and return to the main loop until next frame
            // Check for cancellation


            yield return null;
        }

        //sources[0].Stop();
    }

}
