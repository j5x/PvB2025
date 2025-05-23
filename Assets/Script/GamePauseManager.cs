using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GamePauseManager : MonoBehaviour
{
    [Header("UI")]
    [Tooltip("The overlay to show when paused")]
    [SerializeField] private GameObject pauseOverlay;

    [Header("Audio")]
    [Tooltip("Sound to play on pause/resume")]
    [SerializeField] private AudioClip clickSfx;

    private AudioSource _audioSource;
    private bool isPaused = false;

    private void Awake()
    {
        // Ensure there's an AudioSource
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    private void Start()
    {
        if (pauseOverlay != null)
            pauseOverlay.SetActive(false);
    }

    /// <summary>
    /// Toggles between paused and resumed states, playing the click SFX.
    /// </summary>
    public void TogglePause()
    {
        // Play click sound first
        if (clickSfx != null)
            _audioSource.PlayOneShot(clickSfx);

        if (isPaused) ResumeGame();
        else PauseGame();
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

    public bool IsPaused() => isPaused;
}
