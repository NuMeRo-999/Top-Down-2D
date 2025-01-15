using UnityEngine;

public class Bullet : MonoBehaviour
{

    public WeaponSystem weaponSystem;
    
    void Start()
    {
        weaponSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (!other.gameObject.CompareTag("Player") &&
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

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(weaponSystem.equippedWeapon.damage);
        }
    }
}
