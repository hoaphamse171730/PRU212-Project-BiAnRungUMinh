using UnityEngine;
using UnityEngine.UI;

public class TalkPrompt : MonoBehaviour
{
    [Tooltip("UI element that displays the 'Talk' prompt")]
    public GameObject talkPromptUI;

    private DialogueTrigger dialogueTrigger;

    private bool playerInRange;

    private void Start()
    {
        if (talkPromptUI != null)
            talkPromptUI.SetActive(false);

        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (talkPromptUI != null)
                talkPromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            if (talkPromptUI != null)
                talkPromptUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueTrigger != null)
                dialogueTrigger.TriggerDialogue();

            if (talkPromptUI != null)
                talkPromptUI.SetActive(false);
        }
    }
}
