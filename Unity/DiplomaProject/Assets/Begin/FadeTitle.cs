using System.Collections;
using TMPro;
using UnityEngine;

public class FadeTitle : MonoBehaviour
{
    public TextMeshPro text1;
    public TextMeshPro text2;
    public float fadeDuration = 5f; // Duration of the fade in seconds
    public float startingAlpha = 1f; // Starting alpha value of the text
    bool firstTime = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrashBin"))
        {
            // Start the fade effect
            if (firstTime)
            {
                StartCoroutine(FadeText());
            }
            firstTime = false;
        }
    }

    IEnumerator FadeText()
    {
        float elapsedTime = 0f;
        Color startColor = text1.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Transparent color

        // Apply the starting alpha to both text objects
        text1.color = new Color(startColor.r, startColor.g, startColor.b, startingAlpha);
        text2.color = new Color(startColor.r, startColor.g, startColor.b, startingAlpha);

        while (elapsedTime < fadeDuration)
        {
            // Calculate the alpha value based on the elapsed time from the start of the coroutine
            float alpha = Mathf.Lerp(startingAlpha, 0f, elapsedTime / fadeDuration);

            // Apply the alpha value to both text objects
            text1.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            text2.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the text is fully transparent at the end of the fade
        text1.color = endColor;
        text2.color = endColor;
    }
}
