using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class AttackComponent : MonoBehaviour
{
    [Header("Configs")]
    public List<AttackConfig> attackConfigs = new();

    private AttackConfig currentAttackConfig;
    private Animator animator;
    private VfxComponent vfx;
    private AudioSource audioSource;
    private Character character;
    private Character targetOverride;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        vfx = GetComponent<VfxComponent>();
        character = GetComponent<Character>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    public void InitializeAttack(AttackConfig config)
    {
        currentAttackConfig = config;
    }

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

    public void PerformAttackByConfig(AttackConfig config, Character explicitTarget = null)
    {
        if (config == null)
        {
            Debug.LogWarning($"{gameObject.name}: AttackConfig is null!");
            return;
        }

        currentAttackConfig = config;
        PlayAttack(currentAttackConfig, explicitTarget);
    }

    private void PlayAttack(AttackConfig config, Character explicitTarget)
    {
        // 1. Animation
        animator?.SetTrigger(config.animatorParameter);

        // 2. Launch SFX
        if (config.attackSfx != null)
            audioSource.PlayOneShot(config.attackSfx);

        // 3. Target setup
        targetOverride = explicitTarget;

        // 4. Invoke delayed logic
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

        // Damage
        HealthComponent targetHealth = target.GetComponent<HealthComponent>();
        if (targetHealth != null)
            targetHealth.TakeDamage(currentAttackConfig.attackDamage);

        // Impact VFX on target
        VfxComponent targetVfx = target.GetComponent<VfxComponent>();
        if (targetVfx != null && currentAttackConfig.impactVFX != null)
            targetVfx.PlayImpactVFX(currentAttackConfig.impactVFX);

        // Impact SFX
        if (currentAttackConfig.impactSfx != null)
            audioSource.PlayOneShot(currentAttackConfig.impactSfx);

        // Optional: additional VFX on scene (e.g., slash trails)
        if (vfx != null && currentAttackConfig.attackVFX != null)
        {
            Transform attackSpawn = null;

            if (currentAttackConfig.useSceneVFXSpawnPoint)
            {
                GameObject attackPoint = GameObject.FindWithTag("AttackVFXPoint");
                if (attackPoint != null)
                    attackSpawn = attackPoint.transform;
            }
            else
            {
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
        PerformAttackByIndex(null, target);
    }
}
