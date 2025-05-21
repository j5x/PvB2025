using UnityEngine;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject startButtonObject;
    [SerializeField] private GameObject[] characterPanels; // ← Assign in Inspector

    private void Start()
    {
        if (startButtonObject != null)
            startButtonObject.SetActive(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCharacterSelected += ShowStartButton;
        }
    }

    public void ShowCharacterPanel(GameObject selectedPanel)
    {
        foreach (GameObject panel in characterPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        if (selectedPanel != null)
            selectedPanel.SetActive(true);
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