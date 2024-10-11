using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : Panel
{
    [SerializeField] private Button resumeButton = null;
    [SerializeField] private Button settingsButton = null;
    [SerializeField] private Button mainMenuButton = null;

    public override void Initialize()
    {
        base.Initialize();
        resumeButton.onClick.AddListener(Resume);
        settingsButton.onClick.AddListener(OpenSettings);
        mainMenuButton.onClick.AddListener(ReturnMainMenu);
    }

    private void Resume()
    {
        PanelManager.GetSingleton("pause").Close();
        Time.timeScale = 1;
    }

    private void OpenSettings()
    {
        PanelManager.GetSingleton("pause").Close();
        PanelManager.GetSingleton("settings").Open();
        PanelManager.GetSingleton("volumemaster").Open();
        PanelManager.GetSingleton("volumebgm").Open();
        PanelManager.GetSingleton("volumesfx").Open();
        PanelManager.GetSingleton("volumevoice").Open();
    }

    protected virtual void ReturnMainMenu()
    {
        Time.timeScale = 1;
        GameManager.Singleton.ReturnToMainMenu();
    }
}
