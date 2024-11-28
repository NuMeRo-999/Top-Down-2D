using System.Collections;
using TMPro;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public Weapon equippedWeapon;
    public Transform firePoint;
    public PlayerAim playerAim;
    public TextMeshProUGUI ammoText;
    private Inventory inventory;

    public GameObject bulletPrefab;


    void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        equippedWeapon = newWeapon;
        ammoText.text = "ammo =" + equippedWeapon.currentAmmo.ToString() + "/" + equippedWeapon.totalAmmo.ToString();
        ammoText.enabled = true;
    }

    public void UnequipWeapon()
    {
        equippedWeapon = null;
        ammoText.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (equippedWeapon != null && playerAim.isAiming && inventory.selectedItem.isWeapon)
            {
                if (equippedWeapon.currentAmmo == 0) StartCoroutine(Reload());
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
            ammoText.text = "ammo =" + equippedWeapon.currentAmmo.ToString() + "/" + equippedWeapon.totalAmmo.ToString();

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

            ammoText.text = "reloading...";
            ammoText.color = Color.red;
            yield return new WaitForSeconds(equippedWeapon.reloadTime);

            int neededAmmo = equippedWeapon.maxAmmo - equippedWeapon.currentAmmo;
            int ammoToReload = Mathf.Min(neededAmmo, equippedWeapon.totalAmmo);

            equippedWeapon.currentAmmo += ammoToReload;
            equippedWeapon.totalAmmo -= ammoToReload;
            ammoText.text = "ammo =" + equippedWeapon.currentAmmo.ToString() + "/" + equippedWeapon.totalAmmo.ToString();
            ammoText.color = Color.white;

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
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, 90));
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * 20, ForceMode2D.Impulse);
        Destroy(bullet, range);
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
