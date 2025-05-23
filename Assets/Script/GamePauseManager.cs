using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseOverlay; // Assign in Inspector
    private bool isPaused = false;

    private void Start()
    {
        if (pauseOverlay != null)
            pauseOverlay.SetActive(false); // Ensure it's hidden on start
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pauseOverlay != null)
            pauseOverlay.SetActive(true);
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseOverlay != null)
            pauseOverlay.SetActive(false);
        Debug.Log("Game Resumed");
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}