using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float stopDistance = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float detectionRange = 10f; // Rango de detección

    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float bulletSpeed = 5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource movementAudioSource; // Fuente de audio para caminar
    [SerializeField] private AudioSource shootingAudioSource; // Fuente de audio para disparar
    [SerializeField] private AudioClip[] walkAudioClips; // Array de clips para caminar
    [SerializeField] private AudioClip[] shootAudioClips; // Array de clips para disparar
    [SerializeField] private float movementSoundInterval = 1f;

    private Animator animator;
    public float fireCooldown;
    private float movementSoundTimer;
    private bool canMoveTowardsPlayer = false; // Para saber si el enemigo puede moverse hacia el jugador

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        movementSoundTimer = movementSoundInterval;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Calcular la distancia entre el enemigo y el jugador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Si el jugador está dentro del rango de detección, permitir el movimiento hacia él
        if (distanceToPlayer <= detectionRange)
        {
            canMoveTowardsPlayer = true;
        }
        else
        {
            canMoveTowardsPlayer = false;
        }

        if (canMoveTowardsPlayer)
        {
            Vector2 directionToPlayer = ((Vector2)player.position - rb.position).normalized;

            if (distanceToPlayer > stopDistance)
            {
                animator.SetBool("Attack", false);
                rb.linearVelocity = directionToPlayer * speed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                animator.SetBool("Attack", true);
                Shoot();
            }

            HandleMovementSound();

            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            rb.rotation = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Si no está dentro del rango, detenerse
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Shoot()
    {
        if (fireCooldown <= 0f)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = firePoint.right * bulletSpeed;
            }

            if (shootAudioClips.Length > 0)
            {
                AudioClip clip = shootAudioClips[Random.Range(0, shootAudioClips.Length)];
                shootingAudioSource.PlayOneShot(clip);
            }

            fireCooldown = fireRate;
            Destroy(bullet, 3f);
        }

        fireCooldown -= Time.fixedDeltaTime;
    }

    private void HandleMovementSound()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            movementSoundTimer -= Time.fixedDeltaTime;
            if (movementSoundTimer <= 0f && walkAudioClips.Length > 0)
            {
                AudioClip clip = walkAudioClips[Random.Range(0, walkAudioClips.Length)];
                movementAudioSource.PlayOneShot(clip);
                movementSoundTimer = movementSoundInterval;
            }
        }
    }
}
