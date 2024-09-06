using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigationManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Load Game");
    }

    public void Settings()
    {
        SceneManager.LoadSceneAsync("Settings");
    }

    public void Back()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }

    public void NewGame()
    {
        SceneManager.LoadSceneAsync("Game Menu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
