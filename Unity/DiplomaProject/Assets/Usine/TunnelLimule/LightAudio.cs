using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAudio : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource LightsON;
    void Start()
    {
        AudioSource[] audios = GetComponents<AudioSource>();
        LightsON = audios[0];
    }

    // Update is called once per frame
    public void PlayAudioLight()
    {
        LightsON.Play();
    }
}
