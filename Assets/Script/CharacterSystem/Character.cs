using System;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Configuration")] 
    [SerializeField] protected string characterName;
    [SerializeField] protected HealthConfig healthConfig;

    [Header("Character Type")]  
    [SerializeField] private CharacterType characterType;
    public CharacterType CharacterType => characterType;

    [Header("Components")]  
    [SerializeField] protected AttackComponent attackComponent;
    [SerializeField] protected HealthComponent healthComponent;

    protected Animator animator;

    public string Name => characterName;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        if (attackComponent == null)
        {
            attackComponent = GetComponent<AttackComponent>();
            if (attackComponent == null)
                Debug.LogError($"{gameObject.name}: Missing AttackComponent! Assign it in Inspector.");
        }

        healthComponent = GetComponent<HealthComponent>() 
                          ?? gameObject.AddComponent<HealthComponent>();
        healthComponent.InitializeHealth(healthConfig);
        // Character no longer auto-subscribes to its own death event
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
        // Only destruction here; death logic handled by subscribers elsewhere
        Destroy(gameObject);
    }
}