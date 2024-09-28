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

    protected virtual void ShowLeaderboards()
    {

    }

    protected virtual void Next()
    {

    }
}
