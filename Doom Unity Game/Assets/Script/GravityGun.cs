using UnityEngine;

public class GravityGun : MonoBehaviour
{
    [Header("References")]
    public Camera cam;              // Player camera
    public Transform holdPoint;     // Where objects are held

    [Header("Settings")]
    public float grabRange = 10f;   // Max distance to grab
    public float holdForce = 150f;  // Force pulling object to hold point
    public float launchForce = 800f; // Launch strength

    private Rigidbody heldObject;

    void Update()
    {
        // Right-click: grab or drop
        if (Input.GetMouseButtonDown(1))
        {
            if (heldObject == null)
                TryGrabObject();
            else
                DropObject();
        }

        // Left-click: launch
        if (Input.GetMouseButtonDown(0) && heldObject != null)
        {
            LaunchObject();
        }
    }

    void FixedUpdate()
    {
        if (heldObject != null)
        {
            MoveHeldObject();
        }
    }

    void TryGrabObject()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                heldObject = rb;

                // Disable gravity while held
                heldObject.useGravity = false;

                // Unity 6 damping system
                heldObject.linearDamping = 10f;
                heldObject.angularDamping = 10f;
            }
        }
    }

    void MoveHeldObject()
    {
        Vector3 direction = holdPoint.position - heldObject.position;
        heldObject.AddForce(direction * holdForce, ForceMode.Acceleration);
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            heldObject.useGravity = true;

            // Reset damping
            heldObject.linearDamping = 0f;
            heldObject.angularDamping = 0f;

            heldObject = null;
        }
    }

    void LaunchObject()
    {
        if (heldObject != null)
        {
            heldObject.useGravity = true;

            // Reset damping
            heldObject.linearDamping = 0f;
            heldObject.angularDamping = 0f;

            // Launch forward
            heldObject.AddForce(cam.transform.forward * launchForce, ForceMode.Impulse);

            heldObject = null;
        }
    }
}
