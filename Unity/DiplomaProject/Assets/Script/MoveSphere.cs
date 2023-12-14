using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSphere : MonoBehaviour
{
    // Start is called before the first frame update


    float Speed = 0.0f; //Don’t touch this
    float Speed2 = 0.0f; //Don’t touch this

    float dash = 0f; //Don’t touch this
    float t = 0;

    public float MaxDashSpeed = 5f;
    public float DashsmoothSpeed = 0.5f;
    private float dashVelocity = 0.0f; //Don’t touch this
    private float dashSpeed = 1f; //Don’t touch this
    public float MaxSpeed = 0.5f; //This is the maximum speed that the object will achieve
    public float Acceleration = 0.1f; //How fast will object reach a maximum speed
    public float Deceleration = 10f; //How fast will object reach a speed of 0
    void Start()
    {
        gameObject.tag = "Player";
    }


    void Update()
    {
        if (Input.GetKey("a"))
        {
            if (Speed > -MaxSpeed) Speed -= Acceleration * Time.deltaTime;
        }
        else if (Input.GetKey("d"))
        {
            if (Speed < MaxSpeed) Speed += Acceleration * Time.deltaTime;
        }

        else
        {
            if (Speed > Deceleration * Time.deltaTime) Speed -= Deceleration * Time.deltaTime;
            else if (Speed < -Deceleration * Time.deltaTime) Speed += Deceleration * Time.deltaTime;
            else
                Speed = 0;
        }

        if (Input.GetKey("s"))
        {
            if (Speed2 > -MaxSpeed) Speed2 -= Acceleration * Time.deltaTime;
        }
        else if (Input.GetKey("w"))
        {
            if (Speed2 < MaxSpeed) Speed2 += Acceleration * Time.deltaTime;
        }

        else
        {
            if (Speed2 > Deceleration * Time.deltaTime) Speed2 -= Deceleration * Time.deltaTime;
            else if (Speed2 < -Deceleration * Time.deltaTime) Speed2 += Deceleration * Time.deltaTime;
            else
                Speed2 = 0;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) & dashSpeed == 1)
        {

            dashSpeed = MaxDashSpeed;
            //Debug.Log("CLICKEDCLICKEDCLICKEDCLICKEDCLICKEDCLICKEDCLICKEDCLICKED");
        }
        else
        {
            //Debug.Log("dashSpeed : " + dashSpeed);
            if (dashSpeed != 1)
            {
                dashSpeed = Mathf.Lerp(5, 1, t += DashsmoothSpeed * Time.deltaTime);
            }
            else
            {
                t = 0;
            }
        }



        transform.position += new Vector3(Speed * Time.deltaTime * dashSpeed, Speed2 * Time.deltaTime * dashSpeed, 0f);

        //Debug.Log("Current Speed: " + Speed);
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
