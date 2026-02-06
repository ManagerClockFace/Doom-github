using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

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

    [Header("Fire Settings")]
    public float fireRate = 0.25f;
    private float nextFireTime = 0f;

    [Header("Ammo Settings")]
    public int maxAmmo = 6;
    public int currentAmmo;
    public float reloadTime = 1.5f;
    private bool isReloading = false;

    [Header("UI")]
    public TextMeshProUGUI ammoText;

    [Header("Reload UI")]
    public GameObject reloadBarBG;     // The background bar
    public Image reloadBarFill;        // The fill image

    [Header("Audio")]
    public AudioSource gunAudio;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        // Hide reload bar at start
        if (reloadBarBG != null)
            reloadBarBG.SetActive(false);

        if (reloadBarFill != null)
            reloadBarFill.fillAmount = 0f;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo!");
            return;
        }

        currentAmmo--;
        UpdateAmmoUI();
        nextFireTime = Time.time + fireRate;

        if (gunAudio != null)
            gunAudio.Play();

        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.linearVelocity = projectileSpawnPoint.forward * projectileSpeed;
            }
        }

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            Debug.Log("Hit: " + hit.collider.name);

            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(10f);
            }

            if (impactEffect)
            {
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        // Show reload bar
        if (reloadBarBG != null)
            reloadBarBG.SetActive(true);

        float elapsed = 0f;

        while (elapsed < reloadTime)
        {
            elapsed += Time.deltaTime;

            if (reloadBarFill != null)
                reloadBarFill.fillAmount = elapsed / reloadTime;

            yield return null;
        }

        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoUI();

        // Hide reload bar
        if (reloadBarBG != null)
            reloadBarBG.SetActive(false);

        if (reloadBarFill != null)
            reloadBarFill.fillAmount = 0f;
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo + " / " + maxAmmo;
    }
}
