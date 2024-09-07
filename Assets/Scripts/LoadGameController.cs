using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadGameController : MonoBehaviour
{
    public Button BackButton;
    public Button LoadGameButton;
    public Button StartANewGameButton;
    public GameObject WarningPanel;
    public Button ConfirmButton;
    public Button CancelButton;
    public TMP_Text WarningText;

    void Start()
    {
        BackButton.onClick.AddListener(GoBack);
        LoadGameButton.onClick.AddListener(LoadSaveData);
        StartANewGameButton.onClick.AddListener(ShowWarningPanel);

        ConfirmButton.onClick.AddListener(StartNewGame);
        CancelButton.onClick.AddListener(HideWarningPanel);

        WarningPanel.SetActive(false);
    }

    void GoBack()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }

    void LoadSaveData()
    {
        Debug.Log("Load save data clicked");
    }

    void ShowWarningPanel()
    {
        WarningText.text = "Warning: Starting a new game will replace the existing save data!";
        WarningPanel.SetActive(true);
    }

    void HideWarningPanel()
    {
        WarningPanel.SetActive(false);
    }
    void StartNewGame()
    {
        Debug.Log("Start new game clicked");
        HideWarningPanel();
    }
}
