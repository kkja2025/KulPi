using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LapuLapuVictoryMenu : VictoryMenu
{
    protected async override void ShowLeaderboards()
    {
        await LeaderboardManager.Singleton.DisplayBossChapter2Leaderboard();
        base.ShowLeaderboards();
    }

    protected async override void Next()
    {
        base.Next();
        BattleManager.Singleton.RemoveEncounter();
        PanelManager.GetSingleton("victory").Close();
        GameManager.Singleton.UnlockEncyclopediaItem("LapuLapu", "unlock");

        Vector3 startingPosition = new Vector3(539, 0, 0);
        PlayerData playerData = GameManager.Singleton.GetPlayerData();
        playerData.SetPosition(startingPosition);
        playerData.SetActiveQuest("Confront the Sakim!");
        await CloudSaveManager.Singleton.SaveNewPlayerData(playerData);     
    }
}
