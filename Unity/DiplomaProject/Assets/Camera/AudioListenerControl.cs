using UnityEngine;

public class AudioListenerControl : MonoBehaviour
{
    // Public fields to reference the child GameObjects
    public Transform SoundListener;
    public Transform SoundListenerGuetteurScene;

    void Start()
    {
        // Optionally, you can find the listeners if not assigned in the Inspector
        if (SoundListener == null)
            SoundListener = transform.Find("SoundListener");

        if (SoundListenerGuetteurScene == null)
            SoundListenerGuetteurScene = transform.Find("SoundListenerGuetteurScene");

        // Ensure listeners are found
        if (SoundListener == null || SoundListenerGuetteurScene == null)
        {
            Debug.LogError("Listeners not found. Ensure the names are correct and the objects are direct children.");
        }

        // Ensure that only one listener is enabled at the start
        SetActiveListener("Listener01");
    }

    // Function to enable the specified listener and disable the other
    public void SetActiveListener(string listenerName)
    {
        // Disable all listeners first
        DisableListener(SoundListener);
        DisableListener(SoundListenerGuetteurScene);

        // Enable the specified listener
        switch (listenerName)
        {
            case "Listener01":
                EnableListener(SoundListener);
                break;
            case "Listener02":
                EnableListener(SoundListenerGuetteurScene);
                break;
            default:
                Debug.LogWarning("Listener not recognized.");
                break;
        }
    }

    private void EnableListener(Transform listener)
    {
        if (listener != null)
        {
            AudioListener audioListener = listener.GetComponent<AudioListener>();
            if (audioListener != null)
            {
                audioListener.enabled = true;
            }
        }
    }

    private void DisableListener(Transform listener)
    {
        if (listener != null)
        {
            AudioListener audioListener = listener.GetComponent<AudioListener>();
            if (audioListener != null)
            {
                audioListener.enabled = false;
            }
        }
    }
}
