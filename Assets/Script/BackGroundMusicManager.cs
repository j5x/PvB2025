using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance;

    [Header("Default Audio")]
    [Tooltip("Music to play in all scenes except stop or main level")]
    public AudioClip backgroundMusic;
    public bool      loopMusic = true;

    [Header("Main Level Audio")]
    [Tooltip("Exact name of your MainLvl scene")]
    public string   mainLevelSceneName;
    [Tooltip("Music to play when MainLvl loads")]
    public AudioClip mainLevelMusic;
    public bool      loopMainLevel = true;

    [Header("Stop Settings")]
    [Tooltip("Exact name of the scene where music should stop completely")]
    public string stopOnSceneName;

    private AudioSource audioSource;
    private bool        isPaused = false;

    private void Awake()
    {
        // Singleton enforcement
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // At startup, pick the right clip for the current scene
        SwapMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SwapMusicForScene(scene.name);
    }

    private void SwapMusicForScene(string sceneName)
    {
        // 1) If it's the stop scene → stop everything
        if (!string.IsNullOrEmpty(stopOnSceneName) && sceneName == stopOnSceneName)
        {
            audioSource.Stop();
            return;
        }

        // 2) If it's your main level → play mainLevelMusic
        if (!string.IsNullOrEmpty(mainLevelSceneName) && sceneName == mainLevelSceneName
            && mainLevelMusic != null)
        {
            PlayClip(mainLevelMusic, loopMainLevel);
            return;
        }

        // 3) Fallback → default backgroundMusic
        if (backgroundMusic != null)
        {
            PlayClip(backgroundMusic, loopMusic);
        }
    }

    private void PlayClip(AudioClip clip, bool loop)
    {
        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    private void Update()
    {
        if (isPaused && audioSource.isPlaying)
            audioSource.Pause();
        else if (!isPaused && !audioSource.isPlaying)
            audioSource.UnPause();
    }

    // Public API
    public void StopMusic()       => audioSource.Stop();
    public void PauseMusic()      => audioSource.Pause();
    public void ResumeMusic()     => audioSource.UnPause();
    public void SetVolume(float v) => audioSource.volume = Mathf.Clamp01(v);
    public void SetPaused(bool p)  => isPaused = p;
}
