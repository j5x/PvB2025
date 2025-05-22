using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float smoothSpeed = 5f;

    private HealthComponent playerHealthComponent;
    private float targetHealth;
    
    public void Initialize(HealthComponent health)
    {
        playerHealthComponent = health;

        if (playerHealthComponent != null)
        {
            slider.maxValue = playerHealthComponent.MaxHealth;
            slider.value = playerHealthComponent.CurrentHealth;
            targetHealth = slider.value;

            playerHealthComponent.OnHealthChanged += OnHealthChanged;
        }
    }

    private void Start()
    {
        FindPlayer();
    }

    private void Update()
    {
        if (playerHealthComponent == null)
        {
            FindPlayer(); // Try again if player reference was lost (e.g., after respawn)
            return;
        }

        if (Mathf.Abs(slider.value - targetHealth) > 0.01f)
        {
            slider.value = Mathf.Lerp(slider.value, targetHealth, Time.deltaTime * smoothSpeed);
        }
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerHealthComponent = player.GetComponent<HealthComponent>();
        
            if (playerHealthComponent != null)
            {
                slider.maxValue = playerHealthComponent.MaxHealth;
                slider.value = playerHealthComponent.CurrentHealth;
                targetHealth = slider.value;

                playerHealthComponent.OnHealthChanged += OnHealthChanged;
            }
            
            var health = player.GetComponentInChildren<HealthComponent>();
            if (health == null)
            {
                Debug.LogError("HealthComponent not found on spawned player prefab!");
            }
        }
    }

    private void OnDestroy()
    {
        if (playerHealthComponent != null)
        {
            playerHealthComponent.OnHealthChanged -= OnHealthChanged;
        }
    }

    private void OnHealthChanged(int currentHealth)
    {
        targetHealth = currentHealth;
    }
}
