using UnityEngine;

public enum ItemType { Ammo, Healing }

[CreateAssetMenu(menuName = "Item/Inventory Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public int ammoAmount;
    public int healAmount;
}
