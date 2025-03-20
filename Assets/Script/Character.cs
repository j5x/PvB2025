using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float speed;

    public float Health => health; // Read-only property
    public float Speed => speed;   // Read-only property

    public CharacterType CharacterType { get; protected set; }
    
    protected Animator animator;
    
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //Abstract Methods -> REQUIRED by inherited classes.
    protected abstract void Move();
    protected abstract void Attack();
    protected abstract void Defend();
    
    public virtual void PerformAction(ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.Move:
                Move();
                break;
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