using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenu : Panel
{
    [SerializeField] private Button LogoutButton = null;
    [SerializeField] private Button LoadButton = null;
    [SerializeField] private Button SettingsButton = null;
    [SerializeField] private Button CloseGameButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        LogoutButton.onClick.AddListener(SignOut);
        LoadButton.onClick.AddListener(LoadGame);
        SettingsButton.onClick.AddListener(OpenSettings);
        CloseGameButton.onClick.AddListener(CloseGame);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    private void SignOut()
    {
        MainMenuManager.Singleton.SignOut();
    }

    private void LoadGame()
    {
        PanelManager.GetSingleton("main").Close();
        PanelManager.GetSingleton("load").Open();
    }

    private void OpenSettings()
    {
        PanelManager.GetSingleton("main").Close();
        PanelManager.GetSingleton("settings").Open();
        PanelManager.GetSingleton("volumemaster").Open();
        PanelManager.GetSingleton("volumebgm").Open();
        PanelManager.GetSingleton("volumesfx").Open();
        PanelManager.GetSingleton("volumevoice").Open();
    }

    private void CloseGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
