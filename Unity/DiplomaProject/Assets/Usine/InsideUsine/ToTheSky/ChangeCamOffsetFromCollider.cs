using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeCamOffsetFromCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MainCamera;
    bool DoitOnce = true;

    public void CameraToTheSkyPhase()
    {
        // Check if pinceObject is not null and has the pinceScript component
        if (MainCamera != null)
        {
            CameraFollow target = MainCamera.GetComponent<CameraFollow>();
            if (target != null)
            {
                target.StoprotationCoroutine();
                //target.transform.rotation = Quaternion.Euler(7.85f, 0f, 0f);
                //target.applyoffsetOnceTarget3 = true;
                target.offset = new Vector3(0f, 0.05f, -1.37f);
                target.CallSmoothSpeed2Transition(1f, 0.2f, 5f);
                //target.fovTransitionDuration = 0.5f;
                //target.smoothSpeed2 = 3f;
                target.minY = -100f;
                target.maxY = 100f;
                //target.SmoothTransitionRotation(Quaternion.Euler(7.85f, 0f, 0f), 0.1f);
            }
            else
            {
                Debug.LogError("CameraFollow component not found on pinceObject.");
            }
        }
        else
        {
            Debug.LogError("MainCamera is null.");
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (DoitOnce)
            {
                CameraToTheSkyPhase();
                DoitOnce = false;
            }
        }
    }
}
