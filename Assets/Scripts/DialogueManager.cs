using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    // Assign these in the Inspector
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Text speakerText; // Optional: to display the speaker's name
    
    private Queue<string> sentences;

    void Awake()
    {
        // Singleton pattern for easy access
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Ensure the panel is hidden initially
        dialoguePanel.SetActive(false);
        sentences = new Queue<string>();

    }


    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePanel.SetActive(true);
        // Optionally display the speaker's name
        if (speakerText != null)
            speakerText.text = dialogue.speaker;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
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

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}


