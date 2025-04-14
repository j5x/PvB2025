using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AttackConfig", menuName = "Enemies/AttackConfig")]
public class AttackConfig : ScriptableObject
{
    [HideInInspector] public string attackName = "Default Attack";
    public int attackDamage = 10;
    [HideInInspector] int attackInterval = 5; // Time between attacks -> Currently controlled by enemy.cs
    public float attackDelay = 0.5f; // Delay after animation starts
    public string animatorParameter = "AttackTrigger";
}