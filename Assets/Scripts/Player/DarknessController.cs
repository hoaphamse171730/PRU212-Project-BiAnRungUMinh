using UnityEngine;

public class DarknessController : MonoBehaviour
{
    [Header("References")]
    public Transform player;                // Assign your Player's transform here
    public SpriteRenderer darknessRenderer; // The SpriteRenderer with the black circle

    [Header("Positioning")]
    public Vector2 offset = Vector2.zero;   // Offset from player's position

    [Header("Light Settings")]
    public float maxLightRadius = 5f;       // Maximum "vision" size
    public float minLightRadius = 0.1f;     // Minimum size (at which the player dies)
    public float lightDrainRate = 1f;       // How fast the light shrinks per second
    public float lightRestoreRate = 1f;     // How fast the light regenerates per second when safe
    private float currentLightRadius;

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

        // If in a safe zone, regenerate light; otherwise, drain it.
        if (isSafe)
        {
            currentLightRadius += lightRestoreRate * Time.deltaTime;
        }
        else
        {
            currentLightRadius -= lightDrainRate * Time.deltaTime;
        }

        // Clamp the current light between the minimum and maximum values.
        currentLightRadius = Mathf.Clamp(currentLightRadius, minLightRadius, maxLightRadius);

        // Scale the sprite to match the current light radius.
        if (darknessRenderer != null)
        {
            darknessRenderer.transform.localScale = new Vector3(currentLightRadius, currentLightRadius, 1);
        }

        // Position the darkness overlay at the player's position plus the offset.
        Vector3 newPos = player.position;
        newPos.x += offset.x;
        newPos.y += offset.y;
        newPos.z = transform.position.z;
        transform.position = newPos;

        // If the light reaches the minimum, kill the player.
        if (currentLightRadius <= minLightRadius)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null && !pc.IsDead)
            {
                pc.TakeDamage(9999); // Sufficient to kill the player.
                isPlayerDeadFromDarkness = true;
            }
        }
    }

    // Method for other scripts to toggle safe mode.
    public void SetSafe(bool safeState)
    {
        isSafe = safeState;
    }

    // Call this to restore light (e.g., from torches, NPCs, or items).
    public void RestoreLight(float amount)
    {
        currentLightRadius += amount;
        if (currentLightRadius > maxLightRadius)
            currentLightRadius = maxLightRadius;
    }
}
