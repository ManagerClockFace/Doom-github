using UnityEngine;

public class GunShooter : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public ParticleSystem muzzleFlash;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    [Header("Projectile Settings")]
    public float projectileSpeed = 40f;

    [Header("Raycast Settings")]
    public float range = 100f;
    public GameObject impactEffect;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Play muzzle flash
        if (muzzleFlash != null)
            muzzleFlash.Play();

        // Fire projectile
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Rigidbody rb = proj.GetComponent<Rigidbody>();

            if (rb != null)
                rb.linearVelocity = projectileSpawnPoint.forward * projectileSpeed;
        }

        // Raycast hit detection (optional but useful)
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            Debug.Log("Hit: " + hit.collider.name);

            if (impactEffect)
            {
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}
