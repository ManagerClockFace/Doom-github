using UnityEngine;

public class LaserWallController : MonoBehaviour
{
    public int killsRequired = 10;
    public GameObject visual;
    public Collider wallCollider;

    [Header("Audio")]
    public AudioSource deactivateSound;

    private bool opened = false;

    void Update()
    {
        if (!opened && EnemyKillManager.instance.enemiesKilled >= killsRequired)
        {
            OpenWall();
        }
    }

    void OpenWall()
    {
        opened = true;

        // Play sound
        if (deactivateSound != null)
            deactivateSound.Play();

        // Disable visuals
        if (visual != null)
            visual.SetActive(false);

        // Disable collider
        if (wallCollider != null)
            wallCollider.enabled = false;

        Debug.Log("Laser wall opened!");
    }
}
