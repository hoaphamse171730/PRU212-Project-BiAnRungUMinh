using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private GameObject talkPromptUI;

    private void Start()
    {
        // If talkPromptUI hasn't been assigned, try finding it by name.
        if (talkPromptUI == null)
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name == "TalkPrompt")
                {
                    talkPromptUI = obj;
                    break;
                }
            }
            if (talkPromptUI == null)
            {
                Debug.LogWarning("TalkPrompt GameObject not found even with Resources.FindObjectsOfTypeAll.");
            }
        }
    }

    public void TriggerDialogue()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
        if (talkPromptUI != null)
        {
            talkPromptUI.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (talkPromptUI != null)
            {
                talkPromptUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (talkPromptUI != null)
            {
                talkPromptUI.SetActive(false);
            }
        }
    }
}
