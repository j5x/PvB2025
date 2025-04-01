using Gameplay.Match3;
using UnityEngine;
using System.Collections;

public class Player : Character
{
    private GridManager gridManager;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D weaponCollider; // Assign the weapon's collider in the Inspector

    private bool isAttacking = false;
    private float attackCooldown = 1f; // Adjust cooldown as needed

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

        if (weaponCollider != null)
        {
            weaponCollider.enabled = false; // Ensure hitbox starts disabled
        }
        else
        {
            Debug.LogError("Weapon hitbox collider is not assigned to the player!");
        }
    }

    protected override void Defend()
    {
        Debug.Log("Player is defending!");
        if (animator != null) animator.SetTrigger("Defend");
    }

    protected override void Attack()
    {
        if (isAttacking) return; // Prevent attack spamming

        isAttacking = true;
        if (animator != null)
        {
            animator.SetTrigger("Attack"); // Play attack animation
        }

        StartCoroutine(ResetAttackCooldown());
    }

    /// <summary>
    /// Called from an Animation Event when the weapon should deal damage.
    /// </summary>
    public void ActivateWeaponHitbox()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true; // Enable the hitbox when the weapon swings
            StartCoroutine(DisableHitboxAfterDelay(0.2f)); // Disable it after a short time
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && weaponCollider.enabled)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(100); // Adjust damage as needed
            }
        }
    }

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null) gridManager.OnMatchMade += OnMatchMade;
        else Debug.LogError("GridManager not found in the scene!");
    }

    private void OnDestroy()
    {
        if (gridManager != null) gridManager.OnMatchMade -= OnMatchMade;
    }

    private void OnMatchMade()
    {
        Attack();
    }

    private IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    public IEnumerator DisableHitboxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (weaponCollider != null) weaponCollider.enabled = false;
    }
}
