using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private int maxSpawns = 5;

    private int spawnCount = 0;
    private GameObject currentEnemy;

    private void Start()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        if (spawnCount >= maxSpawns)
        {
            Debug.Log("Max enemy spawn limit reached.");
            return;
        }

        StartCoroutine(SpawnEnemyWithDelay(respawnDelay));
    }

    private IEnumerator SpawnEnemyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemyPrefab != null && spawnPoint != null)
        {
            currentEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            Enemy enemyScript = currentEnemy.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                enemyScript.spawner = this;
            }

            spawnCount++;
        }
        else
        {
            Debug.LogWarning("Missing enemy prefab or spawn point.");
        }
    }
}
