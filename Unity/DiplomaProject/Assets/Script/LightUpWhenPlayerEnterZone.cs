using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUpWhenPlayerEnterZone : MonoBehaviour
{

    private bool TurnOnLight = false;



    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (TurnOnLight)
        {

            gameObject.GetComponentInChildren<Light>().enabled = true;

        }
        else
        {
            gameObject.GetComponentInChildren<Light>().enabled = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTER");
        if (other.tag == "Lamp")
        {
            TurnOnLight = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("ENTER");
        if (other.tag == "Lamp")
        {
            TurnOnLight = true;
        }
    }
}
