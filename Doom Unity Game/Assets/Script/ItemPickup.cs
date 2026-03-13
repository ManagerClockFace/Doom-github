using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public InventoryItem item;

    private void OnTriggerEnter(Collider other)
    {
        var inv = other.GetComponent<PlayerInventory>();
        if (inv == null) return;

        if (item.itemType == ItemType.Ammo)
            inv.AddAmmo(item.itemName, item.ammoAmount);

        if (item.itemType == ItemType.Healing)
            inv.AddMedkit(item);

        Destroy(gameObject);
    }
}
