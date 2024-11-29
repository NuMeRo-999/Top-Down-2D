using UnityEngine;

public class Bullet : MonoBehaviour
{

    public WeaponSystem weaponSystem;
    
    void Start()
    {
        weaponSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponSystem>();
    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(weaponSystem.equippedWeapon.damage);
        }
    }
}
