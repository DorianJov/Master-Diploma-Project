using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tunnelLimuleSpawn : MonoBehaviour
{
    private bool limuleSpawned = false;
    private Rigidbody rb;

    private MoveSphereTunnel moveSphereTunnel; // Reference to the other script

    // Start is called before the first frame update
    void Start()
    {
        // Cache the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Check if the Rigidbody component is found
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on the GameObject.");
        }

        // Cache the limuleMoveTunnel component
        moveSphereTunnel = GetComponent<MoveSphereTunnel>();

        // Check if the limuleMoveTunnel component is found
        if (moveSphereTunnel == null)
        {
            Debug.LogError("limuleMoveTunnel component not found on the GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (limuleSpawned && rb != null)
        {
            // Set velocity to move in the positive x direction
            float randomSpeed = 0.1f;
            rb.velocity = transform.right * randomSpeed;
            rb.useGravity = true;

            limuleSpawned = false;
        }
    }

    public void ActivateLimuleSpawn()
    {
        limuleSpawned = true;
        // Set canMove to true in the limuleMoveTunnel script
        if (moveSphereTunnel != null)
        {
            moveSphereTunnel.SphereCanMove = true;
        }

    }
}
