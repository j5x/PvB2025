using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private HealthComponent enemyHealth;

    public void DealDamage(int damageAmount)
    {
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damageAmount);
        }
        else
        {
            Debug.LogWarning("No enemy assigned to DamageSystem!");
        }
    }
}
