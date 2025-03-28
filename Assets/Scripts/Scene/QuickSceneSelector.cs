using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickSceneSelector : MonoBehaviour
{
    [Tooltip("List of scene names to load. Make sure these scenes are added to the Build Settings.")]
    public string[] scenes;

    // Button dimensions and spacing
    public int buttonWidth = 200;
    public int buttonHeight = 50;
    public int spacing = 10;

    private void OnGUI()
    {
        // Start position for buttons
        int x = 10;
        int y = 10;

        // Create a button for each scene
        foreach (string sceneName in scenes)
        {
            if (GUI.Button(new Rect(x, y, buttonWidth, buttonHeight), sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            y += buttonHeight + spacing;
        }
    }
}
