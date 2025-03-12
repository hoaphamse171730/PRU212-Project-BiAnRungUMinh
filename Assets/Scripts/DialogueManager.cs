using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Text speakerText;

    private Queue<string> sentences = new Queue<string>();
    private Dialogue currentDialogue;

    public bool IsDialogueActive => dialoguePanel && dialoguePanel.activeSelf;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (dialoguePanel)
            dialoguePanel.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        if (dialoguePanel)
            dialoguePanel.SetActive(true);

        if (speakerText != null)
            speakerText.text = dialogue.Speaker;

        sentences.Clear();
        foreach (string sentence in dialogue.Sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = sentences.Dequeue();
    }

    public void EndDialogue()
    {
        if (dialoguePanel)
            dialoguePanel.SetActive(false);

        if (currentDialogue != null && !string.IsNullOrEmpty(currentDialogue.Note))
        {
            NotesManager.Instance.AddNote(currentDialogue.Note);
        }

        currentDialogue = null;
    }
}
