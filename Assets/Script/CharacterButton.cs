using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;

    public void OnSelect()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SelectCharacter(characterPrefab);
        }
    }
}