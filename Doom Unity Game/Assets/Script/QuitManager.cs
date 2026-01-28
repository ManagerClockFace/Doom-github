using UnityEngine;

public class QuitManager : MonoBehaviour
{
    public void QuitGame()
    {
        // This line quits the actual built application
        Application.Quit();

        // This line stops the "Play" mode in the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Debug.Log("Game is exiting...");
    }
}