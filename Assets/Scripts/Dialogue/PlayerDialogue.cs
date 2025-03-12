using UnityEngine;

public class PlayerDialogue : MonoBehaviour
{
    private DialogueTrigger currentDialogueTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        DialogueTrigger dt = other.GetComponent<DialogueTrigger>();
        if (dt != null)
        {
            currentDialogueTrigger = dt;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        DialogueTrigger dt = other.GetComponent<DialogueTrigger>();
        if (dt != null && dt == currentDialogueTrigger)
        {
            currentDialogueTrigger = null;
            if (DialogueManager.Instance != null)
                DialogueManager.Instance.EndDialogue();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
            {
                DialogueManager.Instance.DisplayNextSentence();
            }
            else if (currentDialogueTrigger != null)
            {
                currentDialogueTrigger.TriggerDialogue();
            }
        }
    }
}
