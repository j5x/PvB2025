using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.selectedCharacterPrefab != null)
        {
            Instantiate(GameManager.Instance.selectedCharacterPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No character prefab was selected!");
        }
    }

}
