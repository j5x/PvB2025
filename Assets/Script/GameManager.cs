using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Character Selection")]
    public GameObject selectedCharacterPrefab;

    [Header("References")]
    [SerializeField] private RoundTimer roundTimer;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Enemies")]
    [SerializeField] private GameObject[] enemyPrefabs; // Easy, Medium, Hard

    public UnityEvent OnGameWon;
    public UnityEvent OnGameLost;

    private int currentRound = 0;
    private GameObject currentEnemy;
    private GameObject player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (selectedCharacterPrefab == null)
        {
            Debug.LogError("No player character selected.");
            return;
        }

        player = levelManager.SpawnPlayer(selectedCharacterPrefab);
        if (player == null) return;

        var playerHealth = player.GetComponent<HealthComponent>();
        playerHealth.OnDeath += HandlePlayerDeath;

        roundTimer.OnRoundStart.AddListener(OnRoundStarted);
        roundTimer.OnGameOver.AddListener(HandleTimeExpired);

        StartGame();
    }

    private void StartGame()
    {
        currentRound = 0;
        roundTimer.StartRound(currentRound);
    }

    private void OnRoundStarted(int roundNumber)
    {
        SpawnEnemy(roundNumber - 1);
    }

    private void SpawnEnemy(int index)
    {
        if (currentEnemy != null)
            Destroy(currentEnemy);

        if (index < enemyPrefabs.Length)
        {
            currentEnemy = enemySpawner.SpawnEnemy(enemyPrefabs[index]);
            var enemyHealth = currentEnemy.GetComponent<HealthComponent>();
            enemyHealth.OnDeath += HandleEnemyDeath;
        }
        else
        {
            Debug.LogWarning("No enemy prefab for round " + (index + 1));
        }
    }

    private void HandleEnemyDeath()
    {
        roundTimer.NotifyEnemyDefeated();

        currentRound++;
        if (currentRound >= enemyPrefabs.Length)
        {
            Debug.Log("All enemies defeated. You won!");
            OnGameWon?.Invoke();
        }
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("Player died!");
        OnGameLost?.Invoke();
    }

    private void HandleTimeExpired()
    {
        Debug.Log("Time ran out!");
        OnGameLost?.Invoke();
    }
}

