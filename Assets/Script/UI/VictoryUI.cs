using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    [SerializeField] private float delayBeforeReturn = 3f;
    [SerializeField] private string levelSelectSceneName = "LevelSelect";

    private void OnEnable()
    {
        StartCoroutine(ReturnToLevelSelectAfterDelay());
    }

    private IEnumerator ReturnToLevelSelectAfterDelay()
    {
        yield return new WaitForSecondsRealtime(delayBeforeReturn);
        SceneManager.LoadScene(levelSelectSceneName);
    }
}