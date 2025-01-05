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

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        LogoutButton.onClick.AddListener(SignOut);
        LoadButton.onClick.AddListener(LoadGame);
        SettingsButton.onClick.AddListener(OpenSettings);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    private void SignOut()
    {
        PopUpMenu_2 panel = (PopUpMenu_2)PanelManager.GetSingleton("popup2");
        panel.Open(PopUpMenu_2.Action.LogOut, "Are you sure you want to sign out?", "Cancel", "Confirm"); 
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
}
