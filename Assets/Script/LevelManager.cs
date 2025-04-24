using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform playerParent;

    [Header("Player HP Bars")] [SerializeField]
    private GameObject icykaHpBar;

    [SerializeField] private GameObject loligirlHpBar;
    [SerializeField] private GameObject sharkgirlHpBar;

    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.selectedCharacterPrefab != null)
        {
            GameObject player = Instantiate(GameManager.Instance.selectedCharacterPrefab, spawnPoint.position,
                Quaternion.identity, playerParent);
            string tag = GameManager.Instance.selectedCharacterTag;

            ActivateHpBarBasedOnTag(tag, player.GetComponent<HealthComponent>());
        }
        else
        {
            Debug.LogWarning("No character prefab was selected!");
        }
    }

    private void ActivateHpBarBasedOnTag(string tag, HealthComponent health)
    {
        icykaHpBar?.SetActive(false);
        loligirlHpBar?.SetActive(false);
        sharkgirlHpBar?.SetActive(false);

        GameObject activeHpBar = null;

        switch (tag)
        {
            case "icyka":
                activeHpBar = icykaHpBar;
                break;
            case "loligirl":
                activeHpBar = loligirlHpBar;
                break;
            case "shark girl":
                activeHpBar = sharkgirlHpBar;
                break;
            default:
                Debug.LogWarning("Unknown character tag: " + tag);
                return;
        }

        if (activeHpBar != null)
        {
            activeHpBar.SetActive(true);

            PlayerHealthBarUI ui = activeHpBar.GetComponent<PlayerHealthBarUI>();
            if (ui != null)
            {
                ui.Initialize(health);
            }
        }
    }
}