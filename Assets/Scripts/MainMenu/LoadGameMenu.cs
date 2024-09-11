using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoadGameMenu : Panel
{
    [SerializeField] private Button BackButton = null;
    [SerializeField] private Button StartGameButtton = null;
    [SerializeField] private Button NewGameButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        BackButton.onClick.AddListener(Back);
        StartGameButtton.onClick.AddListener(StartGame);
        NewGameButton.onClick.AddListener(NewGame);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    private void Back()
    {
        PanelManager.GetSingleton("load").Close();
        PanelManager.GetSingleton("main").Open();
    }

    private void StartGame()
    {
        MainMenuManager.Singleton.LoadGame();
    }

    private void NewGame()
    {
        PanelManager.Close("loading");
        PopUpMenu_2 panel = (PopUpMenu_2)PanelManager.GetSingleton("popup2");
        panel.Open(PopUpMenu_2.Action.NewGame, "Are you sure you want to start a new game? Old save data will be overwritten.", "Cancel", "Confirm");
    }
}
