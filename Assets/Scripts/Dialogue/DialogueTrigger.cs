using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private GameObject talkPromptUI;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dialogueSound;

    private void Start()
    {
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

        // With [RequireComponent] this should never be null, but it's good to double-check.
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void TriggerDialogue()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }

        // Play the dialogue sound effect.
        if (audioSource != null && dialogueSound != null)
        {
            audioSource.PlayOneShot(dialogueSound);
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
