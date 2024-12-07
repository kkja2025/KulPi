using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using Unity.Services.Core;

public class LeaderboardManager : MonoBehaviour
{
    private bool initialized = false;
    private static LeaderboardManager singleton = null;
    private List<LeaderboardItem> leaderboard = new List<LeaderboardItem>();

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

    public List<LeaderboardItem> GetLeaderboard()
    {
        return leaderboard;
    }
    
    private async void StartClientService()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
        }
    }

    public async Task<string> GetPlayerNameAsync()
    {
        try
        {
            string player = await AuthenticationService.Instance.GetPlayerNameAsync();
            return player;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to get player name: {e.Message}");
            return null;
        }
    }
    public async Task SubmitTimeBossChapter1(long timeInMilliseconds)
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

    public async Task DisplayBossChapter1Leaderboard()
    {
        try
        {
            string leaderboardId = "diwata_battle_leaderboard";
            var leaderboardEntries = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);
            leaderboard = new List<LeaderboardItem>();
            foreach (var leaderboardEntry in leaderboardEntries.Results)
            {
                LeaderboardItem item = new LeaderboardItem(leaderboardEntry.PlayerName, leaderboardEntry.Score.ToString(), leaderboardEntry.Rank.ToString());
                leaderboard.Add(item);
            }
        }
        catch (Exception e)
        {
            leaderboard = new List<LeaderboardItem>();
            Debug.LogError($"Failed to fetch leaderboard: {e.Message}");
        }
    }
    
    public async Task SubmitTimeSigbinTikbalangChapter1(long timeInMilliseconds)
    {
        try
        {
            string leaderboardId = "sigbin_tikbalang_battle_leaderboard";
            await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, timeInMilliseconds);
            Debug.Log("Best time submitted successfully!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to submit time: {e.Message}");
        }
    }
    
    public async Task DisplaySigbinTikbalangChapter1Leaderboard()
    {
        try
        {
            string leaderboardId = "sigbin_tikbalang_battle_leaderboard";
            var leaderboardEntries = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);
            leaderboard = new List<LeaderboardItem>();
            foreach (var leaderboardEntry in leaderboardEntries.Results)
            {
                LeaderboardItem item = new LeaderboardItem(leaderboardEntry.PlayerName, leaderboardEntry.Score.ToString(), leaderboardEntry.Rank.ToString());
                leaderboard.Add(item);
            }
        }
        catch (Exception e)
        {
            leaderboard = new List<LeaderboardItem>();
            Debug.LogError($"Failed to fetch leaderboard: {e.Message}");
        }
    }

    public async Task SubmitTimeChapter1SacredGrove(long timeInMilliseconds)
    {
        try
        {
            string leaderboardId = "sacred_grove_platform_leaderboard";
            await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, timeInMilliseconds);
            Debug.Log("Best time submitted successfully!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to submit time: {e.Message}");
        }
    }

    public async Task DisplaySacredGroveChapter1Leaderboard()
    {
        try
        {
            string leaderboardId = "sacred_grove_platform_leaderboard";
            var leaderboardEntries = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);
            leaderboard = new List<LeaderboardItem>();
            foreach (var leaderboardEntry in leaderboardEntries.Results)
            {
                LeaderboardItem item = new LeaderboardItem(leaderboardEntry.PlayerName, leaderboardEntry.Score.ToString(), leaderboardEntry.Rank.ToString());
                leaderboard.Add(item);
            }
        }
        catch (Exception e)
        {
            leaderboard = new List<LeaderboardItem>();
            Debug.LogError($"Failed to fetch leaderboard: {e.Message}");
        }
    }

    public async Task SubmitTimeBossChapter2(long timeInMilliseconds)
    {
        try
        {
            string leaderboardId = "lapulapu_battle_leaderboard";
            await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, timeInMilliseconds);
            Debug.Log("Best time submitted successfully!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to submit time: {e.Message}");
        }
    }

    public async Task DisplayBossChapter2Leaderboard()
    {
        try
        {
            string leaderboardId = "lapulapu_battle_leaderboard";
            var leaderboardEntries = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);
            leaderboard = new List<LeaderboardItem>();
            foreach (var leaderboardEntry in leaderboardEntries.Results)
            {
                LeaderboardItem item = new LeaderboardItem(leaderboardEntry.PlayerName, leaderboardEntry.Score.ToString(), leaderboardEntry.Rank.ToString());
                leaderboard.Add(item);
            }
        }
        catch (Exception e)
        {
            leaderboard = new List<LeaderboardItem>();
            Debug.LogError($"Failed to fetch leaderboard: {e.Message}");
        }
    }

    public async Task SubmitTimeFinalBoss(long timeInMilliseconds)
    {
        try
        {
            string leaderboardId = "final_boss_leaderboard";
            await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, timeInMilliseconds);
            Debug.Log("Best time submitted successfully!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to submit time: {e.Message}");
        }
    }

    public async Task DisplayFinalBossLeaderboard()
    {
        try
        {
            string leaderboardId = "final_boss_leaderboard";
            var leaderboardEntries = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);
            leaderboard = new List<LeaderboardItem>();
            foreach (var leaderboardEntry in leaderboardEntries.Results)
            {
                LeaderboardItem item = new LeaderboardItem(leaderboardEntry.PlayerName, leaderboardEntry.Score.ToString(), leaderboardEntry.Rank.ToString());
                leaderboard.Add(item);
            }
        }
        catch (Exception e)
        {
            leaderboard = new List<LeaderboardItem>();
            Debug.LogError($"Failed to fetch leaderboard: {e.Message}");
        }
    }

    public async Task SubmitTimeRhythm(long timeInMilliseconds)
    {
        try
        {
            string leaderboardId = "rhythm_leaderboard";
            await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, timeInMilliseconds);
            Debug.Log("Best time submitted successfully!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to submit time: {e.Message}");
        }
    }

    public async Task DisplayRhythmLeaderboard()
    {
        try
        {
            string leaderboardId = "rhythm_leaderboard";
            var leaderboardEntries = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);
            leaderboard = new List<LeaderboardItem>();
            foreach (var leaderboardEntry in leaderboardEntries.Results)
            {
                LeaderboardItem item = new LeaderboardItem(leaderboardEntry.PlayerName, leaderboardEntry.Score.ToString(), leaderboardEntry.Rank.ToString());
                leaderboard.Add(item);
            }
        }
        catch (Exception e)
        {
            leaderboard = new List<LeaderboardItem>();
            Debug.LogError($"Failed to fetch leaderboard: {e.Message}");
        }
    }
}