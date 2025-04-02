using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private AttackComponent attackComponent;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private float attackInterval = 5f;

    protected override void Awake()
    {
        base.Awake();

        attackComponent = GetComponent<AttackComponent>();
        healthComponent = GetComponent<HealthComponent>();

        if (attackComponent == null)
        {
            Debug.LogWarning($"{gameObject.name}: AttackComponent is missing!");
            attackComponent = gameObject.AddComponent<AttackComponent>(); // Ensure it's there
        }

        if (healthComponent == null)
        {
            Debug.LogWarning($"{gameObject.name}: HealthComponent is missing!");
            healthComponent = gameObject.AddComponent<HealthComponent>(); // Ensure it's there
        }

        if (attackComponent.attackConfigs.Count > 0)
        {
            attackComponent.InitializeAttack(attackComponent.attackConfigs[0]); // Set default attack
        }
        else
        {
            Debug.LogError($"{gameObject.name}: No AttackConfig assigned!");
        }

        attackComponent.AIControlledAttackLoop(attackInterval);
    }

    protected override void Attack()
    {
        int attackIndex = Random.Range(0, attackComponent.attackConfigs.Count);
        attackComponent.PerformAttack(attackIndex);
    }

    protected override void Defend()
    {
        Debug.Log($"{gameObject.name} is defending!");
    }

    public void TakeDamage(int damage)
{
    if (healthComponent == null) return;

    Debug.Log($"{gameObject.name} BEFORE DAMAGE: HP = {healthComponent.CurrentHealth}");

    healthComponent.TakeDamage(damage);

    Debug.Log($"{gameObject.name} AFTER DAMAGE: HP = {healthComponent.CurrentHealth}");
}

\
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && weaponCollider.enabled)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(attackComponent.attackConfig.attackDamage);
            }
        }
    }
}
