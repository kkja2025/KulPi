using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LapuLapuVictoryMenu : VictoryMenu
{
    protected async override void ShowLeaderboards()
    {
        // await LeaderboardManager.Singleton.DisplayBossChapter2Leaderboard();
        base.ShowLeaderboards();
    }

    protected override void Next()
    {
        base.Next();
        // BattleManager.Singleton.RemoveEncounter();
        // PanelManager.GetSingleton("victory").Close();
        // GameManager.Singleton.UnlockEncyclopediaItem("Diwata", "unlock");
    }
}
