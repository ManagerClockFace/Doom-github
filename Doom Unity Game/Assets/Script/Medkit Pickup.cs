using UnityEngine;

public class MedkitPickup : MonoBehaviour
{
    public float healAmount = 25f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            // Heal player
            health.Heal(healAmount);

            // Play pickup sound if effects script exists
            PickupEffects fx = GetComponent<PickupEffects>();
            if (fx != null)
                fx.PlayPickupSound();

            // Destroy after sound triggers
            Destroy(gameObject, 0.05f);
        }
    }
}
