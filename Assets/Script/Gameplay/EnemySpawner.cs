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

        // ✅ Try to find HealthComponent in children
        HealthComponent health = enemy.GetComponentInChildren<HealthComponent>();
        if (health == null)
        {
            Debug.LogError($"[EnemySpawner] No HealthComponent found on or in children of '{enemy.name}'!");
        }

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

        // ✅ Try to find HealthComponent in children
        HealthComponent health = enemy.GetComponentInChildren<HealthComponent>();
        if (health == null)
        {
            Debug.LogError($"[EnemySpawner] No HealthComponent found on or in children of '{enemy.name}'!");
        }

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
