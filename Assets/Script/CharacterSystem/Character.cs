using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] protected string characterName;
    [SerializeField] protected HealthConfig healthConfig;

    [Header("Components")]
    [SerializeField] protected AttackComponent attackComponent;
    [SerializeField] protected HealthComponent healthComponent;
    
    protected Animator animator;
    
    public string Name => characterName;
    public CharacterType CharacterType { get; protected set; }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        
        if (attackComponent == null)
        {
            attackComponent = GetComponent<AttackComponent>();
            if (attackComponent == null)
            {
                Debug.LogError($"{gameObject.name}: Missing AttackComponent! Assign it in Inspector.");
            }
        }

        healthComponent = GetComponent<HealthComponent>() ?? gameObject.AddComponent<HealthComponent>();
        healthComponent.InitializeHealth(healthConfig);
        healthComponent.OnDeath += Die;
    }

    protected abstract void Attack();
    protected abstract void Defend();

    public virtual void TakeDamage(float damage)
    {
        if (healthComponent == null)
        {
            Debug.LogError($"{gameObject.name}: HealthComponent is missing!");
            return;
        }

        healthComponent.TakeDamage((int)damage);
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }
}