using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GlowingFlower : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    public bool active = false;

    public float LookAtSpeed = 0.1f;

    Transform circleChild;
    //Transform lightChild;

    public Transform target;
    public Transform RestTarget;
    public bool lookingAtGenerator = false;
    AudioSource[] sources;

    ParticleSystem myParticleSystem;

    void Start()
    {
        //Get default Parameters
        sources = this.gameObject.GetComponents<AudioSource>();
        circleChild = this.gameObject.transform.GetChild(0);
        //lightChild = this.gameObject.transform.GetChild(0).GetChild(0);
        animator = GetComponentInChildren<Animator>();
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
        //gameObject.tag = "Player";
    }

    // Update is called once per frame
    void Update()
    {
        //circleChild.transform.Rotate(Vector3.right, 90f, Space.Self);

        if (active)
        {

            //StartCoroutine(LookAtSmoothly());


        }



    }

    private IEnumerator LookAtSmoothly(Transform target)
    {

        if (!active) { yield break; }

        while (active)
        {
            Quaternion lookRotation = Quaternion.LookRotation(target.position - circleChild.transform.position);
            circleChild.transform.rotation = Quaternion.Slerp(circleChild.transform.rotation, lookRotation, Time.deltaTime * LookAtSpeed);
            yield return null;
        }


    }


    private IEnumerator LookAtRestSmoothly(Transform target)
    {

        if (active) { yield break; }

        while (!active)
        {
            Quaternion lookRotation = Quaternion.LookRotation(target.position - circleChild.transform.position);
            circleChild.transform.rotation = Quaternion.Slerp(circleChild.transform.rotation, lookRotation, Time.deltaTime * LookAtSpeed);
            if (circleChild.transform.rotation == lookRotation) { yield break; }
            yield return null;
        }


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!active)
            {
                myParticleSystem.Play();
                Debug.Log("ENTER");
                active = true;
                StartCoroutine(LookAtSmoothly(target));
                animator.SetBool("lightUP", true);
                sources[0].volume = 0.117f;
                sources[0].Play();

            }
            else
            {
                active = false;
                StartCoroutine(LookAtRestSmoothly(RestTarget));
                Debug.Log("ENTER");
                animator.SetBool("lightUP", false);
                sources[1].Play();
                StartCoroutine(TurnOFFAudio());
            }

        }

        if (other.CompareTag("Player"))
        {


        }
    }

    IEnumerator TurnOFFAudio()
    {
        AudioSource[] sources = this.gameObject.GetComponents<AudioSource>();
        while (sources[0].volume != 0)
        {
            if (active)
            {
                yield break; // Exit the coroutine immediately
            }
            print("volume down");
            sources[0].volume -= Time.deltaTime * 0.1f;
            yield return null;
        }
    }

}
