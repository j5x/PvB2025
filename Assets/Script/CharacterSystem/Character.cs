using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] protected string characterName;
    [SerializeField] protected HealthConfig healthConfig;

    [Header("Components")]
    [SerializeField] protected AttackComponent attackComponent;
    [SerializeField] protected HealthComponent healthComponent;

    protected Animator animator;
    protected HealthBarUI healthBarUI; // Dynamically assigned UI reference

    public string Name => characterName;
    public CharacterType CharacterType { get; protected set; }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        // Ensure AttackComponent is assigned
        if (attackComponent == null)
        {
            attackComponent = GetComponent<AttackComponent>();
            if (attackComponent == null)
            {
                Debug.LogError($"{gameObject.name}: Missing AttackComponent! Assign it in Inspector.");
            }
        }

        // Ensure HealthComponent is assigned
        healthComponent = GetComponent<HealthComponent>() ?? gameObject.AddComponent<HealthComponent>();
        healthComponent.InitializeHealth(healthConfig);

        // Explicitly create or find the HealthBarUI for both Player and Enemy
        if (healthBarUI == null)
        {
            GameObject healthBarObject = new GameObject("HealthBar");
            healthBarUI = healthBarObject.AddComponent<HealthBarUI>();
            healthBarObject.transform.SetParent(GameObject.Find("Canvas").transform); // Assuming a canvas exists
        }

        healthBarUI.Initialize(healthComponent.BaseMaxHealth, characterName, transform.position);
        healthBarUI.UpdateHealth(healthComponent.CurrentHealth);

        // Subscribe to health events to update the health bar when health changes
        healthComponent.OnHealthChanged += HandleHealthChanged;
        healthComponent.OnDeath += Die;
    }

    protected abstract void Attack();
    protected abstract void Defend();

    public virtual void TakeDamage(float damage)
    {
        if (healthComponent == null)
        {
            Debug.LogError($"{gameObject.name}: HealthComponent is missing!");
            return;
        }

        healthComponent.TakeDamage((int)damage);
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");

        // Optionally, clean up health bar UI (or set it to 0)
        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealth(0); // Set health bar to 0 when character dies
        }

        Destroy(gameObject);
    }

    // Handle health changes and update the health bar UI accordingly
    protected virtual void HandleHealthChanged(int currentHealth)
    {
        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealth(currentHealth);
        }
    }
}
