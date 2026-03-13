using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public Weapon[] weaponSlots = new Weapon[3];   // 0=Primary, 1=Secondary, 2=Knife
    public int currentWeaponIndex = 0;

    public Dictionary<string, int> ammo = new Dictionary<string, int>();
    public List<InventoryItem> medkits = new List<InventoryItem>();

    void Start()
    {
        if (weaponSlots[2] == null)
            Debug.LogWarning("Knife slot is empty! Assign a knife weapon in the Inspector.");
    }

    public void AddAmmo(string ammoType, int amount)
    {
        if (!ammo.ContainsKey(ammoType))
            ammo[ammoType] = 0;

        ammo[ammoType] += amount;
    }

    public void AddMedkit(InventoryItem medkit)
    {
        medkits.Add(medkit);
    }

    public void UseMedkit(PlayerHealth health)
    {
        if (medkits.Count == 0) return;

        var kit = medkits[0];
        health.Heal(kit.healAmount);
        medkits.RemoveAt(0);
    }

    public void AddWeapon(Weapon weapon, int slotIndex)
    {
        if (slotIndex == 2)
        {
            Debug.LogWarning("Knife slot cannot be replaced.");
            return;
        }

        weaponSlots[slotIndex] = weapon;
    }

    public void SwitchWeapon(int index)
    {
        if (weaponSlots[index] == null) return;
        currentWeaponIndex = index;
    }

    public Weapon GetCurrentWeapon()
    {
        return weaponSlots[currentWeaponIndex];
    }
}
