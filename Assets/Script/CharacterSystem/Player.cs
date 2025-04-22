using Gameplay.Match3;
using UnityEngine;
using System.Collections;

public class Player : Character
{
    private GridManager gridManager;
    private bool isAttacking = false;
    private float attackCooldown = 1f;

    protected override void Awake()
    {
        base.Awake();

        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null)
        {
            gridManager.OnMatchMade += OnMatchMade;
        }
        else
        {
            Debug.LogError("GridManager not found in the scene!");
        }

        if (attackComponent == null || attackComponent.attackConfigs.Count == 0)
        {
            Debug.LogError($"{gameObject.name}: No AttackConfig assigned in AttackComponent!");
        }

        if (healthComponent != null)
        {
            healthComponent.OnDeath += HandleDeath;
        }
    }

    private void OnDestroy()
    {
        if (gridManager != null)
            gridManager.OnMatchMade -= OnMatchMade;

        if (healthComponent != null)
            healthComponent.OnDeath -= HandleDeath;
    }

    private void OnMatchMade()
    {
        Attack();
    }

    protected override void Attack()
    {
        if (isAttacking || attackComponent == null || attackComponent.attackConfigs.Count == 0)
            return;

        isAttacking = true;
        attackComponent.PerformAttack(0);
        StartCoroutine(ResetAttackCooldown());
    }

    protected override void Defend()
    {
        Debug.Log($"{gameObject.name} is defending!");
    }

    private void HandleDeath()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }

    private IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
}