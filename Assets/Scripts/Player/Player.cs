using UnityEngine;

public class Player : MonoBehaviour
{
  public int currentHealth;
  [SerializeField] int maxHealth = 100;

  private Animator animator;

  void Start()
  {
    currentHealth = maxHealth;

    animator = GetComponent<Animator>();
  }

  void Update()
  {
    if(currentHealth <= 0) Die();
  }

  public void TakeDamage(int damage) {

    // no hay animación salen particulas de sangre junto al sonido de pegar de los enemigos

    currentHealth -= damage;

    if(currentHealth <= 0) Die();
  }

  public void Die()
  {
    animator.SetBool("isDead", true);
    GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    GetComponent<SpriteRenderer>().sortingOrder = -1;
    GetComponent<PlayerMovement>().enabled = false;
    GetComponent<PlayerAttack>().enabled = false;
    GetComponent<Collider2D>().enabled = false;
  }
}
