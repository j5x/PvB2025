using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject creditsPanel;

    [Header("SFX")]
    [Tooltip("Sound to play when you click Start")]
    [SerializeField] private AudioClip clickSfx;
    [Tooltip("How long to wait for the SFX to start playing before loading the scene")]
    [SerializeField] private float loadDelay = 0.2f;

    private AudioSource _audioSource;

    private const string kLoadingSceneName = "LoadingScene";

    private void Awake()
    {
        // grab or add an AudioSource
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();

        _audioSource.playOnAwake = false;
    }

    public void StartGame()
    {
        // play the click sound
        if (clickSfx != null)
            _audioSource.PlayOneShot(clickSfx);

        // kick off the delayed load so you actually hear the click
        StartCoroutine(DoLoadAfterDelay());
    }

    private IEnumerator DoLoadAfterDelay()
    {
        yield return new WaitForSeconds(loadDelay);
        GameLoader.NextSceneName = "CharacterSelect";
        SceneManager.LoadScene(kLoadingSceneName);
    }

    public void ShowCredits()
    {
        creditsPanel?.SetActive(true);
    }

    public void HideCredits()
    {
        creditsPanel?.SetActive(false);
    }
}
