using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent<int> OnRoundStart = new UnityEvent<int>();
    public UnityEvent OnAllRoundsComplete = new UnityEvent();
    public UnityEvent OnGameOver = new UnityEvent();

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject defeatScreen;

    [Header("Spawner Reference")]
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Round Settings")]
    [SerializeField] private int totalRounds = 3;

    private int currentRound = 0;
    private HealthComponent lastEnemyHealth;
    private GameObject lastEnemyGO;      // <-- track last enemy object

    private void Start()
    {
        Time.timeScale = 1f;
        BeginNextRound();
    }

    /// <summary>
    /// Call when any enemy dies.
    /// </summary>
    public void EnemyDefeated()
    {
        // 1) Unsubscribe from the last enemy's death event
        if (lastEnemyHealth != null)
        {
            lastEnemyHealth.OnDeath -= HandleEnemyDeath;
            lastEnemyHealth = null;
        }

        // 2) Ensure the old enemy object is gone
        if (lastEnemyGO != null)
        {
            Destroy(lastEnemyGO);
            lastEnemyGO = null;
        }

        // 3) Advance to the next round
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
        // Remember the GameObject so we can clean it up later
        lastEnemyGO = enemy;

        // Wire up OnDeath → HandleEnemyDeath
        var hc = enemy.GetComponent<HealthComponent>();
        if (hc != null)
        {
            lastEnemyHealth = hc;
            hc.OnDeath += HandleEnemyDeath;
        }
    }

    private void HandleEnemyDeath()
    {
        // Simply call the public API once
        EnemyDefeated();
    }
}
