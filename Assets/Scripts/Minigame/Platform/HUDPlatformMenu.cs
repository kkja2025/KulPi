using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDPlatformMenu : HUDCombatMenu
{
    protected override void OpenTutorial()
    {   
        Time.timeScale = 0;
        PanelManager.GetSingleton("tutorialpause").Open();
    }
}
