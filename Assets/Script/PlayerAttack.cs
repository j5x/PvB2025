using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int attackDamage = 10; // Adjust as needed
    [SerializeField] private LayerMask enemyLayer; // Define the enemy layer
    [SerializeField] private Collider weaponCollider; // Reference to the weapon's collider (hitbox)

    private Animator animator;
    private bool isAttacking = false;
    private bool hasHit = false; // Flag to track if the weapon has already hit an enemy during the attack

    private void Start()
    {
        animator = GetComponent<Animator>();
        weaponCollider.enabled = false;  // Disable the collider initially, it'll be enabled during the attack
    }

    // Call this function through the Animation Event when the attack starts
    public void StartAttack()
    {
        isAttacking = true;
        hasHit = false; // Reset the hit flag at the start of the new attack
        weaponCollider.enabled = true;  // Enable the weapon collider to detect hits
    }

    // Call this function through the Animation Event when the attack ends
    public void EndAttack()
    {
        isAttacking = false;
        weaponCollider.enabled = false;  // Disable the weapon collider after the attack
    }

    // This function will be triggered by the Animation Event to deal damage to enemies
    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking && !hasHit && other.CompareTag("Enemy"))  // Only deal damage if attacking and hasn't hit yet
        {
            HealthComponent enemyHealth = other.GetComponent<HealthComponent>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage); // Deal damage
                Debug.Log($"Damaged {other.gameObject.name} for {attackDamage} HP!");
                hasHit = true; // Set the flag to true to prevent further damage in this attack
            }
        }
    }
}
