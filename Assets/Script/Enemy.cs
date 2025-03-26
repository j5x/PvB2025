using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Character
{
    [SerializeField] private List<AttackPattern> attackPatterns;
    [SerializeField] private float attackInterval = 5f;
    private Coroutine attackRoutine;
    private AttackPattern currentPattern;

    protected override void Awake()
    {
        base.Awake();
        StartAttacking(); // Start the attack cycle when the enemy is spawned
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
            Attack(); // Perform an attack at each interval
        }
    }

    protected override void Attack()
    {
        if (attackPatterns == null || attackPatterns.Count == 0) return;

        // Randomly select an attack pattern from the list and store it
        currentPattern = attackPatterns[Random.Range(0, attackPatterns.Count)];

        // Trigger the animation for the selected attack pattern
        animator.SetTrigger(currentPattern.animatorParameter);

        // Execute attack logic after the animation delay
        Invoke(nameof(PerformAttack), currentPattern.attackDelay);
    }

    private void PerformAttack()
    {
        if (currentPattern != null)
        {
            Debug.Log($"{gameObject.name} performs a {currentPattern.animatorParameter} attack, dealing {currentPattern.damage} damage!");
        }
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