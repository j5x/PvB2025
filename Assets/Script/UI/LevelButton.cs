using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        // 1) Spawn a temporary object that plays the click sound then self-destructs
        if (clickSfx != null)
            StartCoroutine(PlayAndDestroy(clickSfx));

        // 2) Begin loading after a small delay
        StartCoroutine(DoLoadAfterDelay());
    }

    private IEnumerator PlayAndDestroy(AudioClip clip)
    {
        var go = new GameObject("OneShotAudio");
        var src = go.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.clip = clip;
        src.Play();
        // Make sure this object survives scene loads:
        DontDestroyOnLoad(go);
        yield return new WaitForSeconds(clip.length);
        Destroy(go);
    }

    private IEnumerator DoLoadAfterDelay()
    {
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
