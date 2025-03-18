using UnityEngine;

public class DarknessController : MonoBehaviour
{
    [Header("References")]
    public Transform player;                // Assign your Player's transform here
    public SpriteRenderer darknessRenderer; // The SpriteRenderer with the darkness sprite

    [Header("Positioning")]
    public Vector2 offset = Vector2.zero;   // Offset from player's position

    [Header("Light Settings")]
    public float maxLightRadius = 5f;       // Maximum "vision" size
    public float minLightRadius = 0.1f;     // Minimum size (when the player dies)
    public float lightDrainRate = 1f;       // How fast the light shrinks per second
    public float lightRestoreRate = 1f;     // How fast the light regenerates per second when safe
    private float currentLightRadius;

    [Header("Twitch Effect Settings")]
    public bool enableTwitch = true;        // Enable/disable the twitch effect
    [Tooltip("How much the darkness overlay should twitch (in world units).")]
    public float twitchAmplitude = 0.1f;
    [Tooltip("How fast the twitch effect oscillates.")]
    public float twitchFrequency = 2f;

    [Tooltip("Multiply the drain rate when in a danger zone.")]
    public float dangerDrainMultiplier = 2f;
    private bool isDanger = false;

    private bool isPlayerDeadFromDarkness = false;
    private bool isSafe = false;  // When true, light drain is paused and light regenerates

    private void Start()
    {
        // Start with full "vision"
        currentLightRadius = maxLightRadius;
    }

    private void Update()
    {
        if (player == null || isPlayerDeadFromDarkness) return;

        // Regenerate or drain light based on safe state.
        if (isSafe)
        {
            currentLightRadius += lightRestoreRate * Time.deltaTime;
        }
        else
        {
            // Apply danger multiplier if danger mode is active.
            float drainRate = lightDrainRate;
            if (isDanger)
            {
                drainRate *= dangerDrainMultiplier;
            }
            currentLightRadius -= drainRate * Time.deltaTime;
        }

        currentLightRadius = Mathf.Clamp(currentLightRadius, minLightRadius, maxLightRadius);

        // Update the darkness sprite scale to match the current light radius.
        if (darknessRenderer != null)
        {
            darknessRenderer.transform.localScale = new Vector3(currentLightRadius, currentLightRadius, 1);
        }

        // Base position is player's position plus the fixed offset.
        Vector3 newPos = player.position;
        newPos.x += offset.x;
        newPos.y += offset.y;
        newPos.z = transform.position.z; // Keep the same Z so it's rendered in front.

        // Add a twitch effect using Perlin noise for smooth randomness.
        if (enableTwitch)
        {
            float noiseX = (Mathf.PerlinNoise(Time.time * twitchFrequency, 0.0f) - 0.5f) * 2f;
            float noiseY = (Mathf.PerlinNoise(0.0f, Time.time * twitchFrequency) - 0.5f) * 2f;
            Vector3 twitchOffset = new Vector3(noiseX * twitchAmplitude, noiseY * twitchAmplitude, 0f);
            newPos += twitchOffset;
        }

        transform.position = newPos;

        // If the light reaches the minimum, kill the player.
        if (currentLightRadius <= minLightRadius)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null && !pc.IsDead)
            {
                pc.TakeDamage(9999); // Enough to kill the player.
                isPlayerDeadFromDarkness = true;
            }
        }
    }

    // Toggle safe mode to pause draining and allow regeneration.
    public void SetSafe(bool safeState)
    {
        isSafe = safeState;
    }

    // Toggle danger mode (increasing the drain rate)
    public void SetDanger(bool dangerState)
    {
        isDanger = dangerState;
    }

    // External call to restore light immediately (e.g., from torches, NPCs, or items).
    public void RestoreLight(float amount)
    {
        currentLightRadius += amount;
        if (currentLightRadius > maxLightRadius)
            currentLightRadius = maxLightRadius;
    }
}
