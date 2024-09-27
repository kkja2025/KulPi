using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialMenu : Panel
{
    [SerializeField] private Button startButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        startButton.onClick.AddListener(StartBattle);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    private void StartBattle()
    {
        PanelManager.GetSingleton("tutorial").Close();
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        } else {
            BattleManager.Singleton.StartBattle();
        }
    }
}
