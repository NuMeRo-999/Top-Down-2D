using UnityEngine;

public class Rocket : MonoBehaviour
{
    public RocketLauncher rocketLauncher;
    public float explosionRadius;
    public int damage;

    public GameObject explosionMarkPrefab;
    public GameObject wallExplosionMarkPrefab;
    public WeaponSystem weaponSystem;
    public AudioClip explosionAudioClip;

    private void Start()
    {
        weaponSystem = FindAnyObjectByType<WeaponSystem>();
        explosionRadius = rocketLauncher.explosionRadius;
        damage = rocketLauncher.damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;

        Vector2 contactPoint = collision.contacts[0].point;
        Vector2 collisionNormal = collision.contacts[0].normal;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, collisionNormal);
        rotation *= Quaternion.Euler(0, 0, 90);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Explode(contactPoint, rotation, rocketLauncher.explosionEffectPrefab);
            Instantiate(explosionMarkPrefab, contactPoint, rotation);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Explode(contactPoint, rotation, rocketLauncher.wallExplosionEffectPrefab);
            Instantiate(wallExplosionMarkPrefab, contactPoint, rotation);
        }
        else
        {
            Explode(contactPoint, rotation, rocketLauncher.wallExplosionEffectPrefab);
            Instantiate(wallExplosionMarkPrefab, contactPoint, rotation);
        }

        Destroy(gameObject);
    }

    private void Explode(Vector2 explosionPosition, Quaternion rotation, GameObject explosionEffect)
    {
        Instantiate(explosionEffect, explosionPosition, rotation);
        weaponSystem.ShakeCamera(2f, 0.3f);

        // Crear un AudioSource dinámico para reproducir el sonido de la explosión
        GameObject soundObject = new GameObject("ExplosionSound");
        soundObject.transform.position = explosionPosition;

        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();
        tempAudioSource.clip = explosionAudioClip;
        tempAudioSource.volume = 1.0f; // Configura el volumen según sea necesario
        tempAudioSource.spatialBlend = 1.0f; // Hacer que el sonido sea 3D
        tempAudioSource.Play();

        Destroy(soundObject, explosionAudioClip.length); // Eliminar el objeto una vez que termine el sonido

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius);

        foreach (Collider2D obj in hitObjects)
        {
            if (obj.CompareTag("Player"))
            {
                Player player = obj.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
            }

            if (obj.CompareTag("Enemy"))
            {
                Enemy enemy = obj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}
