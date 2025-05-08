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

    private void Awake()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();

        if (character == null)
            Debug.LogError($"{gameObject.name}: Missing Character reference!");

        if (attackConfigs == null || attackConfigs.Count == 0)
            Debug.LogError($"{gameObject.name}: No attack configs assigned!");
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

    /// <summary>
    /// Call this from an Animation Event at the moment of impact.
    /// </summary>
    public void ExecuteAttackLogic()
    {
        if (character == null || currentAttackConfig == null)
            return;

        Character target = (character is Player) ? FindObjectOfType<Enemy>() : FindObjectOfType<Player>();
        if (target == null) return;

        // Handle impact
        if (currentAttackConfig.impactVFXPrefab != null && target.uiVFXAnchor != null)
        {
            Instantiate(
                currentAttackConfig.impactVFXPrefab,
                target.uiVFXAnchor.position + currentAttackConfig.vfxOffset,
                Quaternion.identity,
                target.uiVFXAnchor
            );
        }

        // Handle projectile
        if (currentAttackConfig.projectileVFXPrefab != null && character.vfxSpawnPoint != null)
        {
            GameObject proj = Instantiate(
                currentAttackConfig.projectileVFXPrefab,
                character.vfxSpawnPoint.position + currentAttackConfig.vfxOffset,
                Quaternion.identity
            );

            // Move it toward target (see note below for movement script)
            proj.transform.LookAt(target.transform);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.AddForce(proj.transform.forward * 100f);
        }

        // Then apply damage
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
