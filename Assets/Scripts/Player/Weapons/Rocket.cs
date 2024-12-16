using UnityEngine;

public class Rocket : MonoBehaviour
{
    public RocketLauncher rocketLauncher;
    public float explosionRadius;
    public int damage;

    public GameObject explosionMarkPrefab;
    public GameObject wallExplosionMarkPrefab;

    private void Start()
    {
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

        Destroy(gameObject);
    }

    private void Explode(Vector2 explosionPosition, Quaternion rotation, GameObject explosionEffect)
    {
        Instantiate(explosionEffect, explosionPosition, rotation);

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
