using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;

    private bool isPaused = false;
    private bool isDisabled = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (!isDisabled && !isPaused)
        {
            isPaused = true;
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
        }
    }

    public void DisablePause()
    {
        isDisabled = true;
        pausePanel.SetActive(false);
    }
}