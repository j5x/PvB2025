using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance;

    public AudioClip backgroundMusic; // The background music clip
    public bool loopMusic = true; // Whether to loop the music

    private AudioSource audioSource;
    private bool isPaused = false; // Track if the game is paused

    private void Awake()
    {
        // Ensure only one instance of the BackgroundMusicManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        // Set up the AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = loopMusic;
        audioSource.playOnAwake = true;

        // Play the background music
        PlayMusic();
    }

    private void Update()
    {
        // Check if the game is paused and handle music accordingly
        if (isPaused && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else if (!isPaused && !audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }

    public void PlayMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void PauseMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void ResumeMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = Mathf.Clamp(volume, 0f, 1f); // Ensure volume is between 0 and 1
        }
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
    }
}