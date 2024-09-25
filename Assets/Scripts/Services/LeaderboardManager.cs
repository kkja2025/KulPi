using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using Unity.Services.Core;

[System.Serializable]
public class LeaderboardItem
{
    public string playerName;
    public string playerScore;
    public string playerRank;

    public LeaderboardItem(string id, string score, string rank)
    {
        playerName = id;
        playerScore = score;
        playerRank = rank;
    }
}

[System.Serializable]
public class LeaderboardItemList
{
    public List<LeaderboardItem> items = null;
}

public class LeaderboardManager : MonoBehaviour
{
    private bool initialized = false;
    private static LeaderboardManager singleton = null;
    public List<LeaderboardItem> leaderboard = null;

    public static LeaderboardManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<LeaderboardManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("LeaderboardManager not found in the scene!");
                }
            }
            return singleton;
        }
    }

    private void Initialize()
    {
        if (initialized) return;
        initialized = true;
    }

    private void Awake()
    {
        Application.runInBackground = true;
        StartClientService();
    }

    private async void StartClientService()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
        }
    }

    public async void SubmitTimeBossChapter1(long timeInMilliseconds)
    {
        try
        {
            string leaderboardId = "diwata_battle_leaderboard";
            await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, timeInMilliseconds);
            Debug.Log("Best time submitted successfully!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to submit time: {e.Message}");
        }
    }

    public async void DisplayBossChapter1Leaderboard()
    {
        try
        {
            string leaderboardId = "diwata_battle_leaderboard";
            var leaderboardEntries = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);
            foreach (var leaderboardEntry in leaderboardEntries.Results)
            {
                LeaderboardItem item = new LeaderboardItem(leaderboardEntry.PlayerName, leaderboardEntry.Score.ToString(), leaderboardEntry.Rank.ToString());
                leaderboard = new List<LeaderboardItem>();
                leaderboard.Add(item);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to fetch leaderboard: {e.Message}");
        }
    }
}
