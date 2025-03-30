using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    private AttackConfig attackConfig;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void InitializeAttack(AttackConfig config)
    {
        attackConfig = config;
    }

    public void PerformAttack(AttackConfig config, int attackIndex)
    {
        if (config == null) return;

        animator.SetTrigger(config.animatorParameter);

        // Debug Log showing which attack (by index) is being performed and its damage
        Debug.Log($"Enemy performs #{attackIndex}: {config.name}, inflicts {config.attackDamage} damage!");

        Invoke(nameof(ExecuteAttackLogic), config.attackDelay);
    }

    private void ExecuteAttackLogic()
    {
        
    }
}