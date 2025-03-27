using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadMenuController : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button returnButton;

    void Start()
    {
        if (restartButton == null || returnButton == null)
        {
            Debug.LogError("One or more button references are missing!");
            return;
        }

        restartButton.onClick.AddListener(OnRestartGameClicked);
        returnButton.onClick.AddListener(OnReturnMenuClicked);
    }

    void OnRestartGameClicked()
    {
        string sceneToLoad = string.IsNullOrEmpty(GameSession.LastScene) ? "Prologue" : GameSession.LastScene;

        if (sceneToLoad == "BadEnding")
        {
            sceneToLoad = "Prologue";
        }

        SceneManager.LoadScene(sceneToLoad);
    }

    void OnReturnMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
