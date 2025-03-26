using UnityEngine;

public class SafeZone : MonoBehaviour
{
    // Public number to adjust the light restoration multiplier in this SafeZone.
    public float safeLightRestoreMultiplier = 1.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player.
        if (other.CompareTag("Player"))
        {
            // Find the DarknessController in the scene.
            DarknessController darkness = FindObjectOfType<DarknessController>();
            if (darkness != null)
            {
                // Pass the multiplier to adjust how fast light is restored.
                darkness.SetSafe(true, safeLightRestoreMultiplier);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // When the player leaves, reset safe mode.
        if (other.CompareTag("Player"))
        {
            DarknessController darkness = FindObjectOfType<DarknessController>();
            if (darkness != null)
            {
                // Reset safe mode; multiplier will default to 1.
                darkness.SetSafe(false);
            }
        }
    }
}

