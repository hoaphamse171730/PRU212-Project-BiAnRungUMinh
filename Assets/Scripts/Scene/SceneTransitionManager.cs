using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private int playerScore = 0; // Example condition for endings

    private int currentSceneIndex = 0;
    // Order: Prologue -> Chapter_1 -> Chapter_2 -> Chapter_3 -> Chapter_4 -> (endings) -> EndMenu
    private string[] sceneOrder = { "Prologue", "Chapter_1", "Chapter_2", "Chapter_3", "Chapter_4", "BadEnding", "OpenEnding", "EndMenu" };

    public static SceneTransitionManager Instance { get; private set; }

    void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        // Initialize currentSceneIndex based on the active scene.
        currentSceneIndex = System.Array.IndexOf(sceneOrder, SceneManager.GetActiveScene().name);
        if (currentSceneIndex == -1)
        {
            Debug.LogWarning("Current scene not found in sceneOrder. Starting from Prologue.");
            currentSceneIndex = 0;
        }
        Debug.Log($"Start: Current scene is {SceneManager.GetActiveScene().name}, currentSceneIndex = {currentSceneIndex}");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int index = System.Array.IndexOf(sceneOrder, scene.name);
        if (index != -1)
        {
            currentSceneIndex = index;
            Debug.Log($"OnSceneLoaded: Updated currentSceneIndex to {currentSceneIndex} for scene {scene.name}");
        }
        else
        {
            Debug.LogWarning($"OnSceneLoaded: Scene {scene.name} not found in sceneOrder.");
        }
    }

    public void NextScene()
    {
        string currentScene = sceneOrder[currentSceneIndex];
        Debug.Log($"NextScene: Current scene is {currentScene} at index {currentSceneIndex}");

        // If we are at Chapter_4, then decide which ending to load.
        if (currentScene == "Chapter_4")
        {
            if (playerScore < 50) // Bad ending condition
            {
                currentSceneIndex = System.Array.IndexOf(sceneOrder, "BadEnding");
                Debug.Log("NextScene: Bad ending condition met. Loading BadEnding.");
            }
            else // Good ending condition
            {
                currentSceneIndex = System.Array.IndexOf(sceneOrder, "OpenEnding");
                Debug.Log("NextScene: Good ending condition met. Loading OpenEnding.");
            }
        }
        // Otherwise, simply move to the next scene in the order.
        else if (currentSceneIndex < sceneOrder.Length - 1)
        {
            currentSceneIndex++;
        }
        else
        {
            Debug.Log("NextScene: Already at final scene.");
        }

        Debug.Log($"NextScene: Loading scene {sceneOrder[currentSceneIndex]} (Index: {currentSceneIndex})");
        SceneManager.LoadScene(sceneOrder[currentSceneIndex]);
    }

    // Method to restart from the beginning (starting at Prologue)
    public void RestartGame()
    {
        currentSceneIndex = 0; // Prologue is at index 0
        Debug.Log($"RestartGame: Resetting to Prologue, currentSceneIndex = {currentSceneIndex}");
        SceneManager.LoadScene(sceneOrder[currentSceneIndex]);
    }

    // Method to return to main menu (assumed to be separate from the scene order)
    public void ReturnToMainMenu()
    {
        Debug.Log("ReturnToMainMenu: Loading MainMenu.");
        SceneManager.LoadScene("MainMenu");
    }

    // Update the player score (for example, call this during gameplay)
    public void UpdatePlayerScore(int score)
    {
        playerScore = score;
    }
}
