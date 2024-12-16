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

    protected override async void Next()
    {
        base.Next();
        BattleManager.Singleton.RemoveEncounter();
        PanelManager.GetSingleton("victory").Close();
        GameManager.Singleton.UnlockEncyclopediaItem("Diwata", "unlock");
        await EncyclopediaManager.Singleton.SaveEncyclopediaEntryAsync();
        
        Vector3 startingPosition = new Vector3(0, 0, 0);
        PlayerData playerData = GameManager.Singleton.GetPlayerData();
        playerData.SetLevel("Chapter2");
        playerData.SetPosition(startingPosition);
        playerData.SetActiveQuest("Investigate Mactan Island to understand what has happened.");
        await CloudSaveManager.Singleton.SavePlayerData(playerData);     
    }
}
