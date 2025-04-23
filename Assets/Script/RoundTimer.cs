using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


    public class RoundTimer : MonoBehaviour
    {
        public UnityEvent<int> OnRoundStart = new UnityEvent<int>(); // Notify UI when a round starts
        public UnityEvent OnGameOver = new UnityEvent(); // Trigger game over

        [SerializeField] private TextMeshProUGUI timerText; // Assign in inspector

        private float[] roundDurations = { 45f, 45f, 60f }; // Round times
        private int currentRound = 0;
        private float timeRemaining;
        private bool isTimerRunning = false;
        
        private Coroutine timerCoroutine;

        private bool isEnemyDead = false; // Placeholder, later link to enemy script

        private void Start()
        {
            StartRound(0);
        }

        public void StartRound(int roundIndex)
        {
            if (roundIndex >= roundDurations.Length)
            {
                Debug.Log("All rounds completed!");
                return;
            }
            
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine); // 🛑 Stop vorige timer
            }

            currentRound = roundIndex;
            timeRemaining = roundDurations[currentRound];
            isTimerRunning = true;
            isEnemyDead = false; // Reset for the new round

            OnRoundStart.Invoke(currentRound + 1);
            UpdateTimerUI();
            timerCoroutine = StartCoroutine(TimerCoroutine());
        }

        private IEnumerator TimerCoroutine()
        {
            while (isTimerRunning && timeRemaining > 0)
            {
                yield return new WaitForSeconds(1f);
                timeRemaining--;
                UpdateTimerUI();

                if (isEnemyDead) // Later replace with actual enemy death check
                {
                    Debug.Log("Enemy defeated! Starting next round...");
                    StartNextRound();
                    yield break;
                }
            }

            if (timeRemaining <= 0)
            {
                Debug.Log("Game Over! Time ran out.");
                OnGameOver.Invoke();
            }
        }

        private void StartNextRound()
        {
            isTimerRunning = false;
            if (currentRound < roundDurations.Length - 1)
            {
                StartRound(currentRound + 1);
            }
            else
            {
                Debug.Log("Game Completed!");
            }
        }

        private void UpdateTimerUI()
        {
            if (timerText != null)
            {
                timerText.text = $"Time: {timeRemaining}s";
            }
        }

        // Call this function when the enemy dies
        public void EnemyDefeated()
        {
            isEnemyDead = true;
        }
    }

