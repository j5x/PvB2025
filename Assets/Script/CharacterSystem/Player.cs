using Gameplay.Match3;
using UnityEngine;
using System.Collections;

public class Player : Character
{
    private GridManager gridManager;
    private bool isAttacking = false;
    private float attackCooldown = 1f;

    private VfxComponent vfxComponent;

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

        AssignVfxSpawnPointByTag("PlayerImpactPoint");
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
        var rm = FindObjectOfType<RoundManager>();
        if (rm != null)
            rm.PlayerDefeated();
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

    private void AssignVfxSpawnPointByTag(string tag)
    {
        if (vfxComponent == null)
            vfxComponent = GetComponent<VfxComponent>();

        if (vfxComponent == null)
        {
            Debug.LogWarning($"{gameObject.name} has no VfxComponent assigned!");
            return;
        }

        GameObject target = GameObject.FindGameObjectWithTag(tag);
        if (target != null)
        {
            vfxComponent.SetVfxSpawnPoint(target.transform);
        }
        else
        {
            Debug.LogWarning($"No GameObject with tag '{tag}' found in the scene!");
        }
    }
}
