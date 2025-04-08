using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected string characterName;
    [SerializeField] protected HealthConfig healthConfig;
    [SerializeField] protected Collider2D weaponCollider;

    public string Name => characterName;
    private HealthComponent HealthComponent { get; set; }
    public CharacterType CharacterType { get; protected set; }

    protected Animator animator;
    protected AttackComponent attackComponent;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        attackComponent = GetComponent<AttackComponent>();

        if (attackComponent == null)
        {
            Debug.LogError($"{gameObject.name}: Missing AttackComponent! Assign it in Inspector.");
        }

        HealthComponent = GetComponent<HealthComponent>() ?? gameObject.AddComponent<HealthComponent>();    
        HealthComponent.InitializeHealth(healthConfig);
        HealthComponent.OnDeath += Die;

        if (weaponCollider == null)
        {
            weaponCollider = GetComponentInChildren<Collider2D>(); // Auto-assign if missing
            if (weaponCollider == null)
            {
                Debug.LogError($"{gameObject.name}: WeaponCollider is NULL! Assign it in Inspector.");
            }
        }
        weaponCollider.enabled = false;
    }

    protected abstract void Attack();
    protected abstract void Defend();

    public virtual void TakeDamage(float damage)
    {
        if (HealthComponent == null)
        {
            Debug.LogError($"{gameObject.name}: HealthComponent is missing!");
            return;
        }
        HealthComponent.TakeDamage((int)damage);
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }

    public void ActivateWeaponHitbox()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            StartCoroutine(DisableHitboxAfterDelay(0.5f));
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: WeaponCollider is NULL in ActivateWeaponHitbox!");
        }
    }

    protected IEnumerator DisableHitboxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }
}
