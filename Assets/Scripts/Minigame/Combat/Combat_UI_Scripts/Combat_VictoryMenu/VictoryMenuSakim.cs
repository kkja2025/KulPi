using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SakimVictoryMenu: VictoryMenu
{
    protected async override void ShowLeaderboards()
    {
        await LeaderboardManager.Singleton.DisplayFinalBossLeaderboard();
        base.ShowLeaderboards();
    }

    protected async override void Next()
    {
        base.Next();
        BattleManager.Singleton.RemoveEncounter();
        PanelManager.GetSingleton("victory").Close();
        PanelManager.GetSingleton("cutscene").Open();
        // GameManager.Singleton.UnlockEncyclopediaItem("", "unlock");   
    }
}
