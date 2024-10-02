using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryMenu : Panel
{
    [SerializeField] private TMP_Text timerText = null;
    [SerializeField] private Button leaderboardsButton = null;
    [SerializeField] private Button nextButton = null;
    [SerializeField] private Button retryButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        leaderboardsButton.onClick.AddListener(ShowLeaderboards);
        nextButton.onClick.AddListener(Next);
        retryButton.onClick.AddListener(Retry);
        base.Initialize();
    }

    public void SetTimerText(string timer)
    {
        if (timerText != null)
        {
            timerText.text = timer;
        }
    }

    protected virtual void ShowLeaderboards()
    {
       PanelManager.GetSingleton("victory").Close();
       PanelManager.GetSingleton("leaderboard").Open();
    }

    protected virtual void Next()
    {
        PanelManager.GetSingleton("victory").Close();
        BattleManager.Singleton.DestroyEnemy();
    }

    protected virtual void Retry()
    {
        BattleManager.Singleton.RestartAsync();
    }
}
