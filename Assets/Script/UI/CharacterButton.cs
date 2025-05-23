using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CharacterButton : MonoBehaviour
{
    [Header("Character Selection")]
    [Tooltip("The prefab to spawn when this button is selected.")]
    [SerializeField] private GameObject characterPrefab;
    [Tooltip("The UI panel to show for this character.")]
    [SerializeField] private GameObject characterPanel;

    [Header("Audio")]
    [Tooltip("Sound played when you select this character.")]
    [SerializeField] private AudioClip selectSfx;
    [Tooltip("Optional delay before showing panel (so you can hear the SFX).")]
    [SerializeField] private float panelDelay = 0.1f;

    private AudioSource _audioSource;

    private void Awake()
    {
        // Guarantee an AudioSource is attached
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    public void OnSelect()
    {
        // Play the select sound
        if (selectSfx != null)
            _audioSource.PlayOneShot(selectSfx);

        // Actually do the selection logic after a tiny delay so SFX can start
        StartCoroutine(HandleSelectionAfterDelay());
    }

    private IEnumerator HandleSelectionAfterDelay()
    {
        yield return new WaitForSeconds(panelDelay);

        if (GameManager.Instance != null)
            GameManager.Instance.SelectCharacter(characterPrefab);

        CharacterSelectUI ui = FindObjectOfType<CharacterSelectUI>();
        if (ui != null)
            ui.ShowCharacterPanel(characterPanel);
    }
}
