using UnityEngine;
using System.Collections;

public class Player : Character
{
    private bool isAttacking = false;
    private float attackCooldown = 1f;

    private VfxComponent vfxComponent;

    protected override void Awake()
    {
        base.Awake();

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
        // Clean up death subscription
        if (healthComponent != null)
            healthComponent.OnDeath -= HandleDeath;
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
        var attackConfig = attackComponent.attackConfigs[0];
        attackComponent.PerformAttackByConfig(attackConfig, target);

        StartCoroutine(ResetAttackCooldown());
    }

    protected override void Defend()
    {
        Debug.Log($"{gameObject.name} is defending!");
    }

    private void HandleDeath()
    {
        // Notify round manager
        var rm = FindObjectOfType<RoundManager>();
        if (rm != null)
            rm.PlayerDefeated();

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

        GameObject point = GameObject.FindGameObjectWithTag(tag);
        if (point != null)
            vfxComponent.SetVfxSpawnPoint(point.transform);
        else
            Debug.LogWarning($"No GameObject with tag '{tag}' found in the scene!");
    }
}
