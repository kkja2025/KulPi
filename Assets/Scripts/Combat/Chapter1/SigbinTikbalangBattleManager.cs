using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SigbinTikbalangBattleManager : BattleManager
{
    private static SigbinTikbalangBattleManager singleton = null;

    public static new SigbinTikbalangBattleManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<SigbinTikbalangBattleManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("SigbinTikbalangBattleManager not found in the scene!");
                }
            }
            return (SigbinTikbalangBattleManager)singleton;
        }
    }

    public override void Defeated()
    {
        Destroy(minionsObject);
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
