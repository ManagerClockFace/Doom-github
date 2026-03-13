using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 50f;
    public float currentHealth;

    [Header("Death Settings")]
    public GameObject deathEffect;   // optional particle effect
    public float destroyDelay = 0.1f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Register kill for laser wall / boss room logic
        if (EnemyKillManager.instance != null)
            EnemyKillManager.instance.RegisterKill();

        // Spawn death effect
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(gameObject, destroyDelay);
    }
}
