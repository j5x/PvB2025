using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float attackInterval = 5f;
    internal EnemySpawner spawner;

    protected override void Awake()
    {
        base.Awake();

        // Attach death listener to HealthComponent
        if (healthComponent != null)
        {
            healthComponent.OnDeath += HandleDeath;
        }

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
        // Notify the spawner when this enemy dies
        if (spawner != null)
        {
            spawner.SpawnEnemy();
        }

        Destroy(gameObject); // Destroy this enemy after spawning the new one
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
