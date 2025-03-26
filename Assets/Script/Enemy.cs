using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Character
{
    [SerializeField] private List<AttackConfig> attackConfigs; 
    [SerializeField] private float attackInterval = 5f; 
    private Coroutine attackRoutine;
    private AttackComponent attackComponent;

    protected override void Awake()
    {
        base.Awake();

        attackComponent = gameObject.GetComponent<AttackComponent>();
        if (attackComponent == null)
        {
            attackComponent = gameObject.AddComponent<AttackComponent>();
        }

        // Initialize the first attack pattern
        if (attackConfigs is { Count: > 0 })
        {
            attackComponent.InitializeAttack(attackConfigs[0]);
        }

        StartAttacking();
    }

    private void StartAttacking()
    {
        if (attackRoutine != null)
            StopCoroutine(attackRoutine);
        attackRoutine = StartCoroutine(AttackCycle());
    }

    private IEnumerator AttackCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);
            Attack(); // Using the parent class method (now functional)
        }
    }

    // Refactored to call PerformRandomAttack
    protected override void Attack()
    {
        PerformRandomAttack(); // Now this method gets called via inheritance
    }

    private void PerformRandomAttack()
    {
        if (attackConfigs == null || attackConfigs.Count == 0) return;

        int randomIndex = Random.Range(0, attackConfigs.Count); // Select random attack index
        AttackConfig selectedAttack = attackConfigs[randomIndex];

        attackComponent.PerformAttack(selectedAttack, randomIndex); // Pass the attack index
    }


    protected override void Defend()
    {
        Debug.Log($"{gameObject.name} is defending!");
    }

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        base.Die();
    }
}