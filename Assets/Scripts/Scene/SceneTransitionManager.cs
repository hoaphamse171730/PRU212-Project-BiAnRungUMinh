using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private int playerScore = 0; // Example condition for endings

    private int currentSceneIndex = 0;
    private string[] sceneOrder = { "MainMenu", "Prologue", "Chapter_1", "Chapter_2", "Chapter_3", "Chapter_4", "BadEnding", "OpenEnding", "EndMenu" };

    // Singleton pattern to ensure only one instance exists
    public static SceneTransitionManager Instance { get; private set; }

    void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this GameObject from being destroyed on scene load
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }
    }

    void Start()
    {
        // Initialize the current scene
        currentSceneIndex = System.Array.IndexOf(sceneOrder, SceneManager.GetActiveScene().name);
        if (currentSceneIndex == -1)
        {
            Debug.LogWarning("Current scene not found in sceneOrder. Starting from MainMenu.");
            currentSceneIndex = 0; // Default to MainMenu
        }
        Debug.Log($"Start: Current scene is {SceneManager.GetActiveScene().name}, currentSceneIndex = {currentSceneIndex}");
    }

    public void NextScene()
    {
        Debug.Log($"NextScene: Before increment, currentSceneIndex = {currentSceneIndex} (Scene: {sceneOrder[currentSceneIndex]})");

        // Determine the next scene
        currentSceneIndex++;
        if (currentSceneIndex >= sceneOrder.Length)
        {
            currentSceneIndex = sceneOrder.Length - 1; // Stay at EndMenu
            Debug.Log($"NextScene: Clamped to EndMenu, currentSceneIndex = {currentSceneIndex}");
        }
        else if (currentSceneIndex == 6) // Check for ending condition at Chapter_4 (index 5 in new sceneOrder)
        {
            if (playerScore < 50) // Bad ending condition
            {
                currentSceneIndex = 6; // BadEnding
                Debug.Log($"NextScene: Bad ending condition met, currentSceneIndex = {currentSceneIndex}");
            }
            else // Good ending condition
            {
                currentSceneIndex = 7; // OpenEnding
                Debug.Log($"NextScene: Good ending condition met, currentSceneIndex = {currentSceneIndex}");
            }
        }

        Debug.Log($"NextScene: Loading scene {sceneOrder[currentSceneIndex]} (Index: {currentSceneIndex})");
        // Load the next scene immediately
        SceneManager.LoadScene(sceneOrder[currentSceneIndex]);
    }

    // Method to restart from the beginning
    public void RestartGame()
    {
        currentSceneIndex = 1; // Set to Prologue (index 1 in new sceneOrder)
        Debug.Log($"RestartGame: Resetting to Prologue, currentSceneIndex = {currentSceneIndex}");
        SceneManager.LoadScene("Prologue");
    }

    // Method to return to main menu
    public void ReturnToMainMenu()
    {
        currentSceneIndex = 1; // Set to Prologue for not getting bug
        Debug.Log($"ReturnToMainMenu: Setting to MainMenu, currentSceneIndex = {currentSceneIndex}");
        SceneManager.LoadScene("MainMenu");
    }

    // Method to update player score (example, call this during gameplay)
    public void UpdatePlayerScore(int score)
    {
        playerScore = score;
    }
}