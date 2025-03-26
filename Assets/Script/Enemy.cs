using UnityEngine;
using System;
using System.Collections.Generic;

public class Enemy : Character
{
    [SerializeField] private List<AttackPattern> attackPatterns; // List of attack patterns
    private int currentAttackIndex = 0; // Index to select the current attack

    // Use this method to set the target (could be a player or another NPC)
    // You don’t need a new reference for HealthComponent, just use the one inherited from Character
    public void SetTarget(Character target)
    {
        // Now we can access the target's HealthComponent directly
        HealthComponent targetHealth = target.GetComponent<HealthComponent>();
        if (targetHealth != null)
        {
            // Use the target's HealthComponent to apply damage
            targetHealth.TakeDamage(10);  // Example of how to apply damage
        }
    }

    protected override void Attack()
    {
        if (attackPatterns.Count == 0) return; // No attack patterns available

        var pattern = attackPatterns[currentAttackIndex];

        // Trigger the animation
        animator.SetTrigger(pattern.animationTriggerName);

        // Execute the attack after a delay to match the animation timing
        Invoke(nameof(PerformAttack), pattern.attackDelay);
    }

    private void PerformAttack()
    {
        if (attackPatterns.Count == 0) return;

        var pattern = attackPatterns[currentAttackIndex];

        // Apply damage to the target
        Debug.Log($"{gameObject.name} performs an attack dealing {pattern.damage} damage!");

        // Use the HealthComponent in the base class to apply damage
        HealthComponent.TakeDamage(Mathf.RoundToInt(pattern.damage));
    }

    protected override void Defend()
    {
        // Implement defend logic, e.g., play defend animation, reduce damage, etc.
    }

    protected override void Die()
    {
        // Enemy dies - trigger any animations or behaviors needed
        Debug.Log($"{gameObject.name} has died.");
        base.Die();
    }
}