using UnityEngine;
using System.Collections.Generic;

public class NotesManager : MonoBehaviour
{
    public static NotesManager instance;

    // A list to store unique notes
    public List<string> notes = new List<string>();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddNote(string note)
    {
        if (!string.IsNullOrEmpty(note) && !notes.Contains(note))
        {
            notes.Add(note);
            Debug.Log("Note added: " + note);
        }
        else
        {
            Debug.Log("Note already exists or is empty.");
        }
    }

    // Optional: Retrieve all stored notes
    public List<string> GetNotes()
    {
        return notes;
    }

    // Optional: Clear notes if needed
    public void ClearNotes()
    {
        notes.Clear();
    }
}
