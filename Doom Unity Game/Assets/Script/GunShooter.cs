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
    public int maxMagazine = 6;
    public int currentMagazine;
    public int reserveAmmo = 0;
    public float reloadTime = 1.5f;
    private bool isReloading = false;

    [Header("UI")]
    public TextMeshProUGUI ammoText;
    public GameObject reloadBarBG;
    public Image reloadBarFill;

    [Header("Audio")]
    public AudioSource gunAudio;
    public AudioSource emptySound;

    void Start()
    {
        currentMagazine = maxMagazine;
        reserveAmmo = 0; // spawn with no extra ammo
        UpdateAmmoUI();

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
        if (currentMagazine <= 0)
        {
            if (emptySound != null)
                emptySound.Play();
            return;
        }

        currentMagazine--;
        UpdateAmmoUI();
        nextFireTime = Time.time + fireRate;

        if (gunAudio != null)
            gunAudio.Play();

        if (muzzleFlash != null)
            muzzleFlash.Play();

        // Projectile
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = projectileSpawnPoint.forward * projectileSpeed;
        }

        // Raycast hit detection
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
                enemy.TakeDamage(25f);

            if (impactEffect)
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    IEnumerator Reload()
    {
        if (currentMagazine == maxMagazine)
            yield break;

        if (reserveAmmo <= 0)
            yield break; // no ammo to reload with

        isReloading = true;

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

        int needed = maxMagazine - currentMagazine;
        int loadAmount = Mathf.Min(needed, reserveAmmo);

        currentMagazine += loadAmount;
        reserveAmmo -= loadAmount;

        isReloading = false;
        UpdateAmmoUI();

        if (reloadBarBG != null)
            reloadBarBG.SetActive(false);

        if (reloadBarFill != null)
            reloadBarFill.fillAmount = 0f;
    }

    public void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentMagazine + "/" + maxMagazine + " (" + reserveAmmo + ")";
    }
}
