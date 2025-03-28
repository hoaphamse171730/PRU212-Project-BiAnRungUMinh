using UnityEngine;
using UnityEngine.UI; // Only needed if you are working with UI

public class ButtonSound : MonoBehaviour
{
    [Header("Button Sound Settings")]
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private float volume = 1f;

    private AudioSource audioSource;

    private void Awake()
    {
        // Get or add an AudioSource component.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // This method can be hooked up to a UI Button's OnClick event.
    public void PlayButtonSound()
    {
        if (buttonClickClip != null)
        {
            audioSource.PlayOneShot(buttonClickClip, volume);
        }
    }
}
