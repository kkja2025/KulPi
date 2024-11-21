using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoatTutorialMenu : Panel
{
    [SerializeField] private Button startButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        if (startButton != null)
        {
        startButton.onClick.AddListener(StartGame);
        }
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
        var tutorial1 = PanelManager.GetSingleton("tutorial1");
        if (tutorial1 != null)
        {
            tutorial1.Open();
        } else {
            return;
        }
    }
    
    protected virtual void StartGame()
    {
        PanelManager.GetSingleton("tutorial").Close();
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        } else {
            BoatManager.Singleton.StartGame();
        }
    }
}
