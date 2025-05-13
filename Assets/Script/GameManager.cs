using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public GameObject selectedCharacterPrefab;
    [HideInInspector] public string selectedCharacterTag;

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
        selectedCharacterTag = characterPrefab.tag;
        Debug.Log("Selected character: " + characterPrefab.name + ", Tag: " + selectedCharacterTag);
    }
}