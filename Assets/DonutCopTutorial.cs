using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DonutCopTutorial : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator copAnimator;
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private TextMeshProUGUI speechText;

    [Header("Settings")]
    [TextArea(2, 5)]
    [SerializeField] private string[] tutorialLines;
    [SerializeField] private float delayBetweenLines = 1.5f;
    [SerializeField] private float textTypingSpeed = 0.03f;

    private int currentLineIndex = 0;
    private bool isTyping = false;

    private void Start()
    {
        speechBubble.SetActive(false);
        StartCoroutine(PlayTutorial());
    }

    private IEnumerator PlayTutorial()
    {
        while (currentLineIndex < tutorialLines.Length)
        {
            yield return StartCoroutine(SpeakLine(tutorialLines[currentLineIndex]));
            currentLineIndex++;
            yield return new WaitForSeconds(delayBetweenLines);
        }
        // Optionally: Disable speech bubble or trigger next step in tutorial
        speechBubble.SetActive(false);
    }

    private IEnumerator SpeakLine(string line)
    {
        copAnimator.SetTrigger("Talk");
        speechBubble.SetActive(true);
        speechText.text = "";
        isTyping = true;

        foreach (char c in line)
        {
            speechText.text += c;
            yield return new WaitForSeconds(textTypingSpeed);
        }

        isTyping = false;
    }

    // Optional: Skip typing when tapped
    public void OnSpeechBubbleClicked()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            speechText.text = tutorialLines[currentLineIndex];
            isTyping = false;
        }
    }
}