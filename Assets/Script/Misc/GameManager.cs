using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject SelectedCharacterPrefab { get; private set; }

    public string SelectedCharacterTag => SelectedCharacterPrefab != null ? SelectedCharacterPrefab.tag : null;

    public event System.Action OnCharacterSelected;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectCharacter(GameObject prefab)
    {
        SelectedCharacterPrefab = prefab;
        OnCharacterSelected?.Invoke();
    }
}
