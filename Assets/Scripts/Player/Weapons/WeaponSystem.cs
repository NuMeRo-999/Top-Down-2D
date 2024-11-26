using System.Collections;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public Weapon equippedWeapon;
    public Transform firePoint;
    public PlayerAim playerAim;


    public void EquipWeapon(Weapon newWeapon)
    {
        equippedWeapon = newWeapon;
        // Debug.Log($"Arma equipada: {equippedWeapon.weaponName}");
    }

    public void UnequipWeapon()
    {
        equippedWeapon = null;
        Debug.Log("Arma desequipada.");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (equippedWeapon != null && playerAim.isAiming)
            {
                Fire();
            }
            else
            {
                Debug.Log("No hay arma equipada o no estás apuntando.");
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (equippedWeapon != null)
                StartCoroutine(Reload());
        }
    }

    void Fire()
    {
        if (equippedWeapon.currentAmmo > 0)
        {
            equippedWeapon.currentAmmo--;

            switch (equippedWeapon.type)
            {
                case WeaponType.Pistol:
                case WeaponType.Revolver:
                    FireBullet(equippedWeapon.range);
                    break;

                case WeaponType.Shotgun:
                    FireShotgun();
                    break;

                case WeaponType.RocketLauncher:
                    FireRocket();
                    break;

                case WeaponType.Explosive:
                    ThrowExplosive();
                    break;
            }
        }
        else
        {
            Debug.Log("Sin munición. Recarga primero.");
        }
    }

    IEnumerator Reload()
    {
        if (equippedWeapon.totalAmmo > 0)
        {
            Debug.Log("Recargando...");
            yield return new WaitForSeconds(equippedWeapon.reloadTime);

            int neededAmmo = equippedWeapon.maxAmmo - equippedWeapon.currentAmmo;
            int ammoToReload = Mathf.Min(neededAmmo, equippedWeapon.totalAmmo);

            equippedWeapon.currentAmmo += ammoToReload;
            equippedWeapon.totalAmmo -= ammoToReload;

            Debug.Log("Recarga completada.");
        }
        else
        {
            Debug.Log("No hay munición suficiente para recargar.");
        }
    }

    void FireBullet(float range)
    {
        Debug.Log($"Disparando bala con alcance {range}.");
        // Instanciar bala y aplicar lógica
    }

    void FireShotgun()
    {
        Shotgun shotgun = (Shotgun)equippedWeapon; // Cast a Shotgun
        for (int i = 0; i < shotgun.pelletCount; i++)
        {
            // Dispersión por cada perdigón
            float angle = Random.Range(-shotgun.spreadAngle, shotgun.spreadAngle);
            Vector3 direction = Quaternion.Euler(0, 0, angle) * transform.right;

            FireBullet(shotgun.range);
        }
    }

    void FireRocket()
    {
        RocketLauncher launcher = (RocketLauncher)equippedWeapon;
        Debug.Log($"Lanzando cohete con radio de explosión {launcher.explosionRadius}.");
        // Instanciar cohete y aplicar lógica de explosión
    }

    void ThrowExplosive()
    {
        Debug.Log("Lanzando explosivo.");
        // Instanciar explosivo y manejar temporizador/detonación
    }
}
