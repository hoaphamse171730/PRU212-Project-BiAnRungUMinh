using UnityEngine;

public class PersistentManagersLoader : MonoBehaviour
{
    [SerializeField] private GameObject decisionManagerPrefab;
    [SerializeField] private GameObject dialogueManagerPrefab;
    [SerializeField] private GameObject notesUIPrefab;
    [SerializeField] private GameObject notesManagerPrefab;

    private void Awake()
    {
        if (FindObjectOfType<DecisionManager>() == null && decisionManagerPrefab != null)
        {
            Instantiate(decisionManagerPrefab);
        }
        // Do the same for other persistent managers.
        if (FindObjectOfType<DialogueManager>() == null && dialogueManagerPrefab != null)
        {
            Instantiate(dialogueManagerPrefab);
        }
        if (FindObjectOfType<NotesUI>() == null && notesUIPrefab != null)
        {
            Instantiate(notesUIPrefab);
        }
        if (FindObjectOfType<NotesManager>() == null && notesManagerPrefab != null)
        {
            Instantiate(notesManagerPrefab);
        }
    }
}
