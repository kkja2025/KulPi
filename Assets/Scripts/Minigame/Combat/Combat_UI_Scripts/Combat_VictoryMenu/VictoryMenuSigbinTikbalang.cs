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

    protected async override void Next()
    {
        base.Next();
        BattleManager.Singleton.RemoveEncounter();
        GameManager.Singleton.UnlockEncyclopediaItem("Sigbin", "unlocksigbin");
        
        Vector3 startingPosition = new Vector3(557, 0, 0);
        PlayerData playerData = GameManager.Singleton.GetPlayerData();
        playerData.SetPosition(startingPosition);
        playerData.SetActiveQuest("Survive as you navigate deeper in the Sacred Grove.");
        await CloudSaveManager.Singleton.SaveNewPlayerData(playerData);
    }
}
