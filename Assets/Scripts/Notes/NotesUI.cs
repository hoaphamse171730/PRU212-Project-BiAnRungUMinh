using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NotesUI : MonoBehaviour
{
    public static NotesUI Instance { get; private set; }

    [SerializeField] private GameObject notesPanel; 
    [SerializeField] private Text notesText;
    private void Awake()
    {
        // Ensure this instance persists and only one exists.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (notesPanel != null)
        {
            notesPanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        // Subscribe to sceneLoaded events.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called whenever a new scene is loaded.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the notesPanel is not assigned, attempt to find it in the new scene.
        if (notesPanel == null)
        {
            notesPanel = GameObject.Find("NotesBox");
            notesPanel.SetActive(false);

        }

        // If notesPanel is found and notesText is not assigned, look for the Text component.
        if (notesPanel != null && notesText == null)
        {
            Transform textTransform = notesPanel.transform.Find("NotesText");
            if (textTransform != null)
            {
                notesText = textTransform.GetComponent<Text>();
            }
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
