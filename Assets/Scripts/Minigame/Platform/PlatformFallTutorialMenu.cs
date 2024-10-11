using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlatformFallTutorialMenu : TutorialMenu
{ 
    [SerializeField] string key = "";
    protected override void StartGame()
    {
        if(key == "reset")
        {
            PlatformFallManager.Singleton.RestartAsync();
        }
        PanelManager.GetSingleton("tutorial").Close();
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        } else {
            PlatformFallManager.Singleton.StartGame();
        }
    }
}
