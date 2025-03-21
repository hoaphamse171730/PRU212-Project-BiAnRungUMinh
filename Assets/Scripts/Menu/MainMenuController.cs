using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button instructionButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject Buttons;

    void Start()
    {
        // Ensure all references are assigned in the Inspector
        if (startButton == null || instructionButton == null || quitButton == null ||
            instructionPanel == null || closeButton == null)
        {
            Debug.LogError("One or more button or panel references are missing in MainMenuScript!");
            return;
        }

        // Add listeners to buttons
        startButton.onClick.AddListener(OnStartGameClicked);
        instructionButton.onClick.AddListener(OnInstructionClicked);
        quitButton.onClick.AddListener(OnQuitGameClicked);
        closeButton.onClick.AddListener(OnCloseClicked);

        // Initially hide the instruction panel
        instructionPanel.SetActive(false);
    }

    void OnStartGameClicked()
    {
        // Load the Prolouge scene
        SceneManager.LoadScene("Prologue");
    }

    void OnInstructionClicked()
    {
        // Show the instruction panel
        instructionPanel.SetActive(true);
        Buttons.SetActive(false);
    }

    void OnCloseClicked()
    {
        // Hide the instruction panel
        instructionPanel.SetActive(false);
        Buttons.SetActive(true);
    }

    void OnQuitGameClicked()
    {
        // Quit the application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in Editor
#else
            Application.Quit(); // Quit the built application
#endif
    }
}