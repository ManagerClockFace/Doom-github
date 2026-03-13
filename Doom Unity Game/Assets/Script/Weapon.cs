using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Info")]
    public string weaponName;
    public Sprite icon;

    [Header("Ammo Settings")]
    public string ammoType = "Bullets";   // Must match your InventoryItem ammo name
    public int ammoPerShot = 1;

    [Header("Magazine")]
    public int maxMagazineSize = 6;       // How many shots before reload

    [Header("Model")]
    public GameObject weaponModel;        // The visible gun model
}
