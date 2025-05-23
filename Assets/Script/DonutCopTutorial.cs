using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class DonutCopTutorial : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator copAnimator;
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private TextMeshProUGUI speechText;

    [Header("Settings")]
    [TextArea(2, 5)]
    [SerializeField] private string[] tutorialLines;
    [SerializeField] private AudioClip[] voiceClips;         // ← one clip per line
    [SerializeField] private float delayBetweenLines = 1.5f;
    [SerializeField] private float textTypingSpeed = 0.03f;

    [Header("Scene Transition")]
    [SerializeField] private string nextSceneName;
    [SerializeField] private float sceneDelay = 1f;

    private AudioSource _audioSource;
    private int currentLineIndex = 0;
    private bool isTyping = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (voiceClips.Length != tutorialLines.Length)
            Debug.LogWarning($"You have {tutorialLines.Length} lines but {voiceClips.Length} clips.");
    }

    private void Start()
    {
        speechBubble.SetActive(false);
        StartCoroutine(PlayTutorial());
    }

    private IEnumerator PlayTutorial()
    {
        while (currentLineIndex < tutorialLines.Length)
        {
            yield return StartCoroutine(SpeakLine(currentLineIndex));
            currentLineIndex++;
            yield return new WaitForSeconds(delayBetweenLines);
        }

        speechBubble.SetActive(false);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            yield return new WaitForSeconds(sceneDelay);
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name not set in DonutCopTutorial.");
        }
    }

    private IEnumerator SpeakLine(int lineIndex)
    {
        // start talking animation & show bubble
        copAnimator.SetTrigger("Talk");
        speechBubble.SetActive(true);

        // play the voice clip if we have one
        if (lineIndex < voiceClips.Length && voiceClips[lineIndex] != null)
        {
            _audioSource.clip = voiceClips[lineIndex];
            _audioSource.Play();
        }

        // type the text
        speechText.text = "";
        isTyping = true;
        string line = tutorialLines[lineIndex];
        foreach (char c in line)
        {
            speechText.text += c;
            yield return new WaitForSeconds(textTypingSpeed);
        }
        isTyping = false;

        // wait for voice to finish if it’s longer than our typing
        if (_audioSource.clip != null)
        {
            float remaining = _audioSource.clip.length - _audioSource.time;
            if (remaining > 0f)
                yield return new WaitForSeconds(remaining);
        }
    }

    /// <summary>
    /// If the bubble is clicked mid-typing, finish immediately.
    /// </summary>
    public void OnSpeechBubbleClicked()
    {
        if (!isTyping) return;

        StopAllCoroutines();
        // show full text
        speechText.text = tutorialLines[currentLineIndex];
        isTyping = false;
        // let the voice continue, then advance in the main loop
        StartCoroutine(ContinueAfterClick());
    }

    // after skipping text we still want the standard delay & scene logic
    private IEnumerator ContinueAfterClick()
    {
        // wait the remainder of the clip
        if (_audioSource.clip != null)
        {
            float remaining = _audioSource.clip.length - _audioSource.time;
            if (remaining > 0f)
                yield return new WaitForSeconds(remaining);
        }
        // now back into the main loop
        currentLineIndex++;
        yield return new WaitForSeconds(delayBetweenLines);
        StartCoroutine(PlayTutorial());
    }
}
