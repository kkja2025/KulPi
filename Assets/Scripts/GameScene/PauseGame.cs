using UnityEngine;
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
        mainMenuButton.onClick.AddListener(OpenReturnMainMenu);
    }

    private void OpenPausePanel()
    {
        Debug.Log("Pause button clicked. Opening pause panel.");
        pausePanel.SetActive(true);
        pauseButton.gameObject.SetActive(true);
    }

    private void ResumeGame()
    {
        Debug.Log("Resume button clicked. Closing pause panel.");
        pausePanel.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    private void OpenSettings()
    {
        Debug.Log("Settings button clicked.");

        pauseButton.gameObject.SetActive(false);

        pausePanel.SetActive(false);
        Debug.Log("Pause panel closed.");

        PanelManager.GetSingleton("main").Close();
        Debug.Log("Main panel closed.");

        PanelManager.GetSingleton("settings").Open();
        Debug.Log("Settings panel opened.");

        PanelManager.GetSingleton("volumemaster").Open();
        PanelManager.GetSingleton("volumebgm").Open();
        PanelManager.GetSingleton("volumesfx").Open();
        PanelManager.GetSingleton("volumevoice").Open();
        Debug.Log("All volume panels opened.");
    }

    private void OpenReturnMainMenu()
    {
        Debug.Log("Main menu button clicked. Returning to main menu.");

        pauseButton.gameObject.SetActive(false);

        pausePanel.SetActive(false);
        Debug.Log("Pause panel closed.");

        PanelManager.GetSingleton("main").Open();
        Debug.Log("Main menu panel opened.");
    }
}
