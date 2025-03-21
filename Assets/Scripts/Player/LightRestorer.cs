using UnityEngine;

public class LightRestorer : MonoBehaviour
{
    [Header("Restore Settings")]
    [Tooltip("How much light to restore when activated.")]
    public float restoreAmount = 3f;
    [Tooltip("If true, this object will be destroyed after restoring light.")]
    public bool destroyAfterRestore = true;
    [Tooltip("If true, the player must press a key (E) to interact and restore light.")]
    public bool requiresInteraction = false;

    // Reference to the DarknessController in the scene.
    private DarknessController darknessController;
    // Tracks if the player is within trigger range.
    private bool playerInRange = false;

    private void Start()
    {
        // Find the DarknessController instance in the scene.
        darknessController = FindObjectOfType<DarknessController>();
        if (darknessController == null)
        {
            Debug.LogWarning("No DarknessController found in the scene. Please add one.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player (make sure your Player GameObject has the tag "Player")
        if (other.CompareTag("Player"))
        {
            if (!requiresInteraction)
            {
                RestoreLight();
            }
            else
            {
                // For interaction, flag that the player is in range.
                playerInRange = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        // If interaction is required and the player is in range, listen for the interact key.
        if (requiresInteraction && playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                RestoreLight();
            }
        }
    }

    private void RestoreLight()
    {
        if (darknessController != null)
        {
            darknessController.RestoreLight(restoreAmount);
        }
        else
        {
            Debug.LogWarning("DarknessController not found. Cannot restore light.");
        }

        // Optionally, destroy the object after use (for one-time pickups).
        if (destroyAfterRestore)
        {
            Destroy(gameObject);
        }
    }
}
