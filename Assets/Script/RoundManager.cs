using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent<int> OnRoundStart        = new UnityEvent<int>();  // new round number
    public UnityEvent      OnAllRoundsComplete = new UnityEvent();      // after last round
    public UnityEvent      OnGameOver          = new UnityEvent();      // player death

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private GameObject      winScreen;
    [SerializeField] private GameObject      defeatScreen;

    [Header("Spawner Reference")]
    [SerializeField] private EnemySpawner    enemySpawner;

    [Header("Round Settings")]
    [SerializeField] private int totalRounds = 3;

    private int        currentRound = 0;
    private GameObject currentEnemy;

    private void Start()
    {
        Time.timeScale = 1f;
        StartNextRound();
    }

    /// <summary>
    /// Call when the enemy dies.
    /// </summary>
    public void EnemyDefeated()
    {
        StartNextRound();
    }

    /// <summary>
    /// Call when the player dies.
    /// </summary>
    public void PlayerDefeated()
    {
        defeatScreen?.SetActive(true);
        OnGameOver.Invoke();
    }

    private void StartNextRound()
    {
        // Unsubscribe from last enemy death
        if (currentEnemy != null)
        {
            var hc = currentEnemy.GetComponent<HealthComponent>();
            if (hc != null)
                hc.OnDeath -= HandleEnemyDeath;
            currentEnemy = null;
        }

        // Have we completed all rounds?
        if (currentRound >= totalRounds)
        {
            winScreen?.SetActive(true);
            OnAllRoundsComplete.Invoke();
            return;
        }

        // Advance to next round
        currentRound++;
        roundText.text = $"Round {currentRound}";
        OnRoundStart.Invoke(currentRound);

        // Spawn the enemy for this round (0-based index)
        SpawnEnemyForRound(currentRound - 1);
    }

    private void SpawnEnemyForRound(int roundIndex)
    {
        var prefab = enemySpawner.GetEnemyPrefab(roundIndex);
        if (prefab == null)
        {
            Debug.LogWarning($"RoundManager: no enemy prefab for round {roundIndex + 1}");
            return;
        }
        enemySpawner.SpawnEnemyWithDelay(prefab, OnEnemySpawned);
    }

    private void OnEnemySpawned(GameObject enemy)
    {
        currentEnemy = enemy;
        var hc = enemy.GetComponent<HealthComponent>();
        if (hc != null)
            hc.OnDeath += HandleEnemyDeath;
    }

    private void HandleEnemyDeath()
    {
        EnemyDefeated();
    }
}
