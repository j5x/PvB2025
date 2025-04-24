using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RoundTimer : MonoBehaviour
{
    public UnityEvent<int> OnRoundStart = new UnityEvent<int>();
    public UnityEvent OnGameOver = new UnityEvent();

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject defeatScreen;

    [Header("Round Settings")]
    [SerializeField] private float[] roundDurations = { 45f, 45f, 60f };

    [Header("Spawner Reference")]
    [SerializeField] private EnemySpawner enemySpawner;

    private int currentRound = 0;
    private float timeRemaining;
    private bool isTimerRunning = false;

    private Coroutine timerCoroutine;
    private GameObject currentEnemy;

    private void Start()
    {
        Time.timeScale = 1f; // Make sure game is unpaused at start
        StartRound(0);
    }

    public void StartRound(int roundIndex)
    {
        if (roundIndex >= roundDurations.Length)
        {
            Debug.Log("All rounds completed!");

            StopMusic();
            Time.timeScale = 0f;

            if (winScreen != null)
                winScreen.SetActive(true);
            else
                Debug.LogWarning("Win screen not assigned!");

            return;
        }

        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        currentRound = roundIndex;
        timeRemaining = roundDurations[currentRound];
        isTimerRunning = true;

        OnRoundStart.Invoke(currentRound + 1);
        UpdateTimerUI();

        SpawnEnemy(currentRound);
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    private void SpawnEnemy(int index)
    {
        GameObject enemyPrefab = enemySpawner.GetEnemyPrefab(index);
        if (enemyPrefab != null)
        {
            currentEnemy = enemySpawner.SpawnEnemy(enemyPrefab);
            Debug.Log("Spawned enemy: " + currentEnemy.name);
        }
        else
        {
            Debug.LogWarning("Enemy prefab is NULL at index: " + index);
        }
    }

    private IEnumerator TimerCoroutine()
    {
        while (isTimerRunning && timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
            UpdateTimerUI();

            if (currentEnemy == null)
            {
                Debug.Log("Enemy defeated! Starting next round...");
                StartNextRound();
                yield break;
            }
        }

        if (timeRemaining <= 0)
        {
            Debug.Log("Game Over! Time ran out.");

            StopMusic();
            Time.timeScale = 0f;

            if (defeatScreen != null)
                defeatScreen.SetActive(true);
            else
                Debug.LogWarning("Defeat screen not assigned!");

            OnGameOver.Invoke();
        }
    }

    private void StartNextRound()
    {
        isTimerRunning = false;
        StartRound(currentRound + 1);
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = $"Time: {timeRemaining}s";
        }
    }

    public void NotifyEnemyDefeated()
    {
        if (currentEnemy != null)
        {
            Destroy(currentEnemy);
            currentEnemy = null;
        }
    }

    public void NotifyPlayerDied()
    {
        Debug.Log("Player died!");

        StopMusic();
        Time.timeScale = 0f;

        if (defeatScreen != null)
            defeatScreen.SetActive(true);
        else
            Debug.LogWarning("Defeat screen not assigned!");

        isTimerRunning = false;

        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
    }

    private void StopMusic()
    {
        AudioSource music = GameObject.FindWithTag("Music")?.GetComponent<AudioSource>();
        if (music != null)
            music.Stop();
    }
}
