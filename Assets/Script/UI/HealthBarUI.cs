using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private Slider slider;
    [SerializeField] private float smoothSpeed = 5f;

    private float targetHealth;

    private void Awake()
    {
        if (healthComponent == null)
            healthComponent = GetComponentInParent<HealthComponent>();

        if (slider == null)
            slider = GetComponentInChildren<Slider>();

        if (healthComponent != null)
            healthComponent.OnHealthChanged += OnHealthChanged;
    }

    private void Start()
    {
        if (healthComponent != null)
        {
            slider.maxValue = healthComponent.MaxHealth;
            slider.value = healthComponent.CurrentHealth;
            targetHealth = slider.value;
        }
    }

    private void Update()
    {
        if (Mathf.Abs(slider.value - targetHealth) > 0.01f)
        {
            slider.value = Mathf.Lerp(slider.value, targetHealth, Time.deltaTime * smoothSpeed);
        }
    }

    private void OnDestroy()
    {
        if (healthComponent != null)
            healthComponent.OnHealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int currentHealth)
    {
        targetHealth = currentHealth;
    }
}