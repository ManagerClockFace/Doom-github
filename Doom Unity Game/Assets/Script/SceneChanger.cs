using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene switching

public class SceneChanger : MonoBehaviour
{
    // Call this function to change the scene by name
    public void ChangeSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Call this function to change the scene by index
    public void ChangeSceneByIndex(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}