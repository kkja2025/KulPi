using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManagerSigbinTikbalang : BattleManager
{
    [SerializeField] private TMP_Text sigbinCountText;
    [SerializeField] private TMP_Text tikbalangCountText;  

    public void UpdateTikbalangCount(int count)
    {
        if (tikbalangCountText != null)
        {
            tikbalangCountText.text = "Tikbalang: " + count.ToString();
        }
    }

    public void UpdateSigbinCount(int  count)
    {
        if (sigbinCountText != null)
        {
            sigbinCountText.text = "Sigbin: " + count.ToString();
        }
    }

    public override void StartBattle()
    {
        base.StartBattle();
        UpdateSigbinCount(0);
        UpdateTikbalangCount(0);
    }

    public override void Defeated()
    {
        base.Defeated();
        PanelManager.GetSingleton("hud").Close();
        LeaderboardManager.Singleton.SubmitTimeSigbinTikbalangChapter1((long)(elapsedTime * 1000));
        VictoryMenu victoryMenu = PanelManager.GetSingleton("victory") as SigbinTikbalangVictoryMenu;
        if (victoryMenu != null)
        {
            victoryMenu.SetTimerText($"Time: {timerText.text}");
            victoryMenu.Open();
        }
    }
}
