using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardsMenu : Panel
{
    [SerializeField] private Button backButton = null;

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
    }

    private void Back()
    {
        PanelManager.GetSingleton("leaderboards").Close();
        PanelManager.GetSingleton("win").Open();
    }
}
