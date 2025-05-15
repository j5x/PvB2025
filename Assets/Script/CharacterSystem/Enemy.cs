using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float attackInterval = 5f;

    protected override void Awake()
    {
        base.Awake();

        // When this enemy dies, call HandleDeath()
        if (healthComponent != null)
            healthComponent.OnDeath += HandleDeath;

        // Ensure AttackComponent is present & configured
        if (attackComponent == null)
        {
            Debug.LogWarning($"{gameObject.name}: AttackComponent is missing!");
            attackComponent = gameObject.AddComponent<AttackComponent>();
        }

        if (attackComponent.attackConfigs.Count > 0)
        {
            attackComponent.InitializeAttack(attackComponent.attackConfigs[0]);
            attackComponent.AIControlledAttackLoop(attackInterval);
        }
        else
        {
            Debug.LogError($"{gameObject.name}: No AttackConfig assigned!");
        }
    }

    private void HandleDeath()
    {
        // Notify the RoundManager that the enemy has been defeated
        var rm = FindObjectOfType<RoundManager>();
        if (rm != null)
            rm.EnemyDefeated();
        else
            Debug.LogWarning("RoundManager not found in scene!");

        // Destroy this enemy
        Destroy(gameObject);
    }

    protected override void Attack()
    {
        if (attackComponent == null || attackComponent.attackConfigs.Count == 0)
            return;

        Character target = FindObjectOfType<Player>();
        if (target == null)
        {
            Debug.LogWarning("Enemy tried to attack but no Player found!");
            return;
        }

        int attackIndex = Random.Range(0, attackComponent.attackConfigs.Count);
        attackComponent.PerformAttack(attackIndex, target);
    }

    protected override void Defend()
    {
        Debug.Log($"{gameObject.name} is defending!");
    }
}