using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int currentHealth;
    [SerializeField] int maxHealth = 20;
    [SerializeField] int attackDamage = 10;
    [SerializeField] float attackRange = 2f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] LayerMask attackLayer;

    private BloodParticles bloodParticles;
    private Animator animator;
    private float lastAttackTime;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        bloodParticles = GetComponent<BloodParticles>();
    }

    void Update()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        // Crear el raycast y excluir el collider del enemigo usando el LayerMask
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, attackRange, attackLayer);

        Debug.DrawLine(transform.position, transform.position + transform.right * attackRange, Color.red);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Player") && !hit.collider.isTrigger)
            {
                animator.SetTrigger("Attack");
                hit.collider.GetComponent<Player>().TakeDamage(attackDamage);
                return;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Generar las partículas de sangre en la posición del enemigo
        bloodParticles.SpawnBloodParticlesAndStain();

        animator.SetTrigger("Hit");

        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        animator.SetBool("isDead", true);
        enabled = false;

        bloodParticles.SpawnBloodBurst();

        if (GetComponent<EnemyMovement>() != null)
        {
            GetComponent<EnemyMovement>().enabled = false;
        }
        else if (GetComponent<ShooterEnemy>() != null)
        {
            GetComponent<ShooterEnemy>().enabled = false;
        }
        
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }
    private void OnDrawGizmos()
    {
        // Dibujar el gizmo del raycast
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * attackRange);
    }
}
