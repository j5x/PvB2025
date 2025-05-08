using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Combat/Attack Config")]
public class AttackConfig : ScriptableObject
{
    public string attackName;
    public int attackDamage;
    public float attackDelay;
    public string animatorParameter;

    [Header("VFX (optional)")]
    public GameObject impactVFXPrefab;     // Spawned on the target’s UI anchor
    public GameObject projectileVFXPrefab; // Thrown from attacker to target
    public Vector3 vfxOffset;              // Optional offset for either
}
