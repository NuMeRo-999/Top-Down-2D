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
  public AudioSource healthAudioSource;
  public AudioSource deathAudioSource;

  private void Start()
  {
    currentHealth = maxHealth;
    animator = GetComponent<Animator>();
    bloodParticles = GetComponent<BloodParticles>();
    weaponSystem = GetComponent<WeaponSystem>();
    healthText.text = "health=" + currentHealth + "/" + maxHealth;
  }

  public void TakeDamage(int damage)
  {
    bloodParticles.SpawnBloodParticlesAndStain();
    weaponSystem.ShakeCamera(1f, 0.3f);

    currentHealth -= damage;
    healthText.text = "health=" + currentHealth + "/" + maxHealth;

    if (currentHealth <= 0) Die();
  }

  public void Heal(int healAmount)
  {
    if (Input.GetMouseButtonDown(0))
    {
      healthAudioSource.Play();
      currentHealth += healAmount;
      if (currentHealth > maxHealth) currentHealth = maxHealth;
      healthText.text = "health=" + currentHealth + "/" + maxHealth;
    }
  }

  public void Die()
  {
    animator.SetBool("isDead", true);
    deathAudioSource.Play();
    bloodParticles.SpawnBloodBurst();

    // Detener controles
    GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    GetComponent<PlayerMovement>().enabled = false;
    GetComponent<PlayerAttack>().enabled = false;
    GetComponent<Collider2D>().enabled = false;

    // Reiniciar al spawn
    Respawn();
  }

  private void Respawn()
  {
    Invoke(nameof(TeleportToSpawn), 2f);
  }

  private void TeleportToSpawn()
  {
    PlayerManager.Instance.MovePlayerToSpawn();

    currentHealth = maxHealth;
    healthText.text = "health=" + currentHealth + "/" + maxHealth;
    GetComponent<PlayerMovement>().enabled = true;
    GetComponent<PlayerAttack>().enabled = true;
    GetComponent<Collider2D>().enabled = true;
    animator.SetBool("isDead", false);
  }

}
