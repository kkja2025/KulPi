using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SigbinTikbalangVictoryMenu : VictoryMenu
{
    protected async override void ShowLeaderboards()
    {
        await LeaderboardManager.Singleton.DisplaySigbinTikbalangChapter1Leaderboard();
        base.ShowLeaderboards();
    }

    protected override void Next()
    {
        base.Next();
        BattleManager.Singleton.DestroyEnemy();
        GameManager.Singleton.UnlockEncyclopediaItem("Sigbin", "unlocksigbin");
    }
}
