using UnityEngine;

public class BoatController : MonoBehaviour
{
    [Header("Boat Movement")]
    [SerializeField] private float boatSpeed = 5f;

    private bool isBoarded = false;
    private Transform boardedPlayer;

    void Update()
    {
        if (isBoarded)
        {
            // Control boat movement with left/right arrow keys.
            float moveInput = Input.GetAxis("Horizontal");
            Vector3 movement = Vector3.right * moveInput * boatSpeed * Time.deltaTime;
            transform.Translate(movement);

            // (Optional) If you want to allow disembarking on key press:
            /*
            if (Input.GetKeyDown(KeyCode.E))
            {
                DisembarkBoat();
            }
            */
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

            // Disable the player's own movement controller.
            PlayerController pc = boardedPlayer.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.enabled = false;
                Debug.Log("PlayerController disabled.");
            }
            Debug.Log("Player is now parented to the boat.");
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
            boardedPlayer.SetParent(null);
            boardedPlayer = null;
            isBoarded = false;
            Debug.Log("Player disembarked from the boat.");
        }
    }
}
