using UnityEngine;
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

    /// Performs an attack using an index (or random if null)
    public void PerformAttackByIndex(int? attackIndex = null, Character explicitTarget = null)
    {
        if (attackConfigs.Count == 0)
            return;

        int index = attackIndex ?? Random.Range(0, attackConfigs.Count);
        if (index < 0 || index >= attackConfigs.Count)
        {
            Debug.LogWarning($"Invalid attack index {index} for {gameObject.name}!");
            return;
        }

        currentAttackConfig = attackConfigs[index];
        PlayAttack(currentAttackConfig, explicitTarget);
    }

    /// Performs an attack using a specific AttackConfig
    public void PerformAttackByConfig(AttackConfig config, Character explicitTarget = null)
    {
        if (config == null)
        {
            Debug.LogWarning("AttackConfig is null!");
            return;
        }

        currentAttackConfig = config;
        PlayAttack(currentAttackConfig, explicitTarget);
    }

    private void PlayAttack(AttackConfig config, Character explicitTarget)
    {
        animator?.SetTrigger(config.animatorParameter);
        targetOverride = explicitTarget;
        Invoke(nameof(ExecuteAttackLogic), config.attackDelay);
    }

    private void ExecuteAttackLogic()
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
        {
            Transform attackSpawn = null;

            if (currentAttackConfig.useSceneVFXSpawnPoint)
            {
                // Standard attack VFX
                GameObject attackPoint = GameObject.FindWithTag("AttackVFXPoint");
                if (attackPoint != null)
                    attackSpawn = attackPoint.transform;
            }
            else
            {
                // Special attack VFX
                GameObject specialPoint = GameObject.FindWithTag("SpecialVFXPoint");
                if (specialPoint != null)
                    attackSpawn = specialPoint.transform;
            }

            if (attackSpawn != null)
            {
                Instantiate(currentAttackConfig.attackVFX, attackSpawn.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("No appropriate VFX spawn point found in the scene.");
            }
        }
    }


    public void AIControlledAttackLoop(float interval)
    {
        InvokeRepeating(nameof(LoopAttack), interval, interval);
    }

    private void LoopAttack()
    {
        Character target = FindObjectOfType<Player>();
        PerformAttackByIndex(null, target); // Now unambiguous
    }
}
