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

    // Start is called before the first frame update
    private void Start()
    {
        flock = GameObject.FindGameObjectWithTag("Flock").GetComponent<Flock>();
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out glitch);
        volume.profile.TryGet(out bloom);
    }

    // Update is called once per frame
    private void Update()
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

    float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return Mathf.Lerp(outMin, outMax, Mathf.InverseLerp(inMin, inMax, value));
    }

}
