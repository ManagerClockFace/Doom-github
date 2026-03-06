using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    // Assign your menu Panel in the Inspector
    public GameObject menuPanel;
    // Assign the key you want to use in the Inspector
    public KeyCode toggleKey = KeyCode.I;

    private bool isMenuOpen;

    void Start()
    {
        // Ensure the initial state matches the Inspector setting
        isMenuOpen = menuPanel.activeSelf;
    }

    void Update()
    {
        // Check if the toggle key is pressed down
        if (Input.GetKeyDown(toggleKey))
        {
            // Toggle the menu state
            isMenuOpen = !isMenuOpen;
            // Set the panel's active state accordingly
            menuPanel.SetActive(isMenuOpen);
        }
    }
}
