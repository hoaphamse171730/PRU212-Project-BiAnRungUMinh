using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadMenuController : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button returnButton;

    void Start()
    {
        // Ensure all references are assigned in the Inspector
        if (restartButton == null || returnButton == null)
        {
            Debug.LogError("One or more button or panel references are missing in MainMenuScript!");
            return;
        }

        // Add listeners to buttons
        restartButton.onClick.AddListener(OnRestartGameClicked);
        returnButton.onClick.AddListener(OnReturnMenuClicked);
    }

    void OnRestartGameClicked()
    {
        // Load the Prologue scene
        SceneManager.LoadScene("Prologue");
    }

    void OnReturnMenuClicked()
    {
        // Load the Main Menu scene
        SceneManager.LoadScene("MainMenu");
    }
}