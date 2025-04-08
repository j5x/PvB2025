using UnityEngine;
using System.Collections.Generic;

public class AttackComponent : MonoBehaviour
{
    public AttackConfig attackConfig;
    private Animator animator;
    private Character character;
    
    [SerializeField] public List<AttackConfig> attackConfigs; // Multiple attack types
    [SerializeField] private bool isAIControlled; // If true, attack triggers automatically

    private void Awake()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<Character>(); // Reference the Character script

        if (character == null)
        {
            Debug.LogError($"{gameObject.name}: AttackComponent is missing a Character reference!");
        }
        
        if (attackConfigs.Count == 0)
        {
            Debug.LogError($"{gameObject.name}: No attack configs assigned!");
        }
    }

    public void InitializeAttack(AttackConfig config)
    {
        attackConfig = config;
    }

    // Now accepts attack index as a parameter
    public void PerformAttack(int? attackIndex = null) // Allows optional parameter
    {
        if (attackConfigs == null || attackConfigs.Count == 0)
        {
            Debug.LogError($"{gameObject.name}: No attackConfigs available!");
            return;
        }

        int selectedIndex = attackIndex ?? Random.Range(0, attackConfigs.Count); // Pick random if null
        AttackConfig selectedAttack = attackConfigs[selectedIndex];

        animator.SetTrigger(selectedAttack.animatorParameter);

        Debug.Log($"{gameObject.name} performs {selectedAttack.attackName}, inflicting {selectedAttack.attackDamage} damage!");

        Invoke(nameof(ExecuteAttackLogic), selectedAttack.attackDelay);
    }

    private void ExecuteAttackLogic()
    {
        if (character == null)
        {
            Debug.LogError($"{gameObject.name}: AttackComponent cannot execute attack logic because Character is NULL.");
            return;
        }

        //character.ActivateWeaponHitbox(); // Triggers hit detection
    }

    public void AIControlledAttackLoop(float interval)
    {
        if (!isAIControlled) return;
        InvokeRepeating(nameof(PerformRandomAttack), 0f, interval);
    }

    private void PerformRandomAttack()
    {
        int attackIndex = Random.Range(0, attackConfigs.Count); // Randomly selects an attack
        PerformAttack(attackIndex); // Perform attack with the selected index
    }
}
