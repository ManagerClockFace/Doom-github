using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Railgun : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public ParticleSystem chargeFlash;
    public ParticleSystem fireFlash;
    public AudioSource fireSound;

    [Header("Beam")]
    public LineRenderer beam;
    public float beamDuration = 0.1f;

    [Header("Beam Light")]
    public Light beamLight;
    public float lightDuration = 0.1f;

    [Header("Screen Shake")]
    public CameraShake camShake;
    public float shakeDuration = 0.15f;
    public float shakeMagnitude = 0.2f;

    [Header("Stats")]
    public float damage = 200f;
    public float range = 500f;
    public float cooldown = 1.5f;
    private bool canFire = true;

    [Header("Piercing")]
    public int maxPierce = 5;

    [Header("UI")]
    public TextMeshProUGUI ammoText;
    public GameObject reloadBarBG;
    public Image reloadBarFill;

    [Header("Ammo")]
    public int maxAmmo = 3;
    public int currentAmmo;
    public float reloadTime = 2f;
    private bool isReloading = false;

    // -------------------------
    // ADS (Aim Down Sights)
    // -------------------------
    [Header("ADS (Aim Down Sights)")]
    public float adsFOV = 30f;
    public float adsSpeed = 10f;
    private float defaultFOV;

    public Transform adsPosition;
    public Transform hipPosition;
    public Transform gunModel;
    // -------------------------

    public void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        if (reloadBarBG != null)
            reloadBarBG.SetActive(false);

        if (beam != null)
            beam.enabled = false;

        if (beamLight != null)
            beamLight.enabled = false;

        // ADS
        defaultFOV = cam.fieldOfView;

        // FIX: Start gun at hip position
        if (gunModel != null && hipPosition != null)
        {
            gunModel.localPosition = hipPosition.localPosition;
            gunModel.localRotation = hipPosition.localRotation;
        }
    }

    void Update()
    {
        if (isReloading)
            return;

        HandleADS();

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButtonDown(0) && canFire)
        {
            StartCoroutine(FireRailgun());
        }
    }

    // -------------------------
    // FIXED ADS SYSTEM
    // -------------------------
    void HandleADS()
    {
        bool aiming = Input.GetMouseButton(1);

        // Smooth camera zoom
        float targetFOV = aiming ? adsFOV : defaultFOV;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * adsSpeed);

        // Smooth gun movement (LOCAL SPACE)
        if (gunModel != null && adsPosition != null && hipPosition != null)
        {
            Transform target = aiming ? adsPosition : hipPosition;

            gunModel.localPosition = Vector3.Lerp(
                gunModel.localPosition,
                target.localPosition,
                Time.deltaTime * adsSpeed
            );

            gunModel.localRotation = Quaternion.Lerp(
                gunModel.localRotation,
                target.localRotation,
                Time.deltaTime * adsSpeed
            );
        }
    }
    // -------------------------

    IEnumerator FireRailgun()
    {
        if (currentAmmo <= 0)
            yield break;

        canFire = false;
        currentAmmo--;
        UpdateAmmoUI();

        if (chargeFlash != null)
            chargeFlash.Play();

        yield return new WaitForSeconds(0.15f);

        if (fireFlash != null)
            fireFlash.Play();

        if (fireSound != null)
            fireSound.Play();

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit[] hits = Physics.RaycastAll(ray, range);

        Vector3 endPoint = ray.origin + ray.direction * range;
        if (hits.Length > 0)
            endPoint = hits[0].point;

        StartCoroutine(FireBeam(ray.origin, endPoint));
        StartCoroutine(FlashLight());

        if (camShake != null)
            StartCoroutine(camShake.Shake(shakeDuration, shakeMagnitude));

        int pierced = 0;
        foreach (RaycastHit hit in hits)
        {
            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                pierced++;

                if (pierced >= maxPierce)
                    break;
            }
        }

        yield return new WaitForSeconds(cooldown);
        canFire = true;
    }

    IEnumerator FireBeam(Vector3 start, Vector3 end)
    {
        if (beam == null)
            yield break;

        beam.enabled = true;
        beam.SetPosition(0, start);
        beam.SetPosition(1, end);

        yield return new WaitForSeconds(beamDuration);

        beam.enabled = false;
    }

    IEnumerator FlashLight()
    {
        if (beamLight == null)
            yield break;

        beamLight.enabled = true;
        yield return new WaitForSeconds(lightDuration);
        beamLight.enabled = false;
    }

    IEnumerator Reload()
    {
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

        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        if (reloadBarBG != null)
            reloadBarBG.SetActive(false);

        if (reloadBarFill != null)
            reloadBarFill.fillAmount = 0f;

        isReloading = false;
    }

   public void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo + " / " + maxAmmo;
    }
}
