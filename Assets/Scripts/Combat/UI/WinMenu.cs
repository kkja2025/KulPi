using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinMenu : Panel
{
    [SerializeField] private TMP_Text timerText = null;
    [SerializeField] private Button leaderboardsButton = null;
    [SerializeField] private Button nextButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        leaderboardsButton.onClick.AddListener(ShowLeaderboards);
        nextButton.onClick.AddListener(Next);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    public void SetTimerText(string timer)
    {
        if (timerText != null)
        {
            timerText.text = timer;
        }
    }

    public void ShowLeaderboards()
    {
       PanelManager.GetSingleton("win").Close();
       PanelManager.GetSingleton("leaderboards").Open();
    }

    private void Next()
    {
        PanelManager.GetSingleton("win").Close();
        GameManager.Singleton.UnlockEncyclopediaItem("Diwata");
        PanelManager.GetSingleton("unlock").Open();
    }
}
