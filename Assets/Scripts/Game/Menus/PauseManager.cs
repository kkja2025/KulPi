using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);

    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Settings()
    {
        PanelManager.GetSingleton("pause").Close();
        PanelManager.GetSingleton("settings").Open();
        PanelManager.GetSingleton("volumemaster").Open();
        PanelManager.GetSingleton("volumebgm").Open();
        PanelManager.GetSingleton("volumesfx").Open();
        PanelManager.GetSingleton("volumevoice").Open();
    }
}
