using System.Collections.Generic;

[System.Serializable]
public class LeaderboardItemList
{
    public List<LeaderboardItem> items = null;
}

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