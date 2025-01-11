using System.Collections;
using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float stopDistance = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float bulletSpeed = 5f;
    
    private Animator animator;
    public float fireCooldown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 directionToPlayer = ((Vector2)player.position - rb.position).normalized;
        float distanceToPlayer = Vector2.Distance(player.position, rb.position);

        if (distanceToPlayer > stopDistance)
        {
            // Moverse hacia el jugador
            animator.SetBool("Attack", false);
            rb.linearVelocity = directionToPlayer * speed;
        }
        else
        {
            // Detenerse y disparar
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("Attack", true);
            Shoot();
        }

        // Rotar hacia el jugador
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        rb.rotation = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime);
    }

    private void Shoot()
    {
        if (fireCooldown <= 0f)
        {
        Debug.Log("Shooting...");
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = firePoint.right * bulletSpeed;
            }

            // Reiniciar el tiempo de enfriamiento
            fireCooldown = fireRate;

            // Destruir la bala después de cierto tiempo
            Destroy(bullet, 3f);
        }

        // Reducir el enfriamiento
        fireCooldown -= Time.fixedDeltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja el rango de ataque en la vista de escena
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
