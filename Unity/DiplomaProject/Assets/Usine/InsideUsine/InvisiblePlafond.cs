using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisiblePlafond : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(activeWallInXseconds(3f));
        }
    }

    IEnumerator activeWallInXseconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        BoxCollider wall = this.GetComponent<BoxCollider>();

        if (wall != null)
        {
            wall.isTrigger = false;
        }
    }
}
