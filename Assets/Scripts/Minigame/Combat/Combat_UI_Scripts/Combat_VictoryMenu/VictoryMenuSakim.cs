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

    protected override void Next()
    {
        base.Next();
        BattleManager.Singleton.RemoveEncounter();
        PanelManager.GetSingleton("victory").Close();
        // GameManager.Singleton.UnlockEncyclopediaItem("", "unlock");
    }
}
