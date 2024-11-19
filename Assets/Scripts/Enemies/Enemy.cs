using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxHealth = 20;
    public int currentHealth;
    public float attackDamage = 10;
    public float attackRange = 0.5f;
    
    private Animator animator;
        
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
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
}
