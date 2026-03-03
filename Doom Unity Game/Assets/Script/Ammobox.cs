using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [Header("Settings")]
    public int ammoAmount = 10;          // How much ammo this box gives
    public float respawnTime = 10f;      // Time before the box reappears
    public bool respawns = true;         // Toggle respawn on/off

    [Header("Visuals")]
    public GameObject model;             // The visible ammo box model
    public AudioSource pickupSound;      // Optional sound

    private bool isAvailable = true;

    void OnTriggerEnter(Collider other)
    {
        if (!isAvailable)
            return;

        // Check if the player has a weapon with ammo
        Railgun railgun = other.GetComponentInChildren<Railgun>();
        GunShooter gun = other.GetComponentInChildren<GunShooter>();

        bool gaveAmmo = false;

        if (railgun != null)
        {
            railgun.currentAmmo = Mathf.Min(
                railgun.currentAmmo + ammoAmount,
                railgun.maxAmmo
            );
            railgun.UpdateAmmoUI();
            gaveAmmo = true;
        }

        if (gun != null)
        {
            gun.currentAmmo = Mathf.Min(
                gun.currentAmmo + ammoAmount,
                gun.maxAmmo
            );
            gun.UpdateAmmoUI();
            gaveAmmo = true;
        }

        if (gaveAmmo)
            Pickup();
    }

    void Pickup()
    {
        isAvailable = false;

        if (pickupSound != null)
            pickupSound.Play();

        if (model != null)
            model.SetActive(false);

        if (respawns)
            StartCoroutine(Respawn());
    }

    System.Collections.IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);

        isAvailable = true;

        if (model != null)
            model.SetActive(true);
    }
}
