using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string weaponName;
    public Sprite icon;
    public int damage;
    public float fireRate;
    public float range;
    public int maxAmmo; // Munición máxima que puede cargar
    public int currentAmmo; // Munición cargada actualmente
    public int totalAmmo; // Munición total disponible para recargar
    public float reloadTime; // Tiempo de recarga
    public WeaponType type; // Tipo de arma (pistola, escopeta, etc.)
}

public enum WeaponType
{
    Pistol,
    Revolver,
    Shotgun,
    Explosive,
    RocketLauncher
}
