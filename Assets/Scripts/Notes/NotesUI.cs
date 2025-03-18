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
        Debug.Log("ToggleNotesPanel called in scene: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        if (notesPanel == null)
        {
            Debug.LogWarning("NotesPanel is not assigned.");
            return;
        }

        bool isActive = notesPanel.activeSelf;
        Debug.Log("NotesPanel active state before toggle: " + isActive);

        notesPanel.SetActive(!isActive);

        if (!isActive)
        {
            Debug.Log("Panel activated, updating notes UI...");
            UpdateNotesUI();
        }

        Debug.Log("NotesPanel active state after toggle: " + !isActive);
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
