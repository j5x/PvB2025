using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Text nameText; // Optional text for character's name

    private Transform targetTransform; // The transform to follow (player or enemy)

    // Initialize the health bar with max health, name, and target position
    public void Initialize(int maxHealth, string characterName, Vector3 position)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }

        if (nameText != null)
        {
            nameText.text = characterName;
        }

        // Set the target transform to follow (e.g., the player's or enemy's position)
        targetTransform = GameObject.Find(characterName)?.transform;
        if (targetTransform != null)
        {
            // Position the health bar above the target
            transform.position = targetTransform.position + new Vector3(0, 2, 0); // Adjust '2' to set the height
        }
    }

    // Update health value on the health bar UI
    public void UpdateHealth(int currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (targetTransform != null)
        {
            // Make sure health bar follows the character
            transform.position = targetTransform.position + new Vector3(0, 2, 0); // Adjust '2' for vertical offset
        }
    }
}