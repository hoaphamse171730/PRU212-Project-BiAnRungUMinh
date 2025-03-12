using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NotesUI : MonoBehaviour
{
    [SerializeField] private GameObject notesPanel; 
    [SerializeField] private Text notesText;
    private void Awake()
    {
        if (notesPanel != null)
        {
            notesPanel.SetActive(false);
        }
    }

    public void ToggleNotesPanel()
    {
        if (notesPanel == null)
        {
            Debug.LogWarning("NotesPanel is not assigned.");
            return;
        }

        bool isActive = notesPanel.activeSelf;
        notesPanel.SetActive(!isActive);

        if (!isActive)
        {
            UpdateNotesUI();
        }
    }

    private void UpdateNotesUI()
    {
        if (notesText == null)
        {
            Debug.LogWarning("NotesText is not assigned.");
            return;
        }

        List<string> notes = NotesManager.Instance.GetNotes();
        notesText.text = "Notes:\n";

        if (notes.Count == 0)
        {
            notesText.text += "No notes available.";
        }
        else
        {
            foreach (string note in notes)
            {
                notesText.text += "- " + note + "\n";
            }
        }
    }
}
