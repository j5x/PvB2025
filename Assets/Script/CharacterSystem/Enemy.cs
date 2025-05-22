using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float attackInterval = 5f;
    private VfxComponent vfxComponent;
    [SerializeField] private GameObject spawnVfxPrefab; // 👈 assign per prefab

    protected override void Awake()
    {
        base.Awake();

        // ——— Removed healthComponent.OnDeath subscription ———

        // Ensure AttackComponent is present & configured
        if (attackComponent == null)
        {
            Debug.LogWarning($"{gameObject.name}: AttackComponent is missing!");
            attackComponent = gameObject.AddComponent<AttackComponent>();
        }

        if (attackComponent.attackConfigs.Count > 0)
        {
            attackComponent.InitializeAttack(attackComponent.attackConfigs[0]);
            attackComponent.AIControlledAttackLoop(attackInterval);
        }
        else
        {
            Debug.LogError($"{gameObject.name}: No AttackConfig assigned!");
        }
        
        AssignVfxSpawnPointByTag("EnemyImpactPoint");
        PlaySpawnVFX();
    }

    // ——— Removed HandleDeath entirely ———

    protected override void Attack()
    {
        if (attackComponent == null || attackComponent.attackConfigs.Count == 0)
            return;

        int attackIndex = Random.Range(0, attackComponent.attackConfigs.Count);
        attackComponent.PerformAttackByIndex(attackIndex);
    }
    
    protected override void Defend()
    {
        Debug.Log($"{gameObject.name} is defending!");
    }
    
    private void AssignVfxSpawnPointByTag(string tag)
    {
        if (vfxComponent == null)
            vfxComponent = GetComponent<VfxComponent>();

        if (vfxComponent == null)
        {
            Debug.LogWarning($"{gameObject.name} has no VfxComponent assigned!");
            return;
        }

        GameObject target = GameObject.FindGameObjectWithTag(tag);
        if (target != null)
            vfxComponent.SetVfxSpawnPoint(target.transform);
        else
            Debug.LogWarning($"No GameObject with tag '{tag}' found in the scene!");
    }
    
    private void PlaySpawnVFX()
    {
        if (spawnVfxPrefab == null) return;

        Vector3 spawnPosition = transform.position + Vector3.up * 1.5f; // lift above ground slightly
        Instantiate(spawnVfxPrefab, spawnPosition, Quaternion.identity);
    }
}