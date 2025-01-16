using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float stopDistance = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float detectionRange = 10f;

    [Header("Minigun Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private float minigunSpinUpTime = 1f;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletSpread = 5f;
    [SerializeField] private float overheatTime = 5f;
    [SerializeField] private float cooldownTime = 3f;

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
    private bool canMoveTowardsPlayer = false;
    private bool isRespawning = false;
    private AudioClip lastPlayedClip = null; // Clip reproducido más recientemente
    public Enemy enemy;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
    }

    void FixedUpdate()
    {
        if (player == null || isRespawning) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        canMoveTowardsPlayer = distanceToPlayer <= detectionRange;

        if (canMoveTowardsPlayer)
        {
            Vector2 directionToPlayer = ((Vector2)player.position - rb.position).normalized;

            if (distanceToPlayer > stopDistance)
            {
                rb.linearVelocity = directionToPlayer * speed;
                StopShooting();
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                StartShooting(directionToPlayer);
            }

            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            rb.rotation = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            StopShooting();
        }
    }

    private void StartShooting(Vector2 directionToPlayer)
    {
        if (isRespawning || isShooting || isOverheated) return;

        isShooting = true;
        isSpinningUp = true;

        PlaySound(minigunStartSound);

        Invoke(nameof(EnableContinuousFire), minigunSpinUpTime);
    }

    private void EnableContinuousFire()
    {
        if (isOverheated) return;

        isSpinningUp = false;
        PlaySound(minigunLoopSound, loop: true);
        animator.SetBool("Attack", true);
    }

    private void StopShooting()
    {
        if (!isShooting && !minigunAudioSource.isPlaying) return;

        isShooting = false;
        isSpinningUp = false;

        CancelInvoke(nameof(EnableContinuousFire));

        if (minigunAudioSource.loop)
        {
            minigunAudioSource.loop = false;
            minigunAudioSource.Stop();
        }

        PlaySound(minigunEndSound);
    }

    private void Update()
    {
        if (isOverheated)
        {
            overheatTimer -= Time.deltaTime;
            if (overheatTimer <= 0f)
            {
                isOverheated = false;
                Debug.Log("Overheat cooldown finished.");
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

        StopShooting();
        PlaySound(overheatSound);

        overheatTimer = cooldownTime;
    }

    private void FireBullet()
    {
        Transform firePoint = firePoints[currentFirePointIndex];
        currentFirePointIndex = (currentFirePointIndex + 1) % firePoints.Length;

        float randomSpread = Random.Range(-bulletSpread, bulletSpread);
        Quaternion spreadRotation = Quaternion.Euler(0, 0, randomSpread);
        Quaternion bulletRotation = firePoint.rotation * spreadRotation;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity = bulletRotation * Vector2.right * bulletSpeed;
        }

        Destroy(bullet, 3f);
    }

    public void Respawn()
    {
        isRespawning = true;
        ResetAudioState();
        StartCoroutine(EndRespawnCooldown());
    }

    private IEnumerator EndRespawnCooldown()
    {
        yield return new WaitForSeconds(1f);
        isRespawning = false;
    }

    public void ResetAudioState()
    {
        if (minigunAudioSource.isPlaying)
        {
            minigunAudioSource.Stop();
        }

        minigunAudioSource.loop = false;
        minigunAudioSource.clip = null;
        lastPlayedClip = null;
        Debug.Log("Audio state reset after respawn.");
    }

    private void PlaySound(AudioClip clip, bool loop = false)
    {
        if (clip == null || lastPlayedClip == clip) return;

        minigunAudioSource.Stop();
        minigunAudioSource.clip = clip;
        minigunAudioSource.loop = loop;
        minigunAudioSource.Play();
        lastPlayedClip = clip;
        Debug.Log($"Playing sound: {clip.name}, loop: {loop}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
