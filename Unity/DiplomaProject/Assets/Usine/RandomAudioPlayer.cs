using UnityEngine;

public class RandomAudioPlayer : MonoBehaviour
{
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources != null && audioSources.Length > 0)
        {
            int randomIndex = Random.Range(0, audioSources.Length);
            AudioSource selectedAudioSource = audioSources[randomIndex];

            if (selectedAudioSource != null)
            {
                selectedAudioSource.Play();

                // Get the length of the audio clip
                float audioClipLength = selectedAudioSource.clip.length;

                // Destroy the GameObject after the duration of the audio clip
                Destroy(gameObject, audioClipLength);
            }
            else
            {
                Debug.LogError("One of the AudioSource components in the array is null.");
            }
        }
        else
        {
            Debug.LogError("No AudioSource components assigned to the array.");
        }
    }
}
