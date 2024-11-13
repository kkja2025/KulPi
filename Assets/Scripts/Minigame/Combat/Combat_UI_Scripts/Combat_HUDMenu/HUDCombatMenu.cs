using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDCombatMenu : Panel
{
    [SerializeField] private Button pauseButton = null;
    [SerializeField] private Button tutorialButton = null;

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
    
    private void OpenPause()
    {
        Time.timeScale = 0;
        PanelManager.GetSingleton("pause").Open();
    }

    protected virtual void OpenTutorial()
    {   
        Time.timeScale = 0;
        PanelManager.GetSingleton("tutorial").Open();
    }
}
