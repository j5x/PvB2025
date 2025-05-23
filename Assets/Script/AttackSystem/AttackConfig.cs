using UnityEngine;

[CreateAssetMenu(fileName = "AttackConfig", menuName = "Enemies/AttackConfig")]
public class AttackConfig : ScriptableObject
{
    public string attackName = "Default Attack";
    public int attackDamage = 10;
    public float attackDelay = 0.5f;
    public string animatorParameter = "AttackTrigger";

    [Header("Visual Effects")]
    public GameObject attackVFX;
    public GameObject impactVFX;
    public bool useSceneVFXSpawnPoint = false;

    [Header("Audio")]
    [Tooltip("Sound to play when this attack is launched")]
    public AudioClip attackSfx;
    [Tooltip("Sound to play when this attack hits the target")]
    public AudioClip impactSfx;
}
