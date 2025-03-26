using UnityEngine;
 // Required for Light2D

public class DarknessController : MonoBehaviour
{
    [Header("References")]
    public Transform player;          // Player's transform
    public UnityEngine.Rendering.Universal.Light2D darknessLight;     // Replace SpriteRenderer with Light2D

    [Header("Positioning")]
    public Vector2 offset = Vector2.zero; // Offset from player's position

    [Header("Light Settings")]
    public float maxLightRadius = 10f;     // Maximum light radius (vision range)
    public float minLightRadius = 0f;   // Minimum radius (when the player dies)
    public float lightDrainRate = 1f;     // How fast the light shrinks per second
    public float lightRestoreRate = 10f;   // How fast the light regenerates per second when safe
    private float currentLightRadius;

    [Header("Twitch Effect Settings")]
    public bool enableTwitch = true;
    [Tooltip("How much the light should twitch (in world units).")]
    public float twitchAmplitude = 2f;
    [Tooltip("How fast the twitch effect oscillates.")]
    public float twitchFrequency = 5f;

    [Tooltip("Multiply the drain rate when in a danger zone.")]
    public float dangerDrainMultiplier = 3f;
    private bool isDanger = false;

    private bool isPlayerDeadFromDarkness = false;
    private bool isSafe = false;
    private float safeZoneMultiplier = 1f;

    private void Start()
    {
        // Start with full "vision"
        currentLightRadius = maxLightRadius;
    }

    private void Update()
    {
        if (player == null || isPlayerDeadFromDarkness)
            return;

        // Handle light restoration in safe mode or light drain normally
        if (isSafe)
        {
            currentLightRadius += lightRestoreRate * safeZoneMultiplier * Time.deltaTime;
        }
        else
        {
            float drainRate = lightDrainRate;
            if (isDanger)
                drainRate *= dangerDrainMultiplier;
            currentLightRadius -= drainRate * Time.deltaTime;
        }

        currentLightRadius = Mathf.Clamp(currentLightRadius, minLightRadius, maxLightRadius);

        // Update the Light2D component to match the current light radius.
        if (darknessLight != null)
        {
            // Adjust the outer radius of the light instead of scaling a sprite.
            darknessLight.pointLightOuterRadius = currentLightRadius;
        }

        // Position the light at the player's position plus offset.
        Vector3 newPos = player.position;
        newPos.x += offset.x;
        newPos.y += offset.y;
        newPos.z = transform.position.z; // Maintain correct Z for rendering

        // Optionally add a twitch effect to the light's position.
        if (enableTwitch)
        {
            float noiseX = (Mathf.PerlinNoise(Time.time * twitchFrequency, 0.0f) - 0.5f) * 2f;
            float noiseY = (Mathf.PerlinNoise(0.0f, Time.time * twitchFrequency) - 0.5f) * 2f;
            Vector3 twitchOffset = new Vector3(noiseX * twitchAmplitude, noiseY * twitchAmplitude, 0f);
            newPos += twitchOffset;
        }

        transform.position = newPos;

        // Check if the light has diminished too far.
        if (currentLightRadius <= minLightRadius)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null && !pc.IsDead)
            {
                pc.TakeDamage(9999); // Trigger player death.
                isPlayerDeadFromDarkness = true;
            }
        }
    }

    // Set safe mode with an optional multiplier to adjust restoration speed.
    public void SetSafe(bool safeState, float multiplier = 1f)
    {
        isSafe = safeState;
        safeZoneMultiplier = multiplier;
    }

    // Toggle danger mode to increase light drain.
    public void SetDanger(bool dangerState)
    {
        isDanger = dangerState;
    }

    // Immediately restore a certain amount of light.
    public void RestoreLight(float amount)
    {
        currentLightRadius += amount;
        if (currentLightRadius > maxLightRadius)
            currentLightRadius = maxLightRadius;
    }
    public void ResetLight()
    {
        currentLightRadius = maxLightRadius;
        isPlayerDeadFromDarkness = false;
        if (darknessLight != null)
        {
            darknessLight.pointLightOuterRadius = currentLightRadius;
        }
    }

}
