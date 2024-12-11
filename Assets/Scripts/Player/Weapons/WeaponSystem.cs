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
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * 20, ForceMode2D.Impulse);
        Destroy(bullet, range);
    }

    void FireShotgun()
    {
        if (equippedWeapon is Shotgun shotgun)
        {
            float angleStep = shotgun.spreadAngle / (shotgun.pelletCount - 1);
            float angle = -shotgun.spreadAngle / 2;

            for (int i = 0; i < shotgun.pelletCount; i++)
            {
                Debug.Log(angle);
                Vector3 direction = Quaternion.Euler(0, 0, angle) * transform.up;

                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, angle));
                bullet.GetComponent<Rigidbody2D>().AddForce(direction * 20, ForceMode2D.Impulse);

                Destroy(bullet, shotgun.range);

                angle += angleStep;
            }
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
        if (equippedWeapon is Explosive explosive)
        {
            // Generar una rotación aleatoria en el eje Z
            float randomRotationZ = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomRotationZ);

            // Instanciar la bomba con la rotación aleatoria
            GameObject grenade = Instantiate(explosive.prefab, firePoint.position, randomRotation);

            // Mover la bomba una pequeña distancia hacia adelante
            StartCoroutine(MoveAndStop(grenade, explosive));
        }
    }


    IEnumerator MoveAndStop(GameObject grenade, Explosive explosive)
    {
        // Obtener la posición inicial
        Vector3 startPosition = grenade.transform.position;
        Vector3 targetPosition = startPosition + firePoint.right * explosive.throwDistance;

        float elapsedTime = 0f;

        // Mover el explosivo hacia adelante durante un tiempo breve
        while (elapsedTime < explosive.moveTime)
        {
            grenade.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / explosive.moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que el explosivo se detiene exactamente en la posición objetivo
        grenade.transform.position = targetPosition;

        // Iniciar la explosión tras el temporizador
        StartCoroutine(HandleExplosion(grenade, explosive));
    }


    IEnumerator HandleExplosion(GameObject grenade, Explosive explosive)
    {
        // Esperar el tiempo de la detonación
        yield return new WaitForSeconds(explosive.detonationTime);

        // Obtener la posición de la explosión
        Vector3 explosionPosition = grenade.transform.position;

        // Detectar colisiones dentro del radio de explosión
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(explosionPosition, explosive.explosionRadius);
        // foreach (Collider2D obj in hitObjects)
        // {
        //     // Aplicar daño si el objeto tiene un componente de salud
        //     Health health = obj.GetComponent<Health>();
        //     if (health != null)
        //     {
        //         health.TakeDamage(explosive.damage);
        //     }
        // }

        // Instanciar el sprite de la marca de explosión
        Instantiate(explosive.explosionMarkPrefab, explosionPosition, Quaternion.identity);

        // Destruir la bomba después de la explosión
        Destroy(grenade);

        // Efecto visual y/o sonoro de explosión
        Instantiate(explosive.explosionEffectPrefab, explosionPosition, Quaternion.identity);
    }

}
