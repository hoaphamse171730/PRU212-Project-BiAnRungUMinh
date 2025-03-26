using UnityEngine;

public class BoatBoardingTrigger : MonoBehaviour
{
    private BoatController boatController;
    private bool playerInRange = false;
    private Transform playerTransform;

    // Optional: assign a UI prompt that appears when the player can board.
    [SerializeField] private GameObject boardPromptUI;

    private void Start()
    {
        // Ensure the BoatBoardingTrigger is a child of the boat that has BoatController.
        boatController = GetComponentInParent<BoatController>();
        if (boardPromptUI != null)
        {
            boardPromptUI.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerTransform = other.transform;
            if (boardPromptUI != null)
            {
                boardPromptUI.SetActive(true);
            }
            Debug.Log("Player entered boarding area.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerTransform = null;
            if (boardPromptUI != null)
            {
                boardPromptUI.SetActive(false);
            }
            Debug.Log("Player left boarding area.");
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerTransform != null)
            {
                Debug.Log("E pressed. Attempting to board boat.");
                boatController.BoardBoat(playerTransform);
            }
            if (boardPromptUI != null)
            {
                boardPromptUI.SetActive(false);
            }
        }
    }
}
