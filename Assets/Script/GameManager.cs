using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public GameObject selectedCharacterPrefab;

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

    public void SelectCharacter(GameObject characterPrefab)
    {
        selectedCharacterPrefab = characterPrefab;
        Debug.Log("Selected character: " + characterPrefab.name);
    }
}