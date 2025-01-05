using UnityEngine;


public class HUDBoatMenu : HUDCombatMenu
{
    protected override void OpenTutorial()
    {   
        Time.timeScale = 0;
        PanelManager.GetSingleton("tutorialpause").Open();
    }
}
