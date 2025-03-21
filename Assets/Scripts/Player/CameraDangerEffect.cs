using UnityEngine;
using System.Collections;

public class CameraDangerEffect : MonoBehaviour
{
    [Header("Danger Effect Settings")]
    [Tooltip("Color that the camera flickers to (e.g., red for a horror effect).")]
    public Color dangerColor = Color.red;
    [Tooltip("Minimum interval between flickers (seconds).")]
    public float flickerIntervalMin = 0.1f;
    [Tooltip("Maximum interval between flickers (seconds).")]
    public float flickerIntervalMax = 0.5f;
    [Tooltip("Duration of each flicker (seconds).")]
    public float flickerDuration = 0.1f;

    private Camera cam;
    private Color originalColor;
    private bool isDangerActive = false;
    private Coroutine flickerCoroutine;

    private void Start()
    {
        cam = GetComponent<Camera>();
        if (cam != null)
        {
            originalColor = cam.backgroundColor;
        }
    }

    /// <summary>
    /// Starts the danger effect.
    /// </summary>
    public void StartDangerEffect()
    {
        if (isDangerActive)
            return;

        isDangerActive = true;
        flickerCoroutine = StartCoroutine(DangerFlicker());
    }

    /// <summary>
    /// Stops the danger effect and restores the original camera look.
    /// </summary>
    public void StopDangerEffect()
    {
        if (!isDangerActive)
            return;

        isDangerActive = false;
        if (flickerCoroutine != null)
            StopCoroutine(flickerCoroutine);
        if (cam != null)
            cam.backgroundColor = originalColor;
    }

    /// <summary>
    /// Coroutine that causes the camera to flicker to a danger color.
    /// </summary>
    private IEnumerator DangerFlicker()
    {
        while (isDangerActive)
        {
            // Wait for a random interval between flickers.
            float waitTime = Random.Range(flickerIntervalMin, flickerIntervalMax);
            yield return new WaitForSeconds(waitTime);

            // Change to danger color.
            if (cam != null)
            {
                cam.backgroundColor = dangerColor;
            }
            yield return new WaitForSeconds(flickerDuration);

            // Restore original background color.
            if (cam != null)
            {
                cam.backgroundColor = originalColor;
            }
        }
    }
}
