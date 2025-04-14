using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float attackInterval = 5f;

    protected override void Awake()
    {
        base.Awake();

        // Initialize AttackComponent
        if (attackComponent == null)
        {
            Debug.LogWarning($"{gameObject.name}: AttackComponent is missing!");
            attackComponent = gameObject.AddComponent<AttackComponent>();
        }

        // Initialize attack configs and start AI loop
        if (attackComponent.attackConfigs.Count > 0)
        {
            attackComponent.InitializeAttack(attackComponent.attackConfigs[0]); // Default attack
            attackComponent.AIControlledAttackLoop(attackInterval);             // Begin AI loop
        }
        else
        {
            Debug.LogError($"{gameObject.name}: No AttackConfig assigned!");
        }

        // Initialize HealthBarUI with enemy health info
        if (healthBarUI != null)
        {
            healthBarUI.Initialize(healthComponent.BaseMaxHealth, "Enemy", transform.position);
            healthBarUI.UpdateHealth(healthComponent.CurrentHealth); // Set initial health value
        }
        else
        {
            Debug.LogError("HealthBarUI not assigned for enemy!");
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

    // Override TakeDamage to update health bar as well
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);  // Handle the damage in the base class (updating health)

        // Update health bar when taking damage
        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealth(healthComponent.CurrentHealth);
        }
    }

    private void OnDestroy()
    {
        // Clean up HealthBarUI on destruction (optional)
        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealth(0);  // Optional: Update health bar to 0 when enemy is destroyed
        }
    }
}
