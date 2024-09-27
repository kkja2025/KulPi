using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SigbinTikbalangVictoryMenu : VictoryMenu
{
    protected async override void ShowLeaderboards()
    {
       PanelManager.GetSingleton("victory").Close();
       await LeaderboardManager.Singleton.DisplaySigbinTikbalangChapter1Leaderboard();
       PanelManager.GetSingleton("leaderboard").Open();

    }

    protected override void Next()
    {
        PanelManager.GetSingleton("victory").Close();
        GameManager.Singleton.UnlockEncyclopediaItem("Sigbin", "unlocksigbin");
    }
}
