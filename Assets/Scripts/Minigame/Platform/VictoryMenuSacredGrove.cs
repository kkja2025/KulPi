using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryMenuSacredGrove : VictoryMenu
{
    protected async override void ShowLeaderboards()
    {
        await LeaderboardManager.Singleton.DisplaySacredGroveChapter1Leaderboard();
        base.ShowLeaderboards();
    }

    protected override void Retry()
    {
        PlatformFallManager.Singleton.RestartAsync();
    }
}
