using UnityEngine;

public class FrameRateHandler : MonoBehaviour
{
    [Header("Frame Rate Settings")]
    [SerializeField] private int targetFrameRate = 60;

    private static FrameRateHandler instance;

    private void Awake()
    {
        // Singleton check to avoid duplicates
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Set frame rate settings
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = 0;
    }

    private void OnValidate()
    {
        // Apply changes immediately in editor
        Application.targetFrameRate = targetFrameRate;
    }
}