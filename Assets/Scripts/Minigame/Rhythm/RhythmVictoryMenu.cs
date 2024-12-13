using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RhythmVictoryMenu : VictoryMenu
{
    protected async override void ShowLeaderboards()
    {
        await LeaderboardManager.Singleton.DisplayRhythmLeaderboard();
        base.ShowLeaderboards();
    }

    protected async override void Next()
    {
        base.Next();
        RhythmManager.Singleton.RemoveEncounter();
        PanelManager.GetSingleton("victory").Close();
        if (GameManager.Singleton != null)
        {
            GameManager.Singleton.UnlockEncyclopediaItem("RhythmsOfUnity", "unlock");
            PlayerData playerData = GameManager.Singleton.GetPlayerData();
            playerData.SetActiveQuest("Gather support for the resistance. (0/2)");
            await CloudSaveManager.Singleton.SavePlayerData(playerData);  
        }
    }

    protected override void Retry()
    {
        RhythmManager.Singleton.RestartAsync();
    }
}
