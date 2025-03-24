using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_HealthConfig", menuName = "Health/HealthConfig")]
public class SO_HealthConfig : ScriptableObject
{
    public int maxHealth = 100;
    public bool canRegenerate = false;
    public float regenRate = 1f; // HP per second
    public int regenAmount = 1;
}

public class HealthComponent : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private SO_HealthConfig healthConfig;

    private int baseMaxHealth;
    private int currentHealth;
    private float regenMultiplier = 1f;
    private float damageMultiplier = 1f;
    public bool IsDead => currentHealth <= 0;

    public event Action<int> OnHealthChanged;
    public event Action OnDeath;

    private void Start()
    {
        if (healthConfig == null)
        {
            Debug.LogError("HealthConfig is missing on " + gameObject.name);
            return;
        }

        baseMaxHealth = healthConfig.maxHealth;
        currentHealth = baseMaxHealth;
        if (healthConfig.canRegenerate)
            InvokeRepeating(nameof(RegenerateHealth), 1f / (healthConfig.regenRate * regenMultiplier), 1f / (healthConfig.regenRate * regenMultiplier));
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        int finalDamage = Mathf.RoundToInt(amount * damageMultiplier);
        currentHealth = Mathf.Max(currentHealth - finalDamage, 0);
        OnHealthChanged?.Invoke(currentHealth);

        if (IsDead)
            Die();
    }

    public void Heal(int amount)
    {
        if (IsDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, baseMaxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }

    private void RegenerateHealth()
    {
        if (healthConfig.canRegenerate && !IsDead && currentHealth < baseMaxHealth)
        {
            Heal(Mathf.RoundToInt(healthConfig.regenAmount * regenMultiplier));
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        CancelInvoke(nameof(RegenerateHealth)); // Stop regeneration if dead
        Debug.Log(gameObject.name + " has died.");
    }

    // Buffs and Debuffs
    public void ApplyHealthBuff(int extraHealth, float duration)
    {
        baseMaxHealth += extraHealth;
        currentHealth += extraHealth;
        OnHealthChanged?.Invoke(currentHealth);
        StartCoroutine(RemoveHealthBuff(extraHealth, duration));
    }

    private IEnumerator RemoveHealthBuff(int extraHealth, float duration)
    {
        yield return new WaitForSeconds(duration);
        baseMaxHealth -= extraHealth;
        currentHealth = Mathf.Min(currentHealth, baseMaxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void ApplyDamageMultiplier(float multiplier, float duration)
    {
        damageMultiplier *= multiplier;
        StartCoroutine(RemoveDamageMultiplier(multiplier, duration));
    }

    private IEnumerator RemoveDamageMultiplier(float multiplier, float duration)
    {
        yield return new WaitForSeconds(duration);
        damageMultiplier /= multiplier;
    }

    public void ApplyRegenMultiplier(float multiplier, float duration)
    {
        regenMultiplier *= multiplier;
        CancelInvoke(nameof(RegenerateHealth));
        InvokeRepeating(nameof(RegenerateHealth), 1f / (healthConfig.regenRate * regenMultiplier), 1f / (healthConfig.regenRate * regenMultiplier));
        StartCoroutine(RemoveRegenMultiplier(multiplier, duration));
    }

    private IEnumerator RemoveRegenMultiplier(float multiplier, float duration)
    {
        yield return new WaitForSeconds(duration);
        regenMultiplier /= multiplier;
        CancelInvoke(nameof(RegenerateHealth));
        InvokeRepeating(nameof(RegenerateHealth), 1f / (healthConfig.regenRate * regenMultiplier), 1f / (healthConfig.regenRate * regenMultiplier));
    }
}