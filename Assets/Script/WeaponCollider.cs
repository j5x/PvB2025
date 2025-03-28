using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    // Store a reference to the collider attached to the weapon.
    private Collider weaponCollider;

    private void Start()
    {
        weaponCollider = GetComponent<Collider>(); // Assuming this is the weapon's collider
        weaponCollider.enabled = false; // Disable the collider at the start
    }

    // Call this when the attack animation starts
    public void EnableWeaponCollider()
    {
        weaponCollider.enabled = true;
    }

    // Call this when the attack animation ends
    public void DisableWeaponCollider()
    {
        weaponCollider.enabled = false;
    }

    // Detect collisions with the weapon collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is an enemy or another player
        if (other.CompareTag("Enemy"))
        {
            // Get the HealthComponent on the enemy and apply damage
            HealthComponent enemyHealth = other.GetComponent<HealthComponent>();
            if (enemyHealth != null)
            {
                // Adjust the damage value as needed
                enemyHealth.TakeDamage(10); // This should be customizable based on your game needs
            }
        }
    }
}
