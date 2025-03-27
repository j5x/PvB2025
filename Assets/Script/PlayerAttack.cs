using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;

    public float attackCooldown = 1f; // Time between attacks
    private float lastAttackTime;

    void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
        lastAttackTime = Time.time;
    }

    void Update()
    {
        // Check if "E" key is pressed and attack is off cooldown
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastAttackTime + attackCooldown)
        {
            TriggerAttack();
            lastAttackTime = Time.time; // Reset cooldown
        }
    }

    void TriggerAttack()
    {
        // Trigger the attack animation by setting the trigger in Animator
        animator.SetTrigger("Attack1");
    }
}
