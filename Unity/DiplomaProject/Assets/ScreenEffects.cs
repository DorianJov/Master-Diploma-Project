using System.Collections;
using System.Collections.Generic;
using Kino.PostProcessing;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class ScreenEffects : MonoBehaviour
{
    Flock flock;
    private Volume volume;

    private Glitch glitch;
    private Bloom bloom;

    private ChromaticAberration chromaticAberration;

    [Header("DarkillonsDistance")]
    public float minDarkillonsDistance = 0.12f;
    public float maxDarkillonsDistance = 0.4f;

    [Header("Glitch")]
    public float minGlitchIntensity = 0f;
    public float maxGlitchIntensity = 1f;

    [Header("Bloom")]
    public float minBloomIntensity = 0.026f;
    public float maxBloomIntensity = 10f;

    public float minBloomThresholdIntensity = 4.13f;
    public float maxBloomThresholdIntensity = 0f;

    [Header("Jump")]

    public float minJumpIntensity = 0.026f;
    public float maxJumpIntensity = 10f;

    [Header("Shake")]
    public MoveTrashBox MoveTrashBox;

    // Start is called before the first frame update
    private void Start()
    {
        flock = GameObject.FindGameObjectWithTag("Flock").GetComponent<Flock>();
        MoveTrashBox.onBoosterActivated.AddListener(ActivateBoost);
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out glitch);
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out chromaticAberration);

        bloom.threshold.Override(4.13f);
        bloom.intensity.Override(0.026f);
        glitch.drift.Override(0);
        glitch.jitter.Override(0);

    }

    // Update is called once per frame
    private void Update()
    {
        if (flock.darkillonsAreTargetingPlayer)
        {
            if (flock != null & flock.averageDistance < 1)
            {
                Debug.Log("Average Distance: " + flock.averageDistance);
                float glitchIntensity = Map(flock.averageDistance, maxDarkillonsDistance, minDarkillonsDistance, minGlitchIntensity, maxGlitchIntensity);
                glitch.drift.Override(glitchIntensity);
                glitch.jitter.Override(glitchIntensity);

                float bloomIntensity = Map(flock.averageDistance, maxDarkillonsDistance / 2, minDarkillonsDistance, minBloomIntensity, maxBloomIntensity);
                bloom.intensity.Override(bloomIntensity);

                float bloomThresholdIntensity = Map(flock.averageDistance, maxDarkillonsDistance / 2, minDarkillonsDistance, minBloomIntensity, maxBloomIntensity);
                bloom.threshold.Override(bloomThresholdIntensity);



            }
            else
            {
                bloom.threshold.Override(4.13f);
                bloom.intensity.Override(0.026f);
                glitch.drift.Override(0);
                glitch.jitter.Override(0);

            }
        }
        else
        {
            /*bloom.threshold.Override(4.13f);
            bloom.intensity.Override(0.026f);
            glitch.drift.Override(0);
            glitch.jitter.Override(0);*/
        }

    }

    void ActivateBoost()
    {
        //print("CACA");
        //StartCoroutine(ShakeCamera(0.1f, 0.2f));
        //glitch.shake.Override(1f);
    }

    public void ActivateJumpPostEffect(float start, float duration)
    {
        if (glitch != null)
        {
            StartCoroutine(GlitchEffectCoroutine(start, duration));
            jitterfunction();
        }
    }

    private IEnumerator GlitchEffectCoroutine(float start, float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            // Lerp the jitter value from start to 0 over the duration
            glitch.jump.Override(Mathf.Lerp(start, 0, timeElapsed / duration));
            //glitch.jitter.Override(Mathf.Lerp(start, 0, timeElapsed / duration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the jitter is set to 0 after the duration
        glitch.jump.Override(0);
        //glitch.jitter.Override(0);
    }

    void jitterfunction()
    {
        //glitch.jump.Override(0.01f);
        //glitch.jitter.Override(0.05f);
        //chromaticAberration.intensity.Override(1f);


    }

    float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return Mathf.Lerp(outMin, outMax, Mathf.InverseLerp(inMin, inMax, value));
    }

    IEnumerator ShakeCamera(float intensity, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float shakeIntensity = Mathf.Lerp(intensity, 0f, elapsedTime / duration);
            glitch.shake.Override(shakeIntensity);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Ensure the shake is completely turned off after the duration
        glitch.shake.Override(0f);
    }

}
