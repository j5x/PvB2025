using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected string characterName;
    [SerializeField] protected float health;

    public string Name => name;
    public float Health => health;

    public CharacterType CharacterType { get; protected set; }
    
    protected Animator animator;
    
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    protected abstract void Attack();
    protected abstract void Defend();
    
    public virtual void PerformAction(ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.Attack:
                Attack();
                break;
            case ActionType.Defend:
                Defend();
                break;
        }
    }
    
    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }
}