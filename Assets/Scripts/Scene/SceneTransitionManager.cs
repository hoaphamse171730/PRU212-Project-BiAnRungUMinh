using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private int playerScore = 0; // Example condition for endings
    private int currentSceneIndex = 0;
    // Order: Prologue -> Chapter_1 -> Chapter_2 -> Chapter_3 -> Chapter_4 -> (endings) -> EndMenu
    private string[] sceneOrder = { "Prologue", "Chapter_1", "Chapter_2", "Chapter_3", "Chapter_4", "BadEnding", "OpenEnding", "EndMenu" };

    public static SceneTransitionManager Instance { get; private set; }

    void Awake()
    {
        // Enforce the singleton pattern and persist the manager across scenes.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // Initialize currentSceneIndex only once from the active scene.
            currentSceneIndex = System.Array.IndexOf(sceneOrder, SceneManager.GetActiveScene().name);
            if (currentSceneIndex == -1)
            {
                Debug.LogWarning("Awake: Current scene not found in sceneOrder. Defaulting to Prologue.");
                currentSceneIndex = 0;
            }
            Debug.Log($"Awake: Active scene is {SceneManager.GetActiveScene().name}, currentSceneIndex set to {currentSceneIndex}");
        }
        else
        {
            // Destroy any duplicate instances.
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

    // Update currentSceneIndex when a new scene is loaded.
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

        if (currentScene == "Chapter_4")
        {
            string decision = DecisionManager.SelectedEventID;

            if (decision == "ShowBadNPC") // Bad ending condition
            {
                currentSceneIndex = System.Array.IndexOf(sceneOrder, "BadEnding");
                Debug.Log("NextScene: Bad ending condition met. Loading BadEnding.");
            }
            else if (decision == "ShowGoodNPC") // Good ending condition
            {
                currentSceneIndex = System.Array.IndexOf(sceneOrder, "OpenEnding");
                Debug.Log("NextScene: Good ending condition met. Loading OpenEnding.");
            }
            else
            {
                currentSceneIndex = System.Array.IndexOf(sceneOrder, "BadEnding");
                Debug.Log("NextScene: BUG when finding decision. Defaulting to BadEnding.");
            }
        }
        else if (currentSceneIndex < sceneOrder.Length - 1)
        {
            currentSceneIndex++;
        }
        else
        {
            Debug.Log("NextScene: Already at final scene.");
        }

        string nextScene = sceneOrder[currentSceneIndex];
        Debug.Log($"NextScene: Loading scene {nextScene} (Index: {currentSceneIndex})");

        // If loading the end menu, clear persistent managers.
        if (nextScene == "EndMenu")
        {
            ClearPersistentManagers();
        }

        SceneManager.LoadScene(nextScene);
    }

    public void RestartGame()
    {
        currentSceneIndex = 0;
        Debug.Log($"RestartGame: Resetting to Prologue, currentSceneIndex = {currentSceneIndex}");
        SceneManager.LoadScene(sceneOrder[currentSceneIndex]);
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("ReturnToMainMenu: Loading MainMenu.");
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Clears persistent managers from the scene.
    /// </summary>
    public void ClearPersistentManagers()
    {
        // Destroy other persistent managers if they exist.
        var decisionManager = FindObjectOfType<DecisionManager>();
        if (decisionManager != null)
        {
            Destroy(decisionManager.gameObject);
        }
        var dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager != null)
        {
            Destroy(dialogueManager.gameObject);
        }
        var uiManager = FindObjectOfType<NotesUI>();
        if (uiManager != null)
        {
            Destroy(uiManager.gameObject);
        }
        var noteManager = FindObjectOfType<NotesManager>();
        if (noteManager != null)
        {
            Destroy(noteManager.gameObject);
        }
        // Optionally, destroy the SceneTransitionManager itself.
        Destroy(gameObject);
    }
}
