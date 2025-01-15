using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Enemy") &&
            !other.gameObject.CompareTag("Bullet") &&
            !other.gameObject.CompareTag("Door") &&
            !other.gameObject.CompareTag("EnemyCollider") &&
            !other.gameObject.CompareTag("ChestTriggerCollider") &&
            !other.gameObject.CompareTag("BossArea") &&
            !other.gameObject.CompareTag("Teleport") &&
            !other.gameObject.CompareTag("CameraZoom"))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
