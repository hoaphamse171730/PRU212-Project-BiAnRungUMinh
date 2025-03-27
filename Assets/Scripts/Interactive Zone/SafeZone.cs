using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public float safeLightRestoreMultiplier = 1.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DarknessController darkness = FindObjectOfType<DarknessController>();
            if (darkness != null)
            {
                darkness.EnterSafeZone(safeLightRestoreMultiplier);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // If the player is on a boat, skip turning off safe mode.
            if (other.transform.parent != null && other.transform.parent.GetComponent<BoatController>() != null)
            {
                return;
            }

            DarknessController darkness = FindObjectOfType<DarknessController>();
            if (darkness != null)
            {
                darkness.ExitSafeZone();
            }
        }
    }
}
