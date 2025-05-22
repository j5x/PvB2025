using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class AttackComponent : MonoBehaviour
{
    public List<AttackConfig> attackConfigs = new();
    private AttackConfig currentAttackConfig;
    private Animator animator;
    private VfxComponent vfx;

    private Character character;
    private Character targetOverride;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        vfx = GetComponent<VfxComponent>();
        character = GetComponent<Character>();
    }

    public void InitializeAttack(AttackConfig config)
    {
        currentAttackConfig = config;
    }

    public void PerformAttack(int? attackIndex = null, Character explicitTarget = null)
    {
        if (attackConfigs.Count == 0)
            return;

        int index = attackIndex ?? Random.Range(0, attackConfigs.Count);
        currentAttackConfig = attackConfigs[index];

        animator?.SetTrigger(currentAttackConfig.animatorParameter);

        targetOverride = explicitTarget; // Assign the explicit target
        Invoke(nameof(ExecuteAttackLogic), currentAttackConfig.attackDelay);
    }

    public void ExecuteAttackLogic()
    {
        Character target = targetOverride;
        if (target == null)
        {
            Debug.LogWarning($"{gameObject.name}: No attack target assigned!");
            return;
        }

        HealthComponent targetHealth = target.GetComponent<HealthComponent>();
        if (targetHealth != null)
            targetHealth.TakeDamage(currentAttackConfig.attackDamage);

        VfxComponent targetVfx = target.GetComponent<VfxComponent>();
        if (targetVfx != null && currentAttackConfig.impactVFX != null)
            targetVfx.PlayImpactVFX(currentAttackConfig.impactVFX);

        if (vfx != null && currentAttackConfig.attackVFX != null)
            if (vfx != null && currentAttackConfig.attackVFX != null)
            {
                Transform attackSpawn = GameObject.FindWithTag("AttackVFXPoint")?.transform;
                if (attackSpawn != null)
                {
                    Instantiate(currentAttackConfig.attackVFX, attackSpawn.position, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("Attack VFX Spawn Point not found in scene or prefab!");
                }
            }
            else
            {
                vfx.PlayAttackVFX(currentAttackConfig.attackVFX);
            }
    }

    public void AIControlledAttackLoop(float interval)
    {
        InvokeRepeating(nameof(LoopAttack), interval, interval);
    }

    private void LoopAttack()
    {
        Character target = FindObjectOfType<Player>();
        PerformAttack(null, target);
    }
}
