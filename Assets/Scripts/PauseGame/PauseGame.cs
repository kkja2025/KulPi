using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject pausePanel;

    public void TogglePausePanel()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
    }

    public void ReturnMainMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ClosePausePanel()
    {
        pausePanel.SetActive(false);
    }

    
}
