using UnityEngine;
using System.Collections.Generic;

public class NotesManager : MonoBehaviour
{
    public static NotesManager Instance { get; private set; }
    [SerializeField] private List<string> notes = new List<string>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddNote(string note)
    {
        if (!string.IsNullOrEmpty(note) && !notes.Contains(note))
        {
            notes.Add(note);
            Debug.Log($"Note added: {note}");
        }
        else
        {
            Debug.Log("Note already exists or is empty.");
        }
    }

    public List<string> GetNotes()
    {
        // Return a copy to prevent external modification
        return new List<string>(notes);
    }

    public void ClearNotes()
    {
        notes.Clear();
    }
}
