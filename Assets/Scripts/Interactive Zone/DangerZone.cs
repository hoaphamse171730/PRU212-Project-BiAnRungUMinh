using UnityEngine;
using System.Collections;

public class DangerZone : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip dangerMusicClip;
    public AudioClip dangerSoundClip;
    public float volume = 1f;
    [Tooltip("Duration for fading out audio on exit.")]
    public float audioFadeDuration = 1f;

    [Header("References")]
    [Tooltip("Assign a reference to your CameraDangerEffect if available.")]
    public CameraDangerEffect cameraDangerEffect;
    [Tooltip("Assign a reference to your ScreenFlickerEffect if available.")]
    public ScreenFlickerEffect screenFlickerEffect;
    [Tooltip("Assign a reference to your DarknessController if available.")]
    public DarknessController darknessController;

    private AudioSource audioSource;
    private bool isInDangerZone = false;

    private void Awake()
    {
        // Get or add AudioSource and set initial properties.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.volume = volume;

        // If not assigned in the Inspector, try to find the components.
        if (cameraDangerEffect == null && Camera.main != null)
        {
            cameraDangerEffect = Camera.main.GetComponent<CameraDangerEffect>();
        }
        if (screenFlickerEffect == null)
        {
            screenFlickerEffect = FindObjectOfType<ScreenFlickerEffect>();
        }
        if (darknessController == null)
        {
            darknessController = FindObjectOfType<DarknessController>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        isInDangerZone = true;

        // Play danger music.
        if (dangerMusicClip != null)
        {
            audioSource.clip = dangerMusicClip;
            audioSource.Play();
        }

        // Play a one-shot danger sound effect.
        if (dangerSoundClip != null)
        {
            audioSource.PlayOneShot(dangerSoundClip);
        }

        // Trigger camera danger effect.
        if (cameraDangerEffect != null)
        {
            cameraDangerEffect.StartDangerEffect();
        }

        // Trigger screen flicker effect.
        if (screenFlickerEffect != null)
        {
            screenFlickerEffect.StartFlicker();
        }

        // Enable danger mode on DarknessController.
        if (darknessController != null)
        {
            darknessController.SetDanger(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        isInDangerZone = false;

        // Fade out audio instead of an abrupt stop.
        if (audioSource.isPlaying)
        {
            StartCoroutine(FadeOutAudio(audioFadeDuration));
        }

        // Stop camera danger effect.
        if (cameraDangerEffect != null)
        {
            cameraDangerEffect.StopDangerEffect();
        }

        // Stop screen flicker effect.
        if (screenFlickerEffect != null)
        {
            screenFlickerEffect.StopFlicker();
        }

        // Disable danger mode on DarknessController.
        if (darknessController != null)
        {
            darknessController.SetDanger(false);
        }
    }

    private IEnumerator FadeOutAudio(float duration)
    {
        float startVolume = audioSource.volume;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }
        audioSource.Stop();
        // Reset volume for the next time danger is entered.
        audioSource.volume = volume;
    }
}
