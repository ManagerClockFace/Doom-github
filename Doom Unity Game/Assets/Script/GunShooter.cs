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
    public GameObject reloadBarBG;
    public Image reloadBarFill;

    [Header("Audio")]
    public AudioSource gunAudio;
    public AudioSource emptySound;

    [Header("Recoil")]
    public float recoilAmount = 2f;
    public float recoilRecoverySpeed = 6f;
    private float currentRecoil = 0f;
    private float recoilVelocity = 0f;

    [Header("Gun Model Recoil")]
    public Transform gunModel;
    public float kickbackDistance = 0.1f;
    public float kickbackSpeed = 12f;
    public float returnSpeed = 8f;

    private Vector3 originalGunPos;
    private Vector3 currentGunOffset;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        if (reloadBarBG != null)
            reloadBarBG.SetActive(false);

        if (reloadBarFill != null)
            reloadBarFill.fillAmount = 0f;

        if (gunModel != null)
            originalGunPos = gunModel.localPosition;
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

        // Smooth camera recoil
        currentRecoil = Mathf.SmoothDamp(currentRecoil, 0f, ref recoilVelocity, 1f / recoilRecoverySpeed);
        cam.transform.localRotation = Quaternion.Euler(currentRecoil, 0f, 0f);

        // Smooth gun model recoil
        currentGunOffset = Vector3.Lerp(currentGunOffset, Vector3.zero, returnSpeed * Time.deltaTime);

        if (gunModel != null)
            gunModel.localPosition = originalGunPos + currentGunOffset;
    }

    void Shoot()
    {
        // EMPTY GUN CLICK
        if (currentAmmo <= 0)
        {
            if (emptySound != null)
                emptySound.Play();

            Debug.Log("Out of ammo!");
            return;
        }

        currentAmmo--;
        UpdateAmmoUI();
        nextFireTime = Time.time + fireRate;

        // Camera recoil
        currentRecoil += recoilAmount;

        // Gun model recoil
        currentGunOffset -= new Vector3(0, 0, kickbackDistance);

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
            {
                rb.isKinematic = false;
                rb.linearVelocity = projectileSpawnPoint.forward * projectileSpeed;
            }
        }

        // Raycast hit detection
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

        if (reloadBarBG != null)
            reloadBarBG.SetActive(false);

        if (reloadBarFill != null)
            reloadBarFill.fillAmount = 0f;
    }

    public void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo + " / " + maxAmmo;
    }
}
