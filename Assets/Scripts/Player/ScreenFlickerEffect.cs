using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlickerEffect : MonoBehaviour
{
    [Header("Flicker Settings")]
    [Tooltip("Color used for the flicker effect (e.g., semi-transparent red).")]
    public Color flickerColor = new Color(1f, 0f, 0f, 0.5f);
    [Tooltip("Duration of each flicker in seconds.")]
    public float flickerDuration = 0.1f;
    [Tooltip("Minimum time between flickers in seconds.")]
    public float flickerIntervalMin = 0.1f;
    [Tooltip("Maximum time between flickers in seconds.")]
    public float flickerIntervalMax = 0.5f;

    private Image overlayImage;
    private bool isFlickering = false;
    private Coroutine flickerCoroutine;

    private void Awake()
    {
        overlayImage = GetComponent<Image>();
        if (overlayImage != null)
        {
            overlayImage.color = Color.clear;
        }
    }

    /// <summary>
    /// Starts the red flicker effect.
    /// </summary>
    public void StartFlicker()
    {
        if (isFlickering) return;
        isFlickering = true;
        flickerCoroutine = StartCoroutine(Flicker());
    }

    /// <summary>
    /// Stops the flicker effect and clears the overlay.
    /// </summary>
    public void StopFlicker()
    {
        isFlickering = false;
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
        }
        if (overlayImage != null)
        {
            overlayImage.color = Color.clear;
        }
    }

    private IEnumerator Flicker()
    {
        while (isFlickering)
        {
            // Wait for a random interval before flickering
            float waitTime = Random.Range(flickerIntervalMin, flickerIntervalMax);
            yield return new WaitForSeconds(waitTime);

            // Set overlay to flicker color
            if (overlayImage != null)
            {
                overlayImage.color = flickerColor;
            }
            yield return new WaitForSeconds(flickerDuration);

            // Return overlay to transparent
            if (overlayImage != null)
            {
                overlayImage.color = Color.clear;
            }
        }
    }
}
