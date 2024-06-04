using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage; // Assign the UI Image in the Inspector
    public float fadeDuration = 5f; // Duration for the fade effect

    void Start()
    {
        // Ensure the Image starts fully transparent
        fadeImage.color = new Color(0f, 0f, 0f, 0f);
    }

    /*void Update()
    {
        if (Input.GetKey("z"))
        {
            print("FadeOUT");
            FadeOut();
        }

        if (Input.GetKey("u"))
        {
            print("FadeIN");
            FadeIn();
        }
    }*/

    // Call this method to start fading to black
    public void FadeOut()
    {
        StartCoroutine(Fade(1f));
    }

    // Call this method to start fading to clear
    public void FadeIn()
    {
        StartCoroutine(Fade(0f));
    }

    IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float elapsedTime = 0f;

        // Set the color to black (but transparent) at the start of the fade
        fadeImage.color = new Color(0f, 0f, 0f, startAlpha);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, newAlpha);
            yield return null;
        }

        // Ensure the final alpha is set
        fadeImage.color = new Color(0f, 0f, 0f, targetAlpha);
    }
}
