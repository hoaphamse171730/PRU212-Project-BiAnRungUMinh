using System.Collections;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [Header("Boat Movement")]
    [SerializeField] private float boatSpeed = 5f;

    [Header("Boarding Settings")]
    [Tooltip("Offset to position the player above the boat once boarded")]
    [SerializeField] private Vector3 boardingOffset = new Vector3(0f, 0.3f, 0f);

    [SerializeField] private KeyCode disembarkKey = KeyCode.Q;

    private bool isBoarded = false;
    private Transform boardedPlayer;

    void Update()
    {
        if (isBoarded)
        {
            float moveInput = Input.GetAxis("Horizontal");
            Vector3 movement = Vector3.right * moveInput * boatSpeed * Time.deltaTime;
            transform.Translate(movement);

            if (Input.GetKeyDown(disembarkKey))
            {
                DisembarkBoat();
            }
        }
    }

    public void BoardBoat(Transform playerTransform)
    {
        if (!isBoarded)
        {
            Debug.Log("BoardBoat called. Boarding the player.");
            isBoarded = true;
            boardedPlayer = playerTransform;
            boardedPlayer.SetParent(transform);
            boardedPlayer.localPosition = boardingOffset;

            // Disable the player's movement controller.
            PlayerController pc = boardedPlayer.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.enabled = false;
                Debug.Log("PlayerController disabled.");
            }

            // Force the player's animation to idle.
            Animator playerAnim = boardedPlayer.GetComponent<Animator>();
            if (playerAnim != null)
            {
                playerAnim.ResetTrigger("Hurt");
                playerAnim.ResetTrigger("Dead");
                playerAnim.Play("idle", 0, 0f);
                playerAnim.Update(0f);
                Debug.Log("Player animation set to Idle.");
            }

            // Stop any residual movement.
            Rigidbody2D playerRb = boardedPlayer.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
                playerRb.bodyType = RigidbodyType2D.Kinematic;
                Debug.Log("Player Rigidbody set to Kinematic.");
            }

            // Protect the player from darkness.
            DarknessController darkness = FindObjectOfType<DarknessController>();
            if (darkness != null)
            {
                darkness.SetSafe(true, 1f); // Adjust the multiplier as needed.
                Debug.Log("Darkness set to safe mode on boat.");
            }

            Debug.Log("Player is now parented to the boat and positioned above it.");
        }
    }

    public void DisembarkBoat()
    {
        if (isBoarded && boardedPlayer != null)
        {
            // Re-enable the player's controller.
            PlayerController pc = boardedPlayer.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.enabled = true;
                Debug.Log("PlayerController re-enabled.");
            }

            // Revert the player's Rigidbody2D to Dynamic.
            Rigidbody2D playerRb = boardedPlayer.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.bodyType = RigidbodyType2D.Dynamic;
                Debug.Log("Player Rigidbody set to Dynamic.");
            }

            // Store reference to the player before unparenting.
            Transform disembarkingPlayer = boardedPlayer;

            // Remove the parent so the player is independent again.
            boardedPlayer.SetParent(null);
            boardedPlayer = null;
            isBoarded = false;
            Debug.Log("Player disembarked from the boat.");

            // Update the DarknessController's player reference,
            // disable safe mode and reset safe zone counter.
            DarknessController darkness = FindObjectOfType<DarknessController>();
            if (darkness != null)
            {
                darkness.player = disembarkingPlayer;
                StartCoroutine(DisableSafeModeDelayed(darkness));
            }
        }
    }
    IEnumerator DisableSafeModeDelayed(DarknessController darkness)
    {
        yield return null; // Wait one frame.
        darkness.SetSafe(false);
        darkness.safeZoneCounter = 0;
        Debug.Log("Darkness safe mode disabled after delay.");
    }

}
