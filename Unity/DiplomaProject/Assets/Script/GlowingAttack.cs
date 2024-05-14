using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GlowingAttack : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        //Get default Parameters
        animator = GetComponentInChildren<Animator>();
        //gameObject.tag = "Player";
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
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("PlayAnim", false);
        //this.gameObject.tag = "Lamp";
    }



    IEnumerator turnOFFLightIn(float seconds)
    {
        // wait for 1 second
        Debug.Log("turnOFFLight in 1 sec");
        yield return new WaitForSeconds(seconds);
        animator.SetBool("PlayAnim", false);

        Debug.Log("coroutine has stopped");
    }

}
