using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDCombatMenu : Panel
{
    [SerializeField] protected Button pauseButton = null;
    [SerializeField] protected Button tutorialButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        tutorialButton.onClick.AddListener(OpenTutorial);
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

    protected void OpenTutorial()
    {   
        Time.timeScale = 0;
        PanelManager.GetSingleton("tutorial").Open();
    }
}
