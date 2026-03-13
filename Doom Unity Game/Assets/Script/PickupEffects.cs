using UnityEngine;

public class PickupEffects : MonoBehaviour
{
    [Header("Spin Settings")]
    public float spinSpeed = 90f; // degrees per second

    [Header("Bob Settings")]
    public bool enableBob = true;
    public float bobHeight = 0.25f;
    public float bobSpeed = 2f;

    [Header("Audio")]
    public AudioSource pickupSound;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        // Spin
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.World);

        // Bob
        if (enableBob)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
        }
    }

    // Call this from your pickup script when collected
    public void PlayPickupSound()
    {
        if (pickupSound != null)
            pickupSound.Play();
    }
}
