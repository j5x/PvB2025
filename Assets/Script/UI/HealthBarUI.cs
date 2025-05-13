using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float smoothSpeed = 5f;

    private HealthComponent targetHealthComponent;
    private float targetHealth;

    private void Start()
    {
        FindEnemyTarget();
    }

    private void Update()
    {
        if (targetHealthComponent == null)
        {
            FindEnemyTarget(); // Try again if enemy was destroyed and we need to find a new one
            return;
        }

        if (Mathf.Abs(slider.value - targetHealth) > 0.01f)
        {
            slider.value = Mathf.Lerp(slider.value, targetHealth, Time.deltaTime * smoothSpeed);
        }
    }

    private void FindEnemyTarget()
    {
        GameObject enemy = GameObject.FindWithTag("Enemy");
        if (enemy != null)
        {
            targetHealthComponent = enemy.GetComponent<HealthComponent>();

            if (targetHealthComponent != null)
            {
                slider.maxValue = targetHealthComponent.MaxHealth;
                slider.value = targetHealthComponent.CurrentHealth;
                targetHealth = slider.value;

                targetHealthComponent.OnHealthChanged += OnHealthChanged;
            }
        }
    }

    private void OnDestroy()
    {
        if (targetHealthComponent != null)
        {
            targetHealthComponent.OnHealthChanged -= OnHealthChanged;
        }
    }

    private void OnHealthChanged(int currentHealth)
    {
        targetHealth = currentHealth;
    }
}
