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
            this.gameObject.tag = "Lamp";
        }
        else
        {
            animator.SetBool("PlayAnim", false);
            this.gameObject.tag = "Untagged";
        }



    }


}
