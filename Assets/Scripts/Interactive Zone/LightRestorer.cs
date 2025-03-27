using UnityEngine;
using System.Collections;

public class LightRestorer : MonoBehaviour
{
    [Header("Restore Settings")]
    [Tooltip("Total amount of light to restore when activated.")]
    public float restoreAmount = 3f;
    [Tooltip("Duration (in seconds) over which the light is restored.")]
    public float restoreDuration = 0.2f;
    [Tooltip("If true, this object will be destroyed after restoring light.")]
    public bool destroyAfterRestore = true;
    [Tooltip("If true, the player must press a key (E) to interact and restore light.")]
    public bool requiresInteraction = false;

    // Reference to the DarknessController in the scene.
    private DarknessController darknessController;
    // Tracks if the player is within trigger range.
    private bool playerInRange = false;
    // Flag to ensure restoration starts only once.
    private bool restorationStarted = false;

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
        // Check if the collider belongs to the player.
        if (other.CompareTag("Player"))
        {
            if (!requiresInteraction)
            {
                TriggerRestore();
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
        // If interaction is required and the player is in range, listen for the "E" key.
        if (requiresInteraction && playerInRange && !restorationStarted)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TriggerRestore();
            }
        }
    }

    private void TriggerRestore()
    {
        if (!restorationStarted)
        {
            restorationStarted = true;
            StartCoroutine(GradualRestore());
        }
    }

    private IEnumerator GradualRestore()
    {
        // Instantly remove the sprite by disabling the SpriteRenderer.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = false;
        }

        // Optionally disable the collider to prevent further triggers.
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        float elapsedTime = 0f;
        while (elapsedTime < restoreDuration)
        {
            float increment = (restoreAmount / restoreDuration) * Time.deltaTime;
            if (darknessController != null)
            {
                darknessController.RestoreLight(increment);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure any remaining amount is applied.
        float totalRestored = (restoreAmount / restoreDuration) * elapsedTime;
        float remainder = restoreAmount - totalRestored;
        if (remainder > 0 && darknessController != null)
        {
            darknessController.RestoreLight(remainder);
        }

        if (destroyAfterRestore)
        {
            Destroy(gameObject);
        }
    }
}
