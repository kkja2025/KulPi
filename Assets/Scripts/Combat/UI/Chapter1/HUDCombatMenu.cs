using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDCombatMenu : Panel
{
    [SerializeField] protected Button pauseButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        pauseButton.onClick.AddListener(OpenPause);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }
    
    protected void OpenPause()
    {
        Time.timeScale = 0;
        PanelManager.GetSingleton("pause").Open();
    }
}
