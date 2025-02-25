using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public Weapon equippedWeapon;
    public Transform firePoint;
    public PlayerAim playerAim;
    public TextMeshProUGUI ammoText;
    private Inventory inventory;

    public GameObject bulletPrefab;
    public CinemachineCamera cinemachineCamera;

    [Header("Audio Sources")]
    public AudioSource rocketAudioSource;
    public AudioSource pistolAudioSource;
    public AudioSource shotgunAudioSource;
    public AudioSource revolverAudioSource;
    public AudioSource pistolReloadAudioSource;
    public AudioSource shotgunReloadAudioSource;
    public AudioSource rocketReloadAudioSource;

    private float nextFireTime = 0f;
    private bool isReloading = false;

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
        // Verifica si se puede disparar
        if (Time.time >= nextFireTime)
        {
            if (equippedWeapon.currentAmmo > 0)
            {
                // Actualiza el tiempo del próximo disparo
                nextFireTime = Time.time + (1f / equippedWeapon.fireRate);

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
                if (equippedWeapon != null)
                    StartCoroutine(Reload());
            }
        }
        else
        {
            Debug.Log("Esperando para disparar...");
        }
    }

    public void ShakeCamera(float intensity, float duration)
    {
        if (cinemachineCamera != null)
        {
            var perlin = cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

            if (perlin != null)
            {
                perlin.AmplitudeGain = intensity;
                StartCoroutine(ResetCameraShake(perlin, duration));
            }
            else
            {
                Debug.LogWarning("No se encontró CinemachineBasicMultiChannelPerlin en la cámara.");
            }
        }
    }

    IEnumerator ResetCameraShake(CinemachineBasicMultiChannelPerlin perlin, float duration)
    {
        yield return new WaitForSeconds(duration);

        perlin.AmplitudeGain = 0f;
    }


    IEnumerator Reload()
    {
        if (equippedWeapon.totalAmmo > 0)
        {
            if (equippedWeapon.name == "Pistol" && !isReloading)
            {
                pistolReloadAudioSource.Play();
            }
            else if (equippedWeapon.name == "Revolver" && !isReloading)
            {
                pistolReloadAudioSource.Play();
            }
            else if (equippedWeapon.name == "Shotgun" && !isReloading)
            {
                shotgunReloadAudioSource.Play();
            }
            else if (equippedWeapon.name == "RPG" && !isReloading)
            {
                rocketReloadAudioSource.Play();
            }
            isReloading = true;

            ammoText.text = "reloading...";
            ammoText.color = Color.red;
            yield return new WaitForSeconds(equippedWeapon.reloadTime);

            int neededAmmo = equippedWeapon.maxAmmo - equippedWeapon.currentAmmo;
            int ammoToReload = Mathf.Min(neededAmmo, equippedWeapon.totalAmmo);

            equippedWeapon.currentAmmo += ammoToReload;
            equippedWeapon.totalAmmo -= ammoToReload;

            ammoText.text = "ammo =" + equippedWeapon.currentAmmo.ToString() + "/" + equippedWeapon.totalAmmo.ToString();
            ammoText.color = Color.white;
            isReloading = false;

            Debug.Log("Recarga completada.");
        }
        else
        {
            Debug.Log("No hay munición suficiente para recargar.");
        }
    }

    void FireBullet(float range)
    {
        ShakeCamera(1.5f, 0.2f);
        pistolAudioSource.Play();
        Debug.Log(equippedWeapon.name);
        if (equippedWeapon.name == "Pistol")
        {
            pistolAudioSource.Play();
        }
        else if (equippedWeapon.name == "Revolver")
        {
            revolverAudioSource.Play();
        }
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * equippedWeapon.speed, ForceMode2D.Impulse);
        Destroy(bullet, range);
    }

    void FireShotgun()
    {
        ShakeCamera(1.5f, 0.2f);
        shotgunAudioSource.Play();
        if (equippedWeapon is Shotgun shotgun)
        {
            float angleStep = shotgun.spreadAngle / (shotgun.pelletCount - 1);
            float angle = -shotgun.spreadAngle / 2;

            for (int i = 0; i < shotgun.pelletCount; i++)
            {
                Vector3 direction = Quaternion.Euler(0, 0, angle) * transform.up;

                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, angle));
                bullet.GetComponent<Rigidbody2D>().AddForce(direction * equippedWeapon.speed, ForceMode2D.Impulse);

                Destroy(bullet, shotgun.range);

                angle += angleStep;
            }
        }
    }

    void FireRocket()
    {
        ShakeCamera(1.5f, 0.2f);
        rocketAudioSource.Play();
        if (equippedWeapon is RocketLauncher rocketLauncher)
        {
            Debug.Log("Lanzando cohete");
            GameObject rocket = Instantiate(rocketLauncher.rocketPrefab, firePoint.position, firePoint.rotation);

            Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
            rocket.GetComponent<Rigidbody2D>().AddForce(firePoint.right * 20, ForceMode2D.Impulse);
        }
    }

    void ThrowExplosive()
    {
        if (equippedWeapon is Explosive explosive)
        {
            float randomRotationZ = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomRotationZ);

            GameObject grenade = Instantiate(explosive.prefab, firePoint.position, randomRotation);

            StartCoroutine(grenade.GetComponent<ExplosiveMonobehaviour>().MoveAndStop(firePoint, grenade, explosive));
        }
    }

}
