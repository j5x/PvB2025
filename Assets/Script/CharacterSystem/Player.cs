using Gameplay.Match3;
using UnityEngine;
using System.Collections;

public class Player : Character
{
    private GridManager gridManager;
    private bool        isAttacking     = false;
    private float       attackCooldown  = 1f;

    protected override void Awake()
    {
        base.Awake();

        // Hook up match-made → attack
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null)
            gridManager.OnMatchMade += OnMatchMade;
        else
            Debug.LogError("GridManager not found in the scene!");

        // Ensure we have attacks configured
        if (attackComponent == null || attackComponent.attackConfigs.Count == 0)
            Debug.LogError($"{gameObject.name}: No AttackConfig assigned in AttackComponent!");

        // When the player dies, call HandleDeath()
        if (healthComponent != null)
            healthComponent.OnDeath += HandleDeath;
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

        Character target = FindObjectOfType<Enemy>();
        if (target == null)
        {
            Debug.LogWarning("Player tried to attack but no Enemy found!");
            return;
        }

        isAttacking = true;
        attackComponent.PerformAttack(0, target);
        StartCoroutine(ResetAttackCooldown());
    }

    protected override void Defend()
    {
        Debug.Log($"{gameObject.name} is defending!");
    }

    private void HandleDeath()
    {
        // Notify the RoundManager that the player has died
        var rm = FindObjectOfType<RoundManager>();
        if (rm != null)
            rm.PlayerDefeated();  // <-- implement this in RoundManager
        else
            Debug.LogWarning("RoundManager not found in scene!");

        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }

    private IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
}
