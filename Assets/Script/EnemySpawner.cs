using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform enemyParent;

    // Declare an array to hold the enemy prefabs
    [SerializeField] private GameObject[] enemyPrefabs;

    public GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab == null || enemyParent == null)
        {
            Debug.LogWarning("Missing enemy prefab or spawn parent.");
            return null;
        }

        GameObject enemy = Instantiate(enemyPrefab, enemyParent.position, Quaternion.identity, enemyParent);
        return enemy;
    }

    // Method to get the next enemy prefab
    public GameObject GetEnemyPrefab(int index)
    {
        if (index < 0 || index >= enemyPrefabs.Length)
        {
            Debug.LogWarning("Index out of bounds, returning null prefab.");
            return null;
        }

        return enemyPrefabs[index];  // Return the prefab at the given index
    }
}