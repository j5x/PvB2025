using UnityEngine;
using System.Collections.Generic;

public class AttackComponent : MonoBehaviour
{
    [Header("Attack Setup")]
    [SerializeField] public List<AttackConfig> attackConfigs;
    [SerializeField] private bool isAIControlled;

    private Animator animator;
    private Character character;
    private AttackConfig currentAttackConfig;
    private VfxComponent vfx;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();
        vfx = GetComponent<VfxComponent>();
    
        if (character == null)
            Debug.LogError($"{gameObject.name}: Missing Character reference!");
        if (vfx == null)
            Debug.LogError($"{gameObject.name}: Missing VFX Component reference!");
    }

    public void InitializeAttack(AttackConfig config)
    {
        currentAttackConfig = config;
    }

    public void PerformAttack(int? attackIndex = null)
    {
        if (attackConfigs == null || attackConfigs.Count == 0)
        {
            Debug.LogError($"{gameObject.name}: No attackConfigs available!");
            return;
        }

        int selectedIndex = attackIndex ?? Random.Range(0, attackConfigs.Count);
        currentAttackConfig = attackConfigs[selectedIndex];

        animator.SetTrigger(currentAttackConfig.animatorParameter);

        Debug.Log($"{gameObject.name} performs {currentAttackConfig.attackName}, " +
                  $"inflicting {currentAttackConfig.attackDamage} damage after {currentAttackConfig.attackDelay} sec");

        // Optional: Use this only if you're not handling via Animation Event
        Invoke(nameof(ExecuteAttackLogic), currentAttackConfig.attackDelay);
    }
    
    public void ExecuteAttackLogic()
    {
        if (character == null || currentAttackConfig == null)
            return;

        Character target = (character is Player) ? FindObjectOfType<Enemy>() : FindObjectOfType<Player>();
        if (target == null) return;

        VfxComponent targetVfx = target.GetComponent<VfxComponent>();

        // Handle Impact VFX on Target
        if (currentAttackConfig.impactVFXPrefab != null && targetVfx != null)
        {
            targetVfx.PlayImpactVFX(currentAttackConfig.impactVFXPrefab, currentAttackConfig.vfxOffset);
        }

        // Handle Projectile VFX from Self to Target
        if (currentAttackConfig.projectileVFXPrefab != null && vfx != null)
        {
            vfx.PlayProjectileVFX(currentAttackConfig.projectileVFXPrefab, currentAttackConfig.vfxOffset, target.transform);
        }

        // Deal damage
        target.TakeDamage(currentAttackConfig.attackDamage);
    }
        
    public void AIControlledAttackLoop(float interval)
    {
        if (!isAIControlled) return;

        InvokeRepeating(nameof(PerformRandomAttack), 0f, interval);
    }

    private void PerformRandomAttack()
    {
        PerformAttack();
    }

    public AttackConfig GetCurrentAttack() => currentAttackConfig;
    public int GetCurrentDamage() => currentAttackConfig != null ? currentAttackConfig.attackDamage : 0;
}
