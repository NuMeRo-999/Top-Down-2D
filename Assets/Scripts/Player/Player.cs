using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
  public int currentHealth;
  [SerializeField] int maxHealth = 100;
  [SerializeField] TextMeshProUGUI healthText;

  private Animator animator;
  private BloodParticles bloodParticles;
  public WeaponSystem weaponSystem;

  void Start()
  {
    currentHealth = maxHealth;

    animator = GetComponent<Animator>();
    bloodParticles = GetComponent<BloodParticles>();
    weaponSystem = GetComponent<WeaponSystem>();
    healthText.text = "health=" + currentHealth.ToString() + "/" + maxHealth.ToString();
  }

  public void TakeDamage(int damage)
  {

    bloodParticles.SpawnBloodParticlesAndStain();
    weaponSystem.ShakeCamera(1f, 0.3f);

    currentHealth -= damage;
    healthText.text = "health=" + currentHealth.ToString() + "/" + maxHealth.ToString();

    if (currentHealth <= 0) Die();
  }

  public void Heal(int healAmount)
  {

    if (Input.GetMouseButtonDown(0))
    {
      currentHealth += healAmount;
      if (currentHealth > maxHealth) currentHealth = maxHealth;
      healthText.text = "health=" + currentHealth.ToString() + "/" + maxHealth.ToString();
    }
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
