using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;


public class GlowingAttack : MonoBehaviour
{

    //public float lightIntensity;
    // Start is called before the first frame update
    //public GameObject m_LightObject;
    private float volumetricDimmer;
    private float intensity;
    private float range;

    private float defaultVolumetricDimmer;
    private float defaultIntensity;
    private float defaultRange;


    public float TargetLight = 16f;
    public float timeToLerp = 0.25f;


    public float targetScale;
    float scaleModifier = 1;
    void Start()
    {
        //Get default Parameters

        defaultVolumetricDimmer = GetComponent<HDAdditionalLightData>().volumetricDimmer;
        defaultIntensity = GetComponent<HDAdditionalLightData>().intensity;
        defaultRange = GetComponent<HDAdditionalLightData>().range;

        volumetricDimmer = GetComponent<HDAdditionalLightData>().volumetricDimmer;
        intensity = GetComponent<HDAdditionalLightData>().intensity;
        range = GetComponent<HDAdditionalLightData>().range;

    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<HDAdditionalLightData>().volumetricDimmer = 16f;
        //GetComponent<HDAdditionalLightData>().intensity = 16f;
        //GetComponent<HDAdditionalLightData>().range = 16f;
        //StartCoroutine(LerpFunction(TargetLight, timeToLerp));
        //StartCoroutine(LerpFunction(0, timeToLerp));
        //StartCoroutine(LerpFunction(defaultVolumetricDimmer, timeToLerp));

        /// GetComponent<HDAdditionalLightData>().volumetricDimmer = Mathf.PingPong(Time.time * 10, 16);
    }

    IEnumerator LerpFunction(float endValue, float duration)
    {
        float time = 0;
        float startValue = 1;
        float startScale = 1;
        while (time < duration)
        {
            scaleModifier = Mathf.Lerp(startValue, endValue, time / duration);
            GetComponent<HDAdditionalLightData>().volumetricDimmer = startScale * scaleModifier;
            time += Time.deltaTime;
            yield return null;
        }

        GetComponent<HDAdditionalLightData>().volumetricDimmer = startScale * endValue;
        scaleModifier = endValue;
    }
}
