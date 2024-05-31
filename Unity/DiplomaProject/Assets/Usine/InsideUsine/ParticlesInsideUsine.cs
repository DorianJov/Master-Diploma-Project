using UnityEngine;

public class ParticlesInsideUsine : MonoBehaviour
{
    private ParticleSystem myParticleSystem;

    void Start()
    {
        myParticleSystem = GetComponent<ParticleSystem>();
        if (myParticleSystem == null)
        {
            Debug.LogError("ParticleSystem component not found on the GameObject.");
        }
    }

    public void SetVelocityOverLifetimeY(float newYVelocity)
    {
        if (myParticleSystem != null)
        {
            var velocityOverLifetime = myParticleSystem.velocityOverLifetime;
            velocityOverLifetime.enabled = true;

            // Assuming the velocity over lifetime is constant
            ParticleSystem.MinMaxCurve yVelocityCurve = velocityOverLifetime.y;
            yVelocityCurve.constant = newYVelocity;
            velocityOverLifetime.y = yVelocityCurve;

            Debug.Log($"Velocity over lifetime Y set to: {newYVelocity}");
        }
    }

    public void DeactivateEmission()
    {
        if (myParticleSystem != null)
        {
            var emission = myParticleSystem.emission;
            emission.enabled = false;

            Debug.Log("Particle emission deactivated.");
        }
    }
}
