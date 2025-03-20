using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    [Header("Character Information")]
    [SerializeField] private string characterName; // Name of the character
    [SerializeField] private string classType;    // Warrior, Mage, Archer, etc.

    // TO-DO -> create Health System to add as Component.
    [Header("Character Stats")]
    [SerializeField] protected int maxHealth = 100;
    protected int currentHealth;

    [SerializeField] protected float attack;
    [SerializeField] protected float defense;
    [SerializeField] protected float agility;

    [Header("Components")]
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    // TO-DO -> StateMachine setup.
    // protected ICharacterState currentState;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        // Initialize character stats (Health, Attack, Defense, Agility)
    }

    // TO-DO: Figure out a way for the matches made to translate and communicate into Actions
    
    // Abstract method for performing actions based on input (e.g., Match-3 combinations)
    public abstract void PerformAction(ActionType actionType);

    // Methods for actions
    public virtual void PerformAttack()
    {
        Debug.Log($"{characterName} performs an attack with {attack} damage!");
        // Specific attack logic here
    }

    public virtual void PerformDefend()
    {
        Debug.Log($"{characterName} defends with {defense} defense!");
        // Specific defense logic here
    }

    public virtual void PerformMobility()
    {
        Debug.Log($"{characterName} moves with {agility} agility!");
        // Specific mobility logic here
    }

    // Health-related methods
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{characterName} has died.");
        // Handle death (e.g., play animation, disable character)
    }

    // Optional: State Machine management (to be implemented later)
    // public void SetState(ICharacterState newState)
    // {
    //     currentState = newState;
    //     // Handle state transitions
    // }

    // Name and Class Type Getters
    public string GetCharacterName()
    {
        return characterName;
    }

    public string GetClassType()
    {
        return classType;
    }

    // Add more methods as needed for other components (e.g., Buffs, Debuffs)
}
