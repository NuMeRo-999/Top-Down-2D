using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
  private Animator animator;
  public Transform attackPoint;
  public float attackRange = 0.3f;
  public LayerMask enemyLayers;

  void Start()
  {
    animator = GetComponent<Animator>();
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
    animator.SetTrigger("Attack");

    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

    foreach (Collider2D enemy in hitEnemies)
    {
      if (enemy.gameObject.CompareTag("Enemy"))
      {
        enemy.GetComponent<Enemy>().TakeDamage(10);
      }
    }
  }

  private void OnDrawGizmos() {

    if (attackPoint == null)
    {
      return;
    }
    Gizmos.DrawWireSphere(attackPoint.position, attackRange);
  }
}
