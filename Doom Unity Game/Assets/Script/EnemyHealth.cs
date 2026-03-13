using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 50f;

    [Header("Damage Number Settings")]
    public GameObject damageNumberPrefab;
    public Transform damageCanvas;

    private float stackOffset = 0f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Enemy health now: " + health);

        if (damageNumberPrefab != null && damageCanvas != null)
        {
            GameObject dmg = Instantiate(damageNumberPrefab, damageCanvas);

            DamageNumber dn = dmg.GetComponent<DamageNumber>();
            if (dn != null)
            {
                dn.SetText(amount.ToString());
                dn.SetTarget(transform, stackOffset);
            }

            stackOffset += 0.5f;
        }

        if (health <= 0f)
        {
            stackOffset = 0f;
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
