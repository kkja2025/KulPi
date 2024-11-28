using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RhythmVictoryMenu : VictoryMenu
{
    protected override void ShowLeaderboards()
    {
        // await LeaderboardManager.Singleton.DisplayBossChapter2Leaderboard();
        base.ShowLeaderboards();
    }

    protected override void Next()
    {
        base.Next();
        RhythmManager.Singleton.RemoveEncounter();
        PanelManager.GetSingleton("victory").Close();
        if (GameManager.Singleton != null)
        {
            GameManager.Singleton.UnlockEncyclopediaItem("RhythmsOfUnity", "unlock");
        }
    }
}
