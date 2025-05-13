using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform enemyParent;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnDelay = 2f;

    public void SpawnEnemyWithDelay(GameObject enemyPrefab, System.Action<GameObject> onSpawned)
    {
        StartCoroutine(SpawnWithDelay(enemyPrefab, onSpawned));
    }

    private IEnumerator SpawnWithDelay(GameObject enemyPrefab, System.Action<GameObject> onSpawned)
    {
        yield return new WaitForSeconds(spawnDelay);

        if (enemyPrefab == null || enemyParent == null)
        {
            Debug.LogWarning("Missing enemy prefab or spawn parent.");
            onSpawned?.Invoke(null);
            yield break;
        }

        GameObject enemy = Instantiate(enemyPrefab, enemyParent.position, Quaternion.identity, enemyParent);
        onSpawned?.Invoke(enemy);
    }

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

    public GameObject GetEnemyPrefab(int index)
    {
        if (index < 0 || index >= enemyPrefabs.Length)
        {
            Debug.LogWarning("Index out of bounds, returning null prefab.");
            return null;
        }

        return enemyPrefabs[index];
    }
}