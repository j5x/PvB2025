using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject characterPanel; // ← Assign in Inspector

    public void OnSelect()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SelectCharacter(characterPrefab);
        }

        CharacterSelectUI ui = FindObjectOfType<CharacterSelectUI>();
        if (ui != null)
        {
            ui.ShowCharacterPanel(characterPanel);

        }
    }
}