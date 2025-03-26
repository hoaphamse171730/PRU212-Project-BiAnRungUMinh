using UnityEngine;
using UnityEngine.UI;

public class ToggleNoteButton : MonoBehaviour
{
    private Button toggleButton;

    private void Awake()
    {
        toggleButton = GetComponent<Button>();
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(OnToggleNoteButtonClicked);
        }
        else
        {
            Debug.LogWarning("Button component not found on ToggleNoteButton.");
        }
    }

    private void OnToggleNoteButtonClicked()
    {
        if (NotesUI.Instance != null)
        {
            NotesUI.Instance.ToggleNotesPanel();
        }
        else
        {
            Debug.LogWarning("UIManager instance not found.");
        }
    }
}
