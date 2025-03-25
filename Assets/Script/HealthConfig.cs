using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthConfig", menuName = "Health/HealthConfig")]
public class HealthConfig : ScriptableObject
{
    public int maxHealth = 100;
    public bool canRegenerate = false;
    public float regenRate = 1f; // HP per second
    public int regenAmount = 1;
}