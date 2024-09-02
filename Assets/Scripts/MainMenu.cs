using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Load Game");
    }

    public void Options()
    {
        SceneManager.LoadSceneAsync("Options");
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
