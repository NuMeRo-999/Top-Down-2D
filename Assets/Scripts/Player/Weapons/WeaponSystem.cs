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
            ammoText.text = "ammo =" + equippedWeapon.currentAmmo.ToString() + "/" + equippedWeapon.totalAmmo.ToString();
            Debug.Log(equippedWeapon.type);

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
        // if (equippedWeapon is RocketLauncher rocketLauncher)
        // {
        Debug.Log("Lanzando cohete");
            GameObject rocket = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
            rocket.GetComponent<Rigidbody2D>().AddForce(firePoint.right * 20, ForceMode2D.Impulse);

            // rb.AddForce(firePoint.right * rocketLauncher.speed, ForceMode2D.Impulse);

            // Configurar destrucción tras un rango máximo si no impacta nada
            // Destroy(rocket, rocketLauncher.maxRange / rocketLauncher.speed);
        // }
    }

    void ThrowExplosive()
    {
        if (equippedWeapon is Explosive explosive)
        {
            float randomRotationZ = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomRotationZ);

            GameObject grenade = Instantiate(explosive.prefab, firePoint.position, randomRotation);

            StartCoroutine(MoveAndStop(grenade, explosive));
        }
    }


    IEnumerator MoveAndStop(GameObject grenade, Explosive explosive)
    {
        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = firePoint.right * 5;

        float elapsedTime = 0f;

        while (elapsedTime < explosive.moveTime)
        {
            elapsedTime += Time.deltaTime;

            Collider2D hit = Physics2D.OverlapCircle(grenade.transform.position, 0.1f);
            if (hit != null && !hit.CompareTag("Player"))
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
                break;
            }

            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        StartCoroutine(HandleExplosion(grenade, explosive));
    }


    IEnumerator HandleExplosion(GameObject grenade, Explosive explosive)
    {
        yield return new WaitForSeconds(explosive.detonationTime);

        Vector3 explosionPosition = grenade.transform.position;

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(explosionPosition, explosive.explosionRadius);

        foreach (Collider2D obj in hitObjects)
        {

            if (obj.tag == "Player" || obj.tag == "Enemy")
            {
                Player player = obj.GetComponent<Player>();
                if (player != null && player.currentHealth > 0)
                {
                    player.TakeDamage(explosive.damage);
                }

                Enemy enemy = obj.GetComponent<Enemy>();
                if (enemy != null && enemy.currentHealth > 0)
                {
                    enemy.TakeDamage(explosive.damage);
                }
            }
        }

        Destroy(grenade);

        Instantiate(explosive.explosionEffectPrefab, explosionPosition, Quaternion.identity);
    }
}
