using Gameplay.Match3;
using UnityEngine;
using System.Collections;

public class Player : Character
{
    private GridManager gridManager;
    private bool isAttacking = false;
    private float attackCooldown = 1f;
    private HealthComponent healthComponent;
    private AttackComponent attackComponent;

    protected override void Awake()
    {
        base.Awake();
        gridManager = FindObjectOfType<GridManager>();
        healthComponent = GetComponent<HealthComponent>();
        attackComponent = GetComponent<AttackComponent>();

        if (gridManager != null)
            gridManager.OnMatchMade += OnMatchMade;
        else
            Debug.LogError("GridManager not found in the scene!");

        if (healthComponent == null)
        {
            Debug.LogWarning($"{gameObject.name}: HealthComponent is missing!");
            healthComponent = gameObject.AddComponent<HealthComponent>();
        }
        else
        {
            // ✅ Subscribe to the OnDeath event
            healthComponent.OnDeath += HandleDeath;
        }

        if (attackComponent == null || attackComponent.attackConfigs.Count == 0)
        {
            Debug.LogError($"{gameObject.name}: No AttackConfig assigned in AttackComponent!");
        }
    }

    private void OnDestroy()
    {
        if (gridManager != null)
            gridManager.OnMatchMade -= OnMatchMade;

        if (healthComponent != null)
            healthComponent.OnDeath -= HandleDeath; // ✅ Unsubscribe from event to avoid memory leaks
    }

    private void OnMatchMade()
    {
        Attack();
    }

    protected override void Attack()
    {
        if (isAttacking || attackComponent == null || attackComponent.attackConfigs.Count == 0) return;

        isAttacking = true;
        attackComponent.PerformAttack(0);
        StartCoroutine(ResetAttackCooldown());
    }

    protected override void Defend()
    {
        Debug.Log($"{gameObject.name} is defending!");
    }

    public void TakeDamage(int damage)
    {
        if (healthComponent == null) return;

        healthComponent.TakeDamage(damage);
    }

    private void HandleDeath()
    {
        Debug.Log($"{gameObject.name} has died!");
        
        // ✅ Ensure the player is properly removed
        Destroy(gameObject);
    }

    public void ActivateWeaponHitbox()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            StartCoroutine(DisableHitboxAfterDelay(0.2f));
        }
    }

    private IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && weaponCollider.enabled)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackComponent.attackConfigs[0].attackDamage);
            }
        }
    }
}
