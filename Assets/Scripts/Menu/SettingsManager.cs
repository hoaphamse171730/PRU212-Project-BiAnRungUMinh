using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button returnToMainMenuButton;

    private bool isPaused = false;

    // Singleton pattern to ensure only one instance exists
    public static SettingsManager Instance { get; private set; }

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
        // Ensure references are assigned
        if (pauseMenuCanvas == null || resumeButton == null || returnToMainMenuButton == null)
        {
            Debug.LogError("PauseMenuCanvas, ResumeButton, or ReturnToMainMenuButton reference is missing in SettingsManager!");
            return;
        }

        // Ensure the pause menu canvas persists (as a child of this GameObject)
        if (pauseMenuCanvas.transform.root == transform)
        {
            DontDestroyOnLoad(pauseMenuCanvas.transform.root.gameObject);
        }
        else
        {
            Debug.LogWarning("PauseMenuCanvas is not a child of SettingsManager. Ensure it persists across scenes!");
        }

        // Hide the pause menu initially
        CanvasGroup canvasGroup = pauseMenuCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        pauseMenuCanvas.SetActive(false);

        // Add button listeners
        resumeButton.onClick.AddListener(ResumeGame);
        returnToMainMenuButton.onClick.AddListener(ReturnToMainMenu);

        // Update button visibility based on the current scene
        UpdateButtonVisibility();
    }

    void Update()
    {
        // Toggle pause menu with Escape key, but only if not in MainMenu
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Called when a new scene is loaded to update button visibility
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateButtonVisibility();
    }

    private void UpdateButtonVisibility()
    {
        // Disable the Resume button in the MainMenu scene
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            resumeButton.gameObject.SetActive(false);
        }
        else
        {
            resumeButton.gameObject.SetActive(true);
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pause the game

        // Show the pause menu
        CanvasGroup canvasGroup = pauseMenuCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            pauseMenuCanvas.SetActive(true);
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume the game

        // Hide the pause menu
        CanvasGroup canvasGroup = pauseMenuCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            pauseMenuCanvas.SetActive(false);
        }
    }

    private void ReturnToMainMenu()
    {
        // Resume time to ensure the game isn't paused in the main menu
        Time.timeScale = 1f;
        isPaused = false;

        // Hide the pause menu
        CanvasGroup canvasGroup = pauseMenuCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            pauseMenuCanvas.SetActive(false);
        }

        // Use SceneTransitionManager to load the main menu
        SceneTransitionManager sceneTransitionManager = SceneTransitionManager.Instance;
        if (sceneTransitionManager != null)
        {
            sceneTransitionManager.ReturnToMainMenu();
        }
        else
        {
            Debug.LogError("SceneTransitionManager not found! Loading MainMenu directly.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}