using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class LevelButton : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("The actual game scene this button should take you to.")]
    [SerializeField] private string targetSceneName;

    [Header("Loading Screen (optional)")]
    [Tooltip("If true, we jump to LoadingScene first, then to Target on completion.")]
    [SerializeField] private bool useLoadingScreen = true;

    [Header("SFX")]
    [Tooltip("Click sound to play when button is pressed.")]
    [SerializeField] private AudioClip clickSfx;
    [Tooltip("Delay before actually loading the scene (so the sound can play).")]
    [SerializeField] private float loadDelay = 0.1f;

    private const string kLoadingSceneName = "LoadingScene";
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        // We don't want it auto‐play anything
        _audioSource.playOnAwake = false;
    }

    public void LoadLevel()
    {
        // Validate selection
        if (GameManager.Instance == null || GameManager.Instance.SelectedCharacterPrefab == null)
        {
            Debug.LogWarning("You must select a character before starting the level!");
            return;
        }

        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogWarning($"LevelButton on {gameObject.name} has no targetSceneName set.");
            return;
        }

        // Play click SFX
        if (clickSfx != null)
            _audioSource.PlayOneShot(clickSfx);

        // Kick off delayed load
        StartCoroutine(DoLoadAfterDelay());
    }

    private IEnumerator DoLoadAfterDelay()
    {
        // Wait a bit so the click sound can start
        yield return new WaitForSeconds(loadDelay);

        if (useLoadingScreen)
        {
            GameLoader.NextSceneName = targetSceneName;
            SceneManager.LoadScene(kLoadingSceneName);
        }
        else
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
