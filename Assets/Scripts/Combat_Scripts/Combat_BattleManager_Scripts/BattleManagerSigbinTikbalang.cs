using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManagerSigbinTikbalang : BattleManager
{
    [SerializeField] public TMP_Text sigbinCountText;
    [SerializeField] public TMP_Text tikbalangCountText;
    private static BattleManagerSigbinTikbalang singleton = null;

    public static new BattleManagerSigbinTikbalang Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<BattleManagerSigbinTikbalang>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("BattleManagerSigbinTikbalang not found in the scene!");
                }
            }
            return (BattleManagerSigbinTikbalang)singleton;
        }
    }

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
        spawnsObject.SetActive(true);
        UpdateSigbinCount(0);
        UpdateTikbalangCount(0);
        isTimerRunning = true;
    }

    public override void Defeated()
    {
        Destroy(spawnsObject);
        PanelManager.GetSingleton("hud").Close();
        LeaderboardManager.Singleton.SubmitTimeSigbinTikbalangChapter1((long)(elapsedTime * 1000));
        VictoryMenu victoryMenu = PanelManager.GetSingleton("victory") as SigbinTikbalangVictoryMenu;
        if (victoryMenu != null)
        {
            victoryMenu.SetTimerText(timerText.text);
            victoryMenu.Open();
        }
    }
}
