using UnityEngine;

[System.Serializable]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public Sprite icon;
    public int damage;
    public float fireRate;
    public float speed;
    public float range;
    public int maxAmmo;
    public int currentAmmo;
    public int totalAmmo;
    public float reloadTime;
    public WeaponType type;
}

public enum WeaponType
{
    Pistol,
    Revolver,
    Shotgun,
    Explosive,
    RocketLauncher
}
