using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneNavigationManager : MonoBehaviour
{
    public Button StartGameButton;
    public Button OptionsButton;
    public Button QuitButton;
    public void Start()
    {
        StartGameButton.onClick.AddListener(StartGame);
        OptionsButton.onClick.AddListener(Options);
        QuitButton.onClick.AddListener(QuitGame);
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Load Game");
    }

    public void Settings()
    {
        SceneManager.LoadSceneAsync("Settings");
    }

    public void QuitGame()
    {
        Application.Quit();
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
