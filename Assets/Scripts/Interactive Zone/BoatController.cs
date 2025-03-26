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

            PlayerController pc = boardedPlayer.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.enabled = false;
                Debug.Log("PlayerController disabled.");
            }

            Animator playerAnim = boardedPlayer.GetComponent<Animator>();
            if (playerAnim != null)
            {
                playerAnim.ResetTrigger("Hurt");
                playerAnim.ResetTrigger("Dead");
                playerAnim.Play("idle", 0, 0f);
                playerAnim.Update(0f);
                Debug.Log("Player animation set to Idle.");
            }
                Rigidbody2D playerRb = boardedPlayer.GetComponent<Rigidbody2D>();

                if (playerRb != null)
                {
                    playerRb.linearVelocity = Vector2.zero;
                    playerRb.bodyType = RigidbodyType2D.Kinematic;
                    Debug.Log("Player Rigidbody set to Kinematic.");
                }
            
            Debug.Log("Player is now parented to the boat and positioned above it.");
        }
    }

    public void DisembarkBoat()
    {
        if (isBoarded && boardedPlayer != null)
        {
            PlayerController pc = boardedPlayer.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.enabled = true;
                Debug.Log("PlayerController re-enabled.");
            }
            Rigidbody2D playerRb = boardedPlayer.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.bodyType = RigidbodyType2D.Dynamic;
                Debug.Log("Player Rigidbody set to Dynamic.");
            }
            boardedPlayer.SetParent(null);
            boardedPlayer = null;
            isBoarded = false;
            Debug.Log("Player disembarked from the boat.");
        }
    }
}
