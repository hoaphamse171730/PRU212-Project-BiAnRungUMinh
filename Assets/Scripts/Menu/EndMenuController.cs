using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenuController : MonoBehaviour
{
    [SerializeField] private Button returnButton;
    [SerializeField] private Button quitButton;

    void Start()
    {
        // Ensure all references are assigned in the Inspector
        if (quitButton == null || returnButton == null)
        {
            Debug.LogError("One or more button or panel references are missing in MainMenuScript!");
            return;
        }

        // Add listeners to buttons
        returnButton.onClick.AddListener(OnReturnMenuClicked);
        quitButton.onClick.AddListener(OnQuitGameClicked);
    }

    void OnReturnMenuClicked()
    {
        // Load the Main Menu scene
        SceneManager.LoadScene("MainMenu");
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