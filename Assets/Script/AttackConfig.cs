using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AttackConfig", menuName = "Enemies/AttackConfig")]
public class AttackConfig : ScriptableObject
{
    public string attackName = "Default Attack";
    public float attackDamage = 10f;
    //public float attackInterval = 5f; // Time between attacks -> Currently controlled by enemy.cs
    public float attackDelay = 0.5f; // Delay after animation starts
    public string animatorParameter = "AttackTrigger";
}