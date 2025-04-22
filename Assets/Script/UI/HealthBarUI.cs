using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private Image fillImage;

    private void Awake()
    {
        if (healthComponent == null)
            healthComponent = GetComponentInParent<HealthComponent>();

        healthComponent.OnHealthChanged += UpdateHealthBar;
    }

    private void Start()
    {
        // Initialize bar on start
        UpdateHealthBar(healthComponent.CurrentHealth);
    }

    private void OnDestroy()
    {
        healthComponent.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int currentHealth)
    {
        float max = healthComponent.MaxHealth;
        fillImage.fillAmount = (float)currentHealth / max;
    }
}