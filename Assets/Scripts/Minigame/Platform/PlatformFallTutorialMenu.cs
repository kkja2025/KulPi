using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlatformFallTutorialMenu : TutorialMenu
{ 
    [SerializeField] string key = "";
    [SerializeField] private Button startCasualButton = null;

    public override void Initialize()
    {
        if (startCasualButton != null)
        {
        startCasualButton.onClick.AddListener(StartCasualGame);
        }
        base.Initialize();
    }
    protected override void StartGame()
    {
        if(key == "reset")
        {
            PlatformFallManager.Singleton.RestartAsync();
        } else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        } else {
            PlatformFallManager.Singleton.StartGame();
        }
    }

    private void StartCasualGame()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        } else {
            PlatformFallManager.Singleton.StartCasualGame();
        }
    }
}
