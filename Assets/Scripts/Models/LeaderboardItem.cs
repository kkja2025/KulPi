using System;
using UnityEngine;

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