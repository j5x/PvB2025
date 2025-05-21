using UnityEngine;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject startButtonObject; // Reference to the whole button GameObject

    private void Start()
    {
        // Hide start button initially
        if (startButtonObject != null)
            startButtonObject.SetActive(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCharacterSelected += ShowStartButton;
        }
    }

    private void ShowStartButton()
    {
        if (startButtonObject != null)
            startButtonObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCharacterSelected -= ShowStartButton;
        }
    }
}