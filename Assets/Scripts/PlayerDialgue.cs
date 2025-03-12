using UnityEngine;

public class PlayerDialogue : MonoBehaviour
{
    // Reference to the dialogue trigger that the player is near.
    private DialogueTrigger currentDialogueTrigger;

    // When the player enters an NPC's trigger area.
    void OnTriggerEnter2D(Collider2D other)
    {
        DialogueTrigger dt = other.GetComponent<DialogueTrigger>();
        if (dt != null)
        {
            currentDialogueTrigger = dt;
        }
    }

    // When the player exits the NPC's trigger area.
    void OnTriggerExit2D(Collider2D other)
    {
        DialogueTrigger dt = other.GetComponent<DialogueTrigger>();
        if (dt != null && dt == currentDialogueTrigger)
        {
            currentDialogueTrigger = null;
            DialogueManager.instance.EndDialogue();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // If the dialogue panel is already active, show the next sentence.
            if (DialogueManager.instance.dialoguePanel.activeSelf)
            {
                DialogueManager.instance.DisplayNextSentence();
            }
            // Otherwise, if near a dialogue trigger, start the dialogue.
            else if (currentDialogueTrigger != null)
            {
                currentDialogueTrigger.TriggerDialogue();
            }
        }
    }
}
