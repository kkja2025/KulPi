using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDCombatMenu : Panel
{
    [SerializeField] private Button ultimateButton = null;
    [SerializeField] private Button pauseButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        ultimateButton.onClick.AddListener(UseUltimate);
        pauseButton.onClick.AddListener(OpenPause);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }
    public void UseUltimate()
    {
        BattleManager.Singleton.UseUltimate();
    }

    private void OpenPause()
    {
        Time.timeScale = 0;
        PanelManager.GetSingleton("pause").Open();
    }
}
