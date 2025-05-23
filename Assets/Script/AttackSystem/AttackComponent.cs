using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class AttackComponent : MonoBehaviour
{
    [Header("Configs")]
    public List<AttackConfig> attackConfigs = new();

    AttackConfig currentAttackConfig;
    Animator animator;
    VfxComponent vfx;
    AudioSource audioSource;
    Character character;
    Character targetOverride;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        vfx = GetComponent<VfxComponent>();
        character = GetComponent<Character>();

        // ensure there's an AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// Pre-select a config if you need to.
    /// </summary>
    public void InitializeAttack(AttackConfig config)
    {
        currentAttackConfig = config;
    }

    /// <summary>
    /// Fire a random or specified attack by index.
    /// </summary>
    public void PerformAttackByIndex(int? attackIndex = null, Character explicitTarget = null)
    {
        if (attackConfigs.Count == 0) return;

        int idx = attackIndex ?? Random.Range(0, attackConfigs.Count);
        if (idx < 0 || idx >= attackConfigs.Count) return;

        currentAttackConfig = attackConfigs[idx];
        PlayAttack(currentAttackConfig, explicitTarget);
    }

    /// <summary>
    /// **New**: Fire exactly this AttackConfig.
    /// </summary>
    public void PerformAttackByConfig(AttackConfig config, Character explicitTarget = null)
    {
        if (config == null)
        {
            Debug.LogWarning($"{name}: PerformAttackByConfig called with null config");
            return;
        }

        currentAttackConfig = config;
        PlayAttack(currentAttackConfig, explicitTarget);
    }

    void PlayAttack(AttackConfig config, Character explicitTarget)
    {
        // 1) animation
        animator?.SetTrigger(config.animatorParameter);

        // 2) attack‐launch SFX
        if (config.attackSfx != null)
            audioSource.PlayOneShot(config.attackSfx);

        // 3) schedule impact/damage
        targetOverride = explicitTarget;
        Invoke(nameof(ExecuteAttackLogic), config.attackDelay);
    }

    void ExecuteAttackLogic()
    {
        var target = targetOverride;
        if (target == null)
        {
            Debug.LogWarning($"{name}: No attack target assigned!");
            return;
        }

        // damage
        var hc = target.GetComponent<HealthComponent>();
        if (hc != null)
            hc.TakeDamage(currentAttackConfig.attackDamage);

        // impact VFX
        if (currentAttackConfig.impactVFX != null)
            vfx.PlayImpactVFX(currentAttackConfig.impactVFX);

        // impact SFX
        if (currentAttackConfig.impactSfx != null)
            audioSource.PlayOneShot(currentAttackConfig.impactSfx);
    }

    public void AIControlledAttackLoop(float interval)
    {
        InvokeRepeating(nameof(LoopAttack), interval, interval);
    }

    void LoopAttack()
    {
        var player = FindObjectOfType<Player>();
        PerformAttackByIndex(null, player);
    }
}
