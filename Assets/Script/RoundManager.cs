using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent<int> OnRoundStart        = new UnityEvent<int>();
    public UnityEvent      OnAllRoundsComplete = new UnityEvent();
    public UnityEvent      OnGameOver          = new UnityEvent();

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private GameObject      winScreen;
    [SerializeField] private GameObject      defeatScreen;

    [Header("Spawner Reference")]
    [SerializeField] private EnemySpawner    enemySpawner;

    [Header("Round Settings")]
    [SerializeField] private int totalRounds = 3;

    private int              currentRound      = 0;
    private HealthComponent  lastEnemyHealth;  

    private void Start()
    {
        Time.timeScale = 1f;
        BeginNextRound();
    }

    /// <summary>
    /// This is your public entry point: call this when any enemy dies.
    /// </summary>
    public void EnemyDefeated()
    {
        // 1) Unsubscribe from the last enemy's death event
        if (lastEnemyHealth != null)
        {
            lastEnemyHealth.OnDeath -= HandleEnemyDeath;
            lastEnemyHealth = null;
        }

        // 2) Advance to the next round (or finish)
        BeginNextRound();
    }

    public void PlayerDefeated()
    {
        defeatScreen?.SetActive(true);
        OnGameOver.Invoke();
    }

    private void BeginNextRound()
    {
        // Completed all rounds?
        if (currentRound >= totalRounds)
        {
            winScreen?.SetActive(true);
            OnAllRoundsComplete.Invoke();
            return;
        }

        // Advance & notify
        currentRound++;
        roundText.text = $"Round {currentRound}";
        OnRoundStart.Invoke(currentRound);

        // Spawn the next enemy
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
        var hc = enemy.GetComponent<HealthComponent>();
        if (hc != null)
        {
            // Subscribe so we get exactly one callback
            lastEnemyHealth = hc;
            hc.OnDeath += HandleEnemyDeath;
        }
    }

    private void HandleEnemyDeath()
    {
        // Internal handler simply routes to the public API
        EnemyDefeated();
    }
}
