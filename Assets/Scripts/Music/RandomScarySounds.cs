using UnityEngine;
using System.Collections;

public class RandomScarySounds : MonoBehaviour
{
    // Array of scary sound clips (assign these via the Inspector)
    public AudioClip[] scaryClips;

    // Minimum and maximum delay between sounds (in seconds)
    public float minDelay = 5f;
    public float maxDelay = 20f;

    // Duration each sound will play (in seconds)
    public float clipPlayDuration = 5f;

    // Reference to the AudioSource component
    private AudioSource audioSource;

    // Store last played clip index
    private int lastClipIndex = -1;

    void Start()
    {
        // Get or add an AudioSource component on this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set AudioSource settings
        audioSource.loop = false;
        audioSource.spatialBlend = 0f; // 0 for 2D sound; adjust if you need 3D
        audioSource.volume = 0.3f;       // Set volume to 0.5

        // Start the coroutine to play sounds at random intervals
        StartCoroutine(PlayRandomScarySounds());
    }

    IEnumerator PlayRandomScarySounds()
    {
        while (true)
        {
            // Wait for a random delay between minDelay and maxDelay
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            // Ensure we have at least one clip assigned
            if (scaryClips.Length > 0)
            {
                int randomIndex = 0;
                // If more than one clip available, choose one that is different from the last played
                if (scaryClips.Length > 1)
                {
                    do
                    {
                        randomIndex = Random.Range(0, scaryClips.Length);
                    } while (randomIndex == lastClipIndex);
                }
                else
                {
                    randomIndex = 0;
                }
                lastClipIndex = randomIndex;
                AudioClip clip = scaryClips[randomIndex];

                // Assign and play the clip
                audioSource.clip = clip;
                audioSource.Play();

                // Wait for clipPlayDuration seconds, then stop the sound
                yield return new WaitForSeconds(clipPlayDuration);
                audioSource.Stop();
            }
        }
    }
}
