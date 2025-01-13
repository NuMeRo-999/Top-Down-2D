using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
  private Animator animator;
  public Transform attackPoint;
  public float attackRange = 0.3f;
  public LayerMask enemyLayers;
  public Chest[] chest;
  public AudioSource attackAudioSource;

  void Start()
  {
    animator = GetComponent<Animator>();
    chest = FindObjectsByType<Chest>(FindObjectsSortMode.None);
  }

  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      Attack();
    }
  }

  void Attack()
  {
    foreach (Chest chest in chest)
    {
      if (chest.openChest) return;
    }

    animator.SetTrigger("Attack");
    attackAudioSource.Play();

    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

    foreach (Collider2D enemy in hitEnemies)
    {
      if (!enemy.isTrigger && enemy.gameObject.CompareTag("Enemy") && enemy.GetComponent<Enemy>().currentHealth > 0)
      {
        enemy.GetComponent<Enemy>().TakeDamage(10);
      }
    }
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
