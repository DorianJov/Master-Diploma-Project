using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayerLIGHT : MonoBehaviour
{

    public Transform target;
    GlowingFlower GlowingFlower;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        //transform.LookAt(target);

        // Same as above, but setting the worldUp parameter to Vector3.left in this example turns the camera on its side
        // if (GlowingFlower.active)
        // {
        transform.LookAt(target, Vector3.left);
        // }




    }
}
