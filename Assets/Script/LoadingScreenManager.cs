using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public static class GameLoader
{
    /// <summary>
    /// Set this before loading the LoadingScene to indicate which scene to load next.
    /// </summary>
    public static string NextSceneName;
}

public class LoadingScreenManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText; // optional

    [Header("Timing")]
    [Tooltip("Minimum time (seconds) the loading screen takes to fill from 0→100%")]
    [SerializeField] private float minLoadTime = 2f;

    private void Start()
    {
        string sceneToLoad = GameLoader.NextSceneName;
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("LoadingScreenManager: GameLoader.NextSceneName not set!");
            return;
        }

        StartCoroutine(LoadWithMinimumTime(sceneToLoad));
    }

    private IEnumerator LoadWithMinimumTime(string targetScene)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(targetScene);
        op.allowSceneActivation = false;

        float startTime = Time.time;
        while (true)
        {
            // true progress (0→1)
            float rawProgress = Mathf.Clamp01(op.progress / 0.9f);
            // time‐based progress (0→1 over minLoadTime)
            float timeProgress = Mathf.Clamp01((Time.time - startTime) / minLoadTime);
            // display the slower of the two
            float uiProgress = Mathf.Min(rawProgress, timeProgress);

            // update UI
            if (progressBar) progressBar.value = uiProgress;
            if (progressText) progressText.text = $"{Mathf.RoundToInt(uiProgress * 100f)}%";

            // once both the scene is loaded (op.progress ≥ .9) AND we've waited long enough...
            if (op.progress >= 0.9f && Time.time - startTime >= minLoadTime)
            {
                op.allowSceneActivation = true;
                yield break;
            }

            yield return null;
        }
    }
}
