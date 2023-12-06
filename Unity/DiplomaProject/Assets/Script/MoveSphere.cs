using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSphere : MonoBehaviour
{
    // Start is called before the first frame update


    float Speed = 0.0f; //Don’t touch this
    float MaxSpeed = 2f; //This is the maximum speed that the object will achieve
    float Acceleration = 0.1f; //How fast will object reach a maximum speed
    float Deceleration = 10f; //How fast will object reach a speed of 0
    void Start()
    {

    }


    void Update()
    {
        if ((Input.GetKey("left")) && (Speed < MaxSpeed)) Speed = Speed - Acceleration * Time.deltaTime;
        else if ((Input.GetKey("right")) && (Speed > -MaxSpeed)) Speed = Speed + Acceleration * Time.deltaTime;
        else
        {
            if (Speed > Deceleration * Time.deltaTime) Speed = Speed - Deceleration * Time.deltaTime;
            else if (Speed < -Deceleration * Time.deltaTime) Speed = Speed + Deceleration * Time.deltaTime;
            else
                Speed = 0;
        }

        transform.position += new Vector3(Speed * Time.deltaTime, 0f, 0f);

        Debug.Log("Current Speed: " + Speed);
    }


    /*
    void Update()
    {
        float speed = 0.4f;  // ...or whatever.

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
        }
        //
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
        }
    }
    */
}
