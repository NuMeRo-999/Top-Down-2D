using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
  public int currentHealth;
  [SerializeField] int maxHealth = 100;
  [SerializeField] TextMeshProUGUI healthText;

  private Animator animator;
  private BloodParticles bloodParticles;

  void Start()
  {
    currentHealth = maxHealth;

    animator = GetComponent<Animator>();
    bloodParticles = GetComponent<BloodParticles>();
    healthText.text = "health=" + currentHealth.ToString() + "/" + maxHealth.ToString();
  }

  void Update()
  {
    
  }

  public void TakeDamage(int damage)
  {

    bloodParticles.SpawnBloodParticlesAndStain();

    currentHealth -= damage;
    healthText.text = "health=" + currentHealth.ToString() + "/" + maxHealth.ToString();

    if (currentHealth <= 0) Die();
  }

  public void Die()
  {
    animator.SetBool("isDead", true);
    Debug.Log("Player died");
    bloodParticles.SpawnBloodBurst();
    GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    GetComponent<SpriteRenderer>().sortingOrder = -1;
    GetComponent<PlayerMovement>().enabled = false;
    GetComponent<PlayerAttack>().enabled = false;
    GetComponent<Collider2D>().enabled = false;
  }
}
