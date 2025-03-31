using Gameplay.Match3;
using UnityEngine;
using System.Collections;
public class Player : Character
{
    private GridManager gridManager; // Reference to the GridManager
    [SerializeField] private Animator animator; // Reference to the Animator component

    private bool isAttacking = false;  // Flag to check if the player is currently attacking
    private float attackCooldown = 1f; // Time in seconds between attacks (adjust as needed)

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>(); // Assuming you have an Animator component on the Player
    }

    // Implementing the abstract method 'Defend()' from the Character class
    protected override void Defend()
    {
        // This could be a block, dodge, or other defense actions
        Debug.Log("Player is defending!");

        // Example of defense logic:
        // You can play a defense animation or make the player invincible for a moment
        if (animator != null)
        {
            animator.SetTrigger("Defend"); // Assuming you have a "Defend" trigger in your Animator
        }
    }

    protected override void Attack()
    {
        // If the player is already attacking, return early
        if (isAttacking) return;

        // Set attacking flag to prevent further attacks during the animation or cooldown
        isAttacking = true;

        // Trigger the attack animation
        if (animator != null)
        {
            animator.SetTrigger("Attack"); // Assuming you have an "Attack" trigger in your Animator
        }

        // Perform the attack logic to deal damage
        PerformAttack();

        // Start a coroutine to reset the attack flag after the attack cooldown or animation is complete
        StartCoroutine(ResetAttackCooldown());
    }

    private void PerformAttack()
    {
        // Create a hitbox (could be a collider or a raycast) to detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1f, LayerMask.GetMask("Enemy")); // Adjust the radius as needed

        foreach (var enemyCollider in hitEnemies)
        {
            // Check if the collider is an enemy
            if (enemyCollider.CompareTag("Enemy"))
            {
                Enemy enemy = enemyCollider.GetComponent<Enemy>(); // Assuming Enemy script is on the enemy

                if (enemy != null)
                {
                    // Call the existing TakeDamage method on the enemy
                    enemy.TakeDamage(10); // You can adjust the damage value as needed
                }
            }
        }
    }

    private void Start()
    {
        // Find the GridManager in the scene
        gridManager = FindObjectOfType<GridManager>();

        // Subscribe to the OnMatchMade event
        if (gridManager != null)
        {
            gridManager.OnMatchMade += OnMatchMade; // Subscribe to the event
        }
        else
        {
            Debug.LogError("GridManager not found in the scene!");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event when the player is destroyed
        if (gridManager != null)
        {
            gridManager.OnMatchMade -= OnMatchMade;
        }
    }

    private void OnMatchMade()
    {
        // Trigger the attack when a match is made
        Attack();
    }

    private IEnumerator ResetAttackCooldown()
    {
        // Wait for the cooldown duration before allowing another attack
        yield return new WaitForSeconds(attackCooldown);

        // Reset the attacking flag so another attack can happen
        isAttacking = false;
    }
}
