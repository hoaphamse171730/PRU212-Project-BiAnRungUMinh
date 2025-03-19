using UnityEngine;
using UnityEngine.UI;

public class FadeInText : MonoBehaviour
{
    [SerializeField] private CanvasGroup textCanvasGroup;
    [SerializeField] private float fadeDuration = 2.0f; // Duration of the fade-in in seconds

    void Start()
    {
        // Ensure the CanvasGroup is assigned
        if (textCanvasGroup == null)
        {
            textCanvasGroup = GetComponent<CanvasGroup>();
            if (textCanvasGroup == null)
            {
                Debug.LogError("CanvasGroup component is missing on " + gameObject.name + "!");
                return;
            }
        }

        // Start the fade-in coroutine
        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        // Start with fully transparent
        textCanvasGroup.alpha = 0f;

        // Gradually increase alpha to 1 over fadeDuration
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            textCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        // Ensure it reaches fully opaque
        textCanvasGroup.alpha = 1f;
    }
}