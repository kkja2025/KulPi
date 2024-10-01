using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiwataVictoryMenu : VictoryMenu
{
    protected async override void ShowLeaderboards()
    {
        await LeaderboardManager.Singleton.DisplayBossChapter1Leaderboard();
        base.ShowLeaderboards();
    }

    protected override void Next()
    {
        base.Next();
        PanelManager.GetSingleton("victory").Close();
        GameManager.Singleton.UnlockEncyclopediaItem("Diwata", "unlock");
    }
}
