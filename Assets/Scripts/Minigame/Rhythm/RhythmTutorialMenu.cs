using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RhythmTutorialMenu : TutorialMenu
{
    protected override void StartGame()
    {
        PanelManager.GetSingleton("tutorial").Close();
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        } else {
            RhythmManager.Singleton.StartGame();
        }
    }
}
