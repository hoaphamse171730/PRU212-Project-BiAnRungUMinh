using UnityEngine;

public class SafeZone : MonoBehaviour
{
    // This script expects the GameObject to have a 2D Collider (set as Trigger).

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player (ensure your Player GameObject is tagged "Player").
        if (other.CompareTag("Player"))
        {
            // Find the DarknessController in the scene.
            DarknessController darkness = FindObjectOfType<DarknessController>();
            if (darkness != null)
            {
                darkness.SetSafe(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // When the player leaves the safe zone, resume normal light drain.
        if (other.CompareTag("Player"))
        {
            DarknessController darkness = FindObjectOfType<DarknessController>();
            if (darkness != null)
            {
                darkness.SetSafe(false);
            }
        }
    }
}
