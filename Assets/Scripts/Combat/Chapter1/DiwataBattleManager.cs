using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine.Utility;
using UnityEditor.Rendering.LookDev;

public class DiwataBattleManager : SigbinTikbalangBattleManager
{
    [SerializeField] public GameObject ultimateButton;
    private BossDiwata boss;
    public GameObject bossObject;
    private static DiwataBattleManager singleton = null;

    public static new DiwataBattleManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<DiwataBattleManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("DiwataBattleManager not found in the scene!");
                }
            }
            return (DiwataBattleManager)singleton;
        }
    }

    public void ShowUltimateButton()
    {
        ultimateButton.SetActive(true);
    }

    public override void StartBattle()
    {
        bossObject.SetActive(true);
        minionsObject.SetActive(true);
        UpdateSigbinCount(0);
        UpdateTikbalangCount(0);
        boss = bossObject.GetComponent<BossDiwata>();
        isTimerRunning = true;
    }

    public void UseUltimate()
    {
        Debug.Log("Ultimate used!");
        BossDiwataMinionSpawner minionSpawner = minionsObject.GetComponent<BossDiwataMinionSpawner>();
        ultimateButton.SetActive(false);
        boss.TakeUltimateDamage();
        minionSpawner.ResetCounters();
    }

    public override void Defeated()
    {
        Debug.Log("Boss destroyed!");
        Destroy(bossObject);
        Destroy(minionsObject);
        PanelManager.GetSingleton("hud").Close();
        LeaderboardManager.Singleton.SubmitTimeBossChapter1((long)(elapsedTime * 1000));
        VictoryMenu diwataVictoryMenu = PanelManager.GetSingleton("victory") as DiwataVictoryMenu;
        if (diwataVictoryMenu != null)
        {
            diwataVictoryMenu.SetTimerText(timerText.text);
            diwataVictoryMenu.Open();
        }
    }
}
