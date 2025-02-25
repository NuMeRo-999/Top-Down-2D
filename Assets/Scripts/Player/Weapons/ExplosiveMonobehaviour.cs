using System.Collections;
using UnityEngine;

public class ExplosiveMonobehaviour : MonoBehaviour
{
    private Rigidbody2D rb;

    public WeaponSystem weaponSystem;
    public AudioClip explosionAudioClip;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponSystem = FindAnyObjectByType<WeaponSystem>();
    }

    public IEnumerator MoveAndStop(Transform firePoint, GameObject grenade, Explosive explosive)
    {
        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = firePoint.right * 5;

        float elapsedTime = 0f;

        while (elapsedTime < explosive.moveTime)
        {
            elapsedTime += Time.deltaTime;

            Collider2D hit = Physics2D.OverlapCircle(grenade.transform.position, 0.3f);
            if (hit != null && !hit.CompareTag("Player") && !hit.isTrigger)
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

    public IEnumerator HandleExplosion(GameObject grenade, Explosive explosive)
    {
        yield return new WaitForSeconds(explosive.detonationTime);

        Vector3 explosionPosition = grenade.transform.position;

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(explosionPosition, explosive.explosionRadius);

        weaponSystem.ShakeCamera(2f, 0.3f);

        // Crear un AudioSource temporal para reproducir el sonido
        GameObject soundObject = new GameObject("ExplosionSound");
        soundObject.transform.position = explosionPosition;

        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();
        tempAudioSource.clip = explosionAudioClip;
        tempAudioSource.volume = 1.0f; // Configura el volumen según lo necesites
        tempAudioSource.spatialBlend = 1.0f; // 3D sound
        tempAudioSource.Play();

        Destroy(soundObject, explosionAudioClip.length); // Destruir el objeto después de que termine el sonido

        foreach (Collider2D obj in hitObjects)
        {
            if (obj.tag is "Player" or "Enemy")
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
