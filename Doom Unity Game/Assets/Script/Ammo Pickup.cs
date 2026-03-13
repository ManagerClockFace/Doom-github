using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 6;

    private void OnTriggerEnter(Collider other)
    {
        GunShooter gun = other.GetComponentInChildren<GunShooter>();
        if (gun != null)
        {
            // Add ammo to reserve
            gun.reserveAmmo += ammoAmount;
            gun.UpdateAmmoUI();

            // Play pickup sound if effects script exists
            PickupEffects fx = GetComponent<PickupEffects>();
            if (fx != null)
                fx.PlayPickupSound();

            // Destroy after sound triggers
            Destroy(gameObject, 0.05f);
        }
    }
}
