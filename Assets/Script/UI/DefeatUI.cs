using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatUI : MonoBehaviour
{
    public void RestartLevel()
    {
        // Reloads the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // replace with actual name if different
    }
}