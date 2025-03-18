using UnityEngine;

public class DarknessController : MonoBehaviour
{
    [Header("References")]
    public Transform player;                // Assign your Player's transform here
    public SpriteRenderer darknessRenderer; // The SpriteRenderer with the black circle

    [Header("Positioning")]
    public Vector2 offset = Vector2.zero;   // Offset from player's position

    [Header("Light Settings")]
    public float maxLightRadius = 5f;       // Starting size of the "vision"
    public float minLightRadius = 0.1f;     // Minimum size (when it hits this, you're "in darkness")
    public float lightDrainRate = 1f;       // How fast the vision shrinks per second
    private float currentLightRadius;

    private bool isPlayerDeadFromDarkness = false;

    private void Start()
    {
        // Start with full "vision"
        currentLightRadius = maxLightRadius;
    }

    private void Update()
    {
        if (player == null || isPlayerDeadFromDarkness) return;

        // 1) Decrease the light radius over time
        currentLightRadius -= lightDrainRate * Time.deltaTime;
        currentLightRadius = Mathf.Clamp(currentLightRadius, minLightRadius, maxLightRadius);

        // 2) Scale the sprite to match the current radius
        if (darknessRenderer != null)
        {
            darknessRenderer.transform.localScale = new Vector3(currentLightRadius, currentLightRadius, 1);
        }

        // 3) Position the darkness overlay at the player's position + offset
        Vector3 newPos = player.position;
        newPos.x += offset.x;
        newPos.y += offset.y;
        newPos.z = transform.position.z; // keep the same Z so it stays in front of everything
        transform.position = newPos;

        // 4) Check if vision is gone => kill the player
        if (currentLightRadius <= minLightRadius)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null && !pc.IsDead)
            {
                pc.TakeDamage(9999); // Enough to kill them
                isPlayerDeadFromDarkness = true;
            }
        }
    }

    // Call this from a torch pickup to restore the light
    public void RestoreLight(float amount)
    {
        currentLightRadius += amount;
        if (currentLightRadius > maxLightRadius)
        {
            currentLightRadius = maxLightRadius;
        }
    }
}
