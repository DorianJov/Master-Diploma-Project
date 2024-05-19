using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuetterfollowTarget : MonoBehaviour
{

    public Transform target;
    Transform circleChild;

    public float LookAtSpeed = 0.1f;

    public bool active = false;

    bool once = true;
    // Start is called before the first frame update
    void Start()
    {
        circleChild = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator LookAtSmoothly(Transform target)
    {

        while (active)
        {
            print("guetteur is guettin");
            Quaternion lookRotation = Quaternion.LookRotation(target.position - circleChild.transform.position);
            circleChild.transform.rotation = Quaternion.Slerp(circleChild.transform.rotation, lookRotation, Time.deltaTime * LookAtSpeed);

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {



        if (other.CompareTag("Player"))
        {
            if (once)
            {
                print("WESH");
                active = true;
                StartCoroutine(LookAtSmoothly(target));

                once = false;
            }

        }
    }
}
