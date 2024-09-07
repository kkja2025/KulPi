using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGameController : MonoBehaviour
{
    public Button BackButton;
    public Button LoadGameButton;
    public Button StartANewGameButton;
    void Start()
    {
        BackButton.onClick.AddListener(GoBack);
        LoadGameButton.onClick.AddListener(LoadSaveData);
        StartANewGameButton.onClick.AddListener(StartNewGame);
        
    }

    void GoBack()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }

    void LoadSaveData()
    {
        Debug.Log("Load save data clicked");
    }

    void StartNewGame()
    {
        Debug.Log("Start new game clicked");
    }
}
