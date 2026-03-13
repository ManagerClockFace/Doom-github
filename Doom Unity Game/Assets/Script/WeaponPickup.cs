using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weapon;
    public int slotIndex; // 0=Primary, 1=Secondary

    private void OnTriggerEnter(Collider other)
    {
        var inv = other.GetComponent<PlayerInventory>();
        if (inv == null) return;

        inv.AddWeapon(weapon, slotIndex);
        Destroy(gameObject);
    }
}
