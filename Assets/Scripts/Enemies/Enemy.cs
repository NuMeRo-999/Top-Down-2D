using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int currentHealth;
    [SerializeField] int maxHealth = 20;
    [SerializeField] float attackDamage = 10;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] Transform attackPoint;

    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {

    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hit");

        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        animator.SetBool("isDead", true);
        this.enabled = false;
        GetComponent<EnemyMovement>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    private void OnDrawGizmos()
    {

        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
