using UnityEngine;

public class EndpointTrigger : MonoBehaviour
{
    private SceneTransitionManager sceneTransitionManager;

    void Start()
    {
        // Find the SceneTransitionManager in the scene
        sceneTransitionManager = FindFirstObjectByType<SceneTransitionManager>();
        if (sceneTransitionManager == null)
        {
            Debug.LogError("SceneTransitionManager not found in the scene!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Assuming the player has a tag "Player"
        if (other.CompareTag("Player"))
        {
            sceneTransitionManager.NextScene();
        }
    }
}