using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Animator animator; // Reference to Animator
    private bool canAttack = true;
    private float attackCooldown = 0.5f; // Adjust as needed

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canAttack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        canAttack = false;
        animator.SetTrigger("Attack"); // Play attack animation
        Invoke(nameof(ResetAttack), attackCooldown); // Cooldown reset
    }

    // Call this function as an **Animation Event** when the attack hits
    public void TryDamageEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        if (hitColliders.Length > 0)
        {
            Collider nearestEnemy = GetClosestEnemy(hitColliders);
            if (nearestEnemy != null)
            {
                HealthComponent enemyHealth = nearestEnemy.GetComponent<HealthComponent>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageAmount);
                    Debug.Log($"Damaged {nearestEnemy.name} for {damageAmount} HP");
                }
            }
        }
    }

    private Collider GetClosestEnemy(Collider[] enemies)
    {
        Collider closest = null;
        float minDistance = float.MaxValue;

        foreach (Collider enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }
        return closest;
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
