using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoatVictoryMenu : VictoryMenu
{
    protected override void ShowLeaderboards()
    {
        // await LeaderboardManager.Singleton.DisplayBossChapter2Leaderboard();
        base.ShowLeaderboards();
    }

    protected async override void Next()
    {
        base.Next();
        PanelManager.GetSingleton("victory").Close();
        if (GameManager.Singleton != null)
        {
            GameManager.Singleton.UnlockEncyclopediaItem("Boat", "unlock");
            Vector3 newPosition = new Vector3(223, 0, 0);
            await GameManager.Singleton.SavePlayerDataPosition(newPosition);
            PlayerData player = GameManager.Singleton.GetPlayerData();
        }
    }

    protected override void Retry()
    {
        BoatManager.Singleton.RestartAsync();
    }
}
