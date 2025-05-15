using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Combat/Attack Config")]
public class AttackConfig : ScriptableObject
{
    public string attackName;
    public int attackDamage;
    public float attackDelay;
    public string animatorParameter;

    [Header("Visual Effects")]
    public GameObject attackVFX;
    public GameObject impactVFX;
}
