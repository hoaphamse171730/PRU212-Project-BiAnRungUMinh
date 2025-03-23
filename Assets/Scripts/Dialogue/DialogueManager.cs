using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Text speakerText;

    // New UI elements for decision system:
    [SerializeField] private GameObject decisionPanel;        // Panel for showing decision choices.
    [SerializeField] private Transform decisionContainer;     // Parent container for decision buttons.
    [SerializeField] private Button decisionButtonPrefab;       // Prefab for a single decision button.

    private Queue<string> sentences = new Queue<string>();
    private Dialogue currentDialogue;

    public bool IsDialogueActive => dialoguePanel && dialoguePanel.activeSelf;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (dialoguePanel)
            dialoguePanel.SetActive(false);
        if (decisionPanel)
            decisionPanel.SetActive(false);
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
            // Instead of ending dialogue immediately, check for decision choices.
            if (currentDialogue != null && currentDialogue.Choices != null && currentDialogue.Choices.Length > 0)
            {
                DisplayChoices();
            }
            else
            {
                EndDialogue();
            }
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
        ClearChoices();
    }

    private void DisplayChoices()
    {
        if (decisionPanel == null || decisionContainer == null || decisionButtonPrefab == null)
        {
            Debug.LogWarning("Decision UI elements are not assigned.");
            EndDialogue();
            return;
        }

        // Activate decision panel.
        decisionPanel.SetActive(true);
        // Clear any existing buttons.
        foreach (Transform child in decisionContainer)
        {
            Destroy(child.gameObject);
        }

        // Create a button for each dialogue choice.
        foreach (DialogueChoice choice in currentDialogue.Choices)
        {
            Button btn = Instantiate(decisionButtonPrefab, decisionContainer);
            Text btnText = btn.GetComponentInChildren<Text>();
            if (btnText != null)
            {
                btnText.text = choice.ChoiceText;
            }
            btn.onClick.AddListener(() => OnChoiceSelected(choice));
        }
    }

    private void ClearChoices()
    {
        if (decisionPanel != null)
        {
            decisionPanel.SetActive(false);
        }
        if (decisionContainer != null)
        {
            foreach (Transform child in decisionContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void OnChoiceSelected(DialogueChoice choice)
    {
        // Store the current dialogue's note (if any) before transitioning.
        if (currentDialogue != null && !string.IsNullOrEmpty(currentDialogue.Note))
        {
            NotesManager.Instance.AddNote(currentDialogue.Note);
        }

        ClearChoices();

        // Trigger the event if an EventID is provided.
        if (!string.IsNullOrEmpty(choice.EventID))
        {
            DecisionManager.Instance.TriggerDecision(choice.EventID);
        }

        // Then check if there's a next dialogue branch.
        if (choice.NextDialogue != null)
        {
            StartDialogue(choice.NextDialogue);
        }
        else
        {
            EndDialogue();
        }
    
}
}
