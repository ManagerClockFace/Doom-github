using UnityEngine;

public class QuitGame : MonoBehaviour
{
    void Update()
    {
        // Check if the Escape key was pressed this frame
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed. Quitting game...");

            // This quits the actual built application
            Application.Quit();

            // This allows the "Quit" action to work inside the Unity Editor
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}