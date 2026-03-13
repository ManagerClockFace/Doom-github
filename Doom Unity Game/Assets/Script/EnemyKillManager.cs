using UnityEngine;

public class EnemyKillManager : MonoBehaviour
{
    public static EnemyKillManager instance;

    public int enemiesKilled = 0;

    private void Awake()
    {
        instance = this;
    }

    public void RegisterKill()
    {
        enemiesKilled++;
    }
}
