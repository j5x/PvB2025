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

    private const string kLoadingSceneName = "LoadingScene";

    public void LoadLevel()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogWarning($"LevelButton on {gameObject.name} has no targetSceneName set.");
            return;
        }

        if (useLoadingScreen)
        {
            // Tell the loader which scene to load next
            GameLoader.NextSceneName = targetSceneName;
            // Now go to your loading‐screen scene
            SceneManager.LoadScene(kLoadingSceneName);
        }
        else
        {
            // Direct load, as before
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
