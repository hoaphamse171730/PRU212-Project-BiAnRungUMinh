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
            DarknessController darkness = FindObjectOfType<DarknessController>();
            if (darkness != null)
            {
                // Enable safe mode.
                darkness.SetSafe(true, safeLightRestoreMultiplier);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.parent != null && other.transform.parent.GetComponent<BoatController>() != null)
            {
                return;
            }

            DarknessController darkness = FindObjectOfType<DarknessController>();
            if (darkness != null)
            {
                // Turn off safe mode.
                darkness.SetSafe(false);
            }
        }
    }
}
