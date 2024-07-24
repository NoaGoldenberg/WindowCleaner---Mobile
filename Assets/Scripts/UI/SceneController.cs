using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private bool isPaused = false;

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void ReloadCurrentScene()
    {
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("Start");
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if (isPaused) return;
        AudioManager.instance.PlayPauseSound();
        //AudioManager.instance.PauseAudio(); // Pause the audio if not muted
        isPaused = true;
        Time.timeScale = 0f;
        UIManager.instance.ShowPauseMenu();
    }

    public void ResumeGame()
    {
        isPaused = false;
        AudioManager.instance.PlaySelectSound();
        //AudioManager.instance.ResumeAudio(); // Resume the audio if it was paused
        Time.timeScale = 1f;
        UIManager.instance.HidePauseMenu();
        UIManager.instance.HideScore();
    }

    public void skipTutorial()
    {
        Debug.Log("entered");
        SceneManager.LoadScene("Game");
    }
}