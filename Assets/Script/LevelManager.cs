using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform playerParent;

    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.selectedCharacterPrefab != null)
        {
            Instantiate(GameManager.Instance.selectedCharacterPrefab, spawnPoint.position, Quaternion.identity, playerParent);
        }
        else
        {
            Debug.LogWarning("No character prefab was selected!");
        }
    }

}