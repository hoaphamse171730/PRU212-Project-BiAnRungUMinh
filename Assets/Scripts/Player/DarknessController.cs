using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DarknessController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Light2D darknessLight;

    [Header("Positioning")]
    public Vector2 offset = Vector2.zero;

    [Header("Light Settings")]
    public float maxLightRadius = 10f;
    public float minLightRadius = 0f;
    public float lightDrainRate = 1f;
    public float lightRestoreRate = 10f;
    private float currentLightRadius;

    [Header("Twitch Effect Settings")]
    public bool enableTwitch = true;
    public float twitchAmplitude = 2f;
    public float twitchFrequency = 5f;

    [Tooltip("Multiply the drain rate when in a danger zone.")]
    public float dangerDrainMultiplier = 3f;
    private bool isDanger = false;

    private bool isPlayerDeadFromDarkness = false;
    private bool isSafe = false;
    private float safeZoneMultiplier = 1f;

    // Safe zone counter
    public int safeZoneCounter = 0;

    private void Start()
    {
        currentLightRadius = maxLightRadius;
    }

    private void Update()
    {
        if (player == null || isPlayerDeadFromDarkness)
            return;

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

        if (darknessLight != null)
        {
            darknessLight.pointLightOuterRadius = currentLightRadius;
        }

        Vector3 newPos = player.position + (Vector3)offset;
        newPos.z = transform.position.z;

        if (enableTwitch)
        {
            float noiseX = (Mathf.PerlinNoise(Time.time * twitchFrequency, 0.0f) - 0.5f) * 2f;
            float noiseY = (Mathf.PerlinNoise(0.0f, Time.time * twitchFrequency) - 0.5f) * 2f;
            newPos += new Vector3(noiseX * twitchAmplitude, noiseY * twitchAmplitude, 0f);
        }

        transform.position = newPos;

        if (currentLightRadius <= minLightRadius)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null && !pc.IsDead)
            {
                pc.TakeDamage(9999);
                isPlayerDeadFromDarkness = true;
            }
        }
    }

    // Instead of directly setting safe mode, manage a counter.
    public void EnterSafeZone(float multiplier)
    {
        safeZoneCounter++;
        // Optionally, you can average or choose the highest multiplier among overlapping zones.
        safeZoneMultiplier = multiplier;
        SetSafe(true, safeZoneMultiplier);
    }

    public void ExitSafeZone()
    {
        safeZoneCounter--;
        if (safeZoneCounter <= 0)
        {
            safeZoneCounter = 0;
            SetSafe(false);
        }
    }

    // Original methods that are called internally.
    public void SetSafe(bool safeState, float multiplier = 1f)
    {
        isSafe = safeState;
        safeZoneMultiplier = multiplier;
    }

    public void SetDanger(bool dangerState)
    {
        isDanger = dangerState;
    }

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
