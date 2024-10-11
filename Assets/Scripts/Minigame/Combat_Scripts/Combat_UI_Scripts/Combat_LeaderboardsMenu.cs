using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LeaderboardsMenu : Panel
{
    [SerializeField] private Button backButton = null;
    [SerializeField] private RectTransform leaderboardContent = null;
    [SerializeField] private GameObject leaderboardItemPrefab = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        backButton.onClick.AddListener(Back);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
        LoadLeaderboardItems();
    }

    private void Back()
    {
        PanelManager.GetSingleton("leaderboard").Close();
        PanelManager.GetSingleton("victory").Open();
    }

    private void LoadLeaderboardItems()
    {
        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }
        var items = LeaderboardManager.Singleton.GetLeaderboard();

        foreach (var item in items)
        {
            AddLeaderboardItem(item);
        }
    }

    private void AddLeaderboardItem (LeaderboardItem item)
    {
        GameObject newItem = Instantiate(leaderboardItemPrefab, leaderboardContent);

        TMP_Text[] textComponents = newItem.GetComponentsInChildren<TMP_Text>();

        foreach (var text in textComponents)
        {
            if (text.name == "TextRank")
            {
                int playerRank = int.Parse(item.playerRank);
                playerRank += 1;
                text.text = playerRank.ToString();
            }
            else if (text.name == "TextPlayerName")
            {
                text.text = item.playerName;
            }
            else if (text.name == "TextTime")
            {
                int totalMilliseconds = int.Parse(item.playerScore);
                int totalSeconds = totalMilliseconds / 1000; 
                int minutes = totalSeconds / 60; 
                int seconds = totalSeconds % 60;
                string formattedTime = string.Format("{0:D2}:{1:D2}", minutes, seconds);
                text.text = formattedTime;
            }
        }
    }
}
