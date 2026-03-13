using UnityEngine;

public class DoomController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public CharacterController controller;

    [Header("Mouse Look")]
    public float mouseSensitivity = 3f;
    float yaw;

    [Header("Headbob")]
    public Transform cam;
    public float bobSpeed = 8f;
    public float bobAmount = 0.05f;
    float bobTimer;

    [Header("Gravity & Jumping")]
    public float gravity = -20f;
    public float jumpForce = 8f;
    float verticalVelocity;

    PlayerInventory inventory;
    PlayerHealth health;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (!controller) controller = GetComponent<CharacterController>();

        inventory = GetComponent<PlayerInventory>();
        health = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
        HandleHeadbob();
        HandleInventoryInput();
    }

    void HandleInventoryInput()
    {
        // Weapon switching
        if (Input.GetKeyDown(KeyCode.Alpha1)) inventory.SwitchWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) inventory.SwitchWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) inventory.SwitchWeapon(2);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) inventory.SwitchWeapon((inventory.currentWeaponIndex + 1) % 3);
        if (scroll < 0f) inventory.SwitchWeapon((inventory.currentWeaponIndex + 2) % 3);

        // Medkit usage
        if (Input.GetKeyDown(KeyCode.Q))
            inventory.UseMedkit(health);
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        yaw += mouseX;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

    void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z).normalized * moveSpeed;

        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f;

            if (Input.GetButtonDown("Jump"))
                verticalVelocity = jumpForce;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);
    }

    void HandleHeadbob()
    {
        Vector3 localPos = cam.localPosition;

        if (controller.velocity.magnitude > 0.1f && controller.isGrounded)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            localPos.y = Mathf.Sin(bobTimer) * bobAmount;
        }
        else
        {
            bobTimer = 0f;
            localPos.y = 0f;
        }

        cam.localPosition = localPos;
    }
}
