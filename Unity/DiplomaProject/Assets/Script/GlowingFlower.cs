using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingFlower : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    public bool active = false;


    void Start()
    {
        //Get default Parameters

        animator = GetComponentInChildren<Animator>();
        //gameObject.tag = "Player";
    }

    // Update is called once per frame
    void Update()
    {


        if (active)
        {

        }


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!active)
            {
                Debug.Log("ENTER");



                animator.SetBool("lightUP", true);

                AudioSource[] sources = this.gameObject.GetComponents<AudioSource>();
                sources[0].volume = 0.117f;
                sources[0].Play();
                active = true;
            }
            else
            {

                Debug.Log("ENTER");



                animator.SetBool("lightUP", false);

                AudioSource[] sources = this.gameObject.GetComponents<AudioSource>();
                sources[1].Play();
                StartCoroutine(turnOFFAudio());
                active = false;

            }
            //sources[1].Play();
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
            print("volune down");
            sources[0].volume -= Time.deltaTime * 0.1f;
            // Yield execution of this coroutine and return to the main loop until next frame
            yield return null;
        }

        //sources[0].Stop();
    }

}
