using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform playerSpawnPoint;

    public GameObject SpawnPlayer(GameObject playerPrefab)
    {
        if (playerPrefab == null)
        {
            Debug.LogWarning("Player prefab is null.");
            return null;
        }

        GameObject player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity, playerSpawnPoint);
        return player;
    }
}