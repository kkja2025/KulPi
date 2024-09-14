using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public GameObject pausePanel;
    public Button pauseButton;
    public Button resumeButton;
    public Button settingsButton;
    public Button mainMenuButton;

    private void Start()
    {
        pausePanel.SetActive(false);

        pauseButton.onClick.AddListener(OpenPausePanel);
        resumeButton.onClick.AddListener(ResumeGame);
        settingsButton.onClick.AddListener(OpenSettings);
        mainMenuButton.onClick.AddListener(ReturnMainMenu);
    }

    private void OpenPausePanel()
    {
        Debug.Log("Pause button clicked. Opening pause panel.");
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        Debug.Log("Resume button clicked. Closing pause panel.");
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OpenSettings()
    {
        Debug.Log("Settings button clicked. Opening settings scene.");
        Time.timeScale = 1f;
        SceneManager.LoadScene("SettingsScene");
    }

    private void ReturnMainMenu()
    {
        Debug.Log("Main menu button clicked. Returning to main menu.");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
}