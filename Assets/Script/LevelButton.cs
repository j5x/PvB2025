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
        //  Check if character was selected
        if (GameManager.Instance == null || GameManager.Instance.SelectedCharacterPrefab == null)
        {
            Debug.LogWarning("You must select a character before starting the level!");
            // Optional: Display a UI warning here
            return;
        }

        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogWarning($"LevelButton on {gameObject.name} has no targetSceneName set.");
            return;
        }

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