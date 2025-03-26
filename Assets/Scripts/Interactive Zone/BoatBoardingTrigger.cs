using UnityEngine;

public class BoatBoardingTrigger : MonoBehaviour
{
    private BoatController boatController;

    private bool playerInRange = false;

    [SerializeField] private GameObject boardPromptUI;

    private void Start()
    {
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
            if (boardPromptUI != null)
            {
                boardPromptUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (boardPromptUI != null)
            {
                boardPromptUI.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            boatController.BoardBoat(GameObject.FindGameObjectWithTag("Player").transform);
            if (boardPromptUI != null)
            {
                boardPromptUI.SetActive(false);
            }
        }
    }
}
