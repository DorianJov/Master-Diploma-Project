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





    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("ENTER");

            active = true;

            animator.SetBool("lightUP", true);

            AudioSource[] sources = this.gameObject.GetComponents<AudioSource>();
            sources[0].Play();
            //sources[1].Play();

        }
    }

}
