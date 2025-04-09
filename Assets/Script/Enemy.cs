using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float attackInterval = 5f;

    protected override void Awake()
    {
        base.Awake();

        if (attackComponent == null)
        {
            Debug.LogWarning($"{gameObject.name}: AttackComponent is missing!");
            attackComponent = gameObject.AddComponent<AttackComponent>();
        }

        if (attackComponent.attackConfigs.Count > 0)
        {
            attackComponent.InitializeAttack(attackComponent.attackConfigs[0]); // Default attack
            attackComponent.AIControlledAttackLoop(attackInterval);             // Begin AI loop
        }
        else
        {
            Debug.LogError($"{gameObject.name}: No AttackConfig assigned!");
        }
    }

    protected override void Attack()
    {
        if (attackComponent == null || attackComponent.attackConfigs.Count == 0)
            return;

        int attackIndex = Random.Range(0, attackComponent.attackConfigs.Count);
        attackComponent.PerformAttack(attackIndex);
    }

    protected override void Defend()
    {
        Debug.Log($"{gameObject.name} is defending!");
    }
}