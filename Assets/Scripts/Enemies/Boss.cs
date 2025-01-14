using UnityEngine;

public class Boss : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float stopDistance = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Minigun Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform[] firePoints; // Múltiples puntos de disparo
    [SerializeField] private float minigunSpinUpTime = 1f;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletSpread = 5f;
    [SerializeField] private float overheatTime = 5f; // Tiempo antes de sobrecalentarse
    [SerializeField] private float cooldownTime = 3f; // Tiempo para enfriarse

    [Header("Audio Settings")]
    [SerializeField] public AudioSource minigunAudioSource;
    [SerializeField] private AudioClip minigunStartSound;
    [SerializeField] private AudioClip minigunLoopSound;
    [SerializeField] private AudioClip minigunEndSound;
    [SerializeField] private AudioClip overheatSound;

    private Animator animator;
    private float fireCooldown;
    private bool isShooting;
    private bool isSpinningUp;
    private bool isOverheated;
    private float overheatTimer;
    private int currentFirePointIndex = 0;

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
            // animator.SetBool("Attack", false);
            rb.linearVelocity = directionToPlayer * speed;
            StopShooting();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            // animator.SetBool("Attack", true);
            StartShooting(directionToPlayer);
        }

        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        rb.rotation = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime);
    }

    private void StartShooting(Vector2 directionToPlayer)
    {
        if (!isShooting && !isOverheated)
        {
            isShooting = true;
            isSpinningUp = true;

            if (minigunStartSound != null)
                minigunAudioSource.PlayOneShot(minigunStartSound);

            Invoke(nameof(EnableContinuousFire), minigunSpinUpTime);
        }
    }

    private void EnableContinuousFire()
    {
        if (isOverheated) return;


        isSpinningUp = false;
        if (minigunLoopSound != null)
        {
            minigunAudioSource.loop = true;
            animator.SetBool("Attack", true);
            minigunAudioSource.clip = minigunLoopSound;
            minigunAudioSource.Play();
        }
    }

    private void StopShooting()
    {
        if (isShooting || minigunAudioSource.isPlaying)
        {
            isShooting = false;
            isSpinningUp = false;

            CancelInvoke(nameof(EnableContinuousFire));

            if (minigunAudioSource.loop)
            {
                minigunAudioSource.loop = false;
                minigunAudioSource.Stop(); // Detener cualquier sonido en curso
            }

            if (minigunEndSound != null)
                minigunAudioSource.PlayOneShot(minigunEndSound);
        }
    }


    private void Update()
    {
        if (isOverheated)
        {
            overheatTimer -= Time.deltaTime;
            if (overheatTimer <= 0f)
            {
                isOverheated = false;
                if (minigunEndSound != null)
                    minigunAudioSource.PlayOneShot(minigunEndSound);
            }
            return;
        }

        if (isShooting && !isSpinningUp)
        {
            overheatTimer += Time.deltaTime;
            if (overheatTimer >= overheatTime)
            {
                Overheat();
                return;
            }

            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                FireBullet();
                fireCooldown = fireRate;
            }
        }
    }

    private void Overheat()
    {
        isOverheated = true;
        isShooting = false;
        animator.SetBool("Attack", false);

        // Detener el disparo y el sonido en bucle
        StopShooting();

        // Reproducir el sonido de sobrecalentamiento
        if (overheatSound != null)
        {
            minigunAudioSource.loop = false; // Asegurar que el bucle se detenga
            minigunAudioSource.Stop();      // Detener el sonido actual
            minigunAudioSource.PlayOneShot(overheatSound); // Reproducir sobrecalentamiento
        }

        overheatTimer = cooldownTime; // Iniciar el tiempo de enfriamiento
    }


    private void FireBullet()
    {
        // Alternar entre los puntos de disparo
        Transform firePoint = firePoints[currentFirePointIndex];
        currentFirePointIndex = (currentFirePointIndex + 1) % firePoints.Length;

        // Variación aleatoria en ángulo
        float randomSpread = Random.Range(-bulletSpread, bulletSpread);

        // Calcular nueva rotación con la variación
        Quaternion spreadRotation = Quaternion.Euler(0, 0, randomSpread);
        Quaternion bulletRotation = firePoint.rotation * spreadRotation;

        // Instanciar la bala con la nueva rotación
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity = bulletRotation * Vector2.right * bulletSpeed;
        }

        Destroy(bullet, 3f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
