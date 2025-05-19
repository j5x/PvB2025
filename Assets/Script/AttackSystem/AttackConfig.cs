using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AttackConfig", menuName = "Enemies/AttackConfig")]
public class AttackConfig : ScriptableObject
{
    [Header("Attack Properties")]
    public string attackName = "Default Attack";
    public int attackDamage = 10;
    [Tooltip("Delay after animation starts before damage/VFX are triggered")]
    public float attackDelay = 0.5f;
    [Tooltip("Trigger name used in Animator")]
    public string animatorParameter = "AttackTrigger";

    [Header("VFX Settings")]
    public AttackVfxType vfxType = AttackVfxType.Slash;

    [Tooltip("Used for Slash-type attacks")]
    public GameObject attackVFX;

    [Tooltip("Used for Impact-type attacks (hits target directly)")]
    public GameObject impactVFX;

    [Tooltip("Used for Projectile-type attacks")]
    public GameObject projectilePrefab;

    [Tooltip("Speed of projectile movement (Projectile type only)")]
    public float projectileSpeed = 10f;

    // This field was hidden before — remove if not used
    //[HideInInspector] public int attackInterval = 5;
}