using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public Sprite icon;
    public bool isWeapon;
    public Weapon weaponStats;
    public bool isHealingItem;
    public int healingAmount;
}
