using UnityEngine;

public class DangerZone : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip dangerMusicClip;
    public AudioClip dangerSoundClip;
    public float volume = 1f;

    private AudioSource audioSource;

    private void Start()
    {
        // Use or add an AudioSource component.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.volume = volume;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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

            // Trigger any camera or UI flicker effects.
            CameraDangerEffect camEffect = Camera.main.GetComponent<CameraDangerEffect>();
            if (camEffect != null)
            {
                camEffect.StartDangerEffect();
            }

            ScreenFlickerEffect flicker = FindObjectOfType<ScreenFlickerEffect>();
            if (flicker != null)
            {
                flicker.StartFlicker();
            }

            // Set danger mode on DarknessController.
            DarknessController dc = FindObjectOfType<DarknessController>();
            if (dc != null)
            {
                dc.SetDanger(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            CameraDangerEffect camEffect = Camera.main.GetComponent<CameraDangerEffect>();
            if (camEffect != null)
            {
                camEffect.StopDangerEffect();
            }

            ScreenFlickerEffect flicker = FindObjectOfType<ScreenFlickerEffect>();
            if (flicker != null)
            {
                flicker.StopFlicker();
            }

            // Disable danger mode.
            DarknessController dc = FindObjectOfType<DarknessController>();
            if (dc != null)
            {
                dc.SetDanger(false);
            }
        }
    }
}
