using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine.Utility;

public class BattleManagerDiwata : BattleManagerSigbinTikbalang
{
    [SerializeField] public GameObject ultimateButton;
    private Boss boss;
    public GameObject bossObject;
    private static BattleManagerDiwata singleton = null;

    public static new BattleManagerDiwata Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<BattleManagerDiwata>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("BattleManagerDiwata not found in the scene!");
                }
            }
            return (BattleManagerDiwata)singleton;
        }
    }

    public void ShowUltimateButton()
    {
        ultimateButton.SetActive(true);
    }

    public override void StartBattle()
    {
        bossObject.SetActive(true);
        spawnsObject.SetActive(true);
        UpdateSigbinCount(0);
        UpdateTikbalangCount(0);
        boss = bossObject.GetComponent<Boss>();
        isTimerRunning = true;
    }

    public void UseUltimate()
    {
        Debug.Log("Ultimate used!");
        SpawnerBossDiwata spawner = spawnsObject.GetComponent<SpawnerBossDiwata>();
        ultimateButton.SetActive(false);
        boss.TakeUltimateDamage();
        spawner.ResetCounters();
    }

    public override void Defeated()
    {
        Debug.Log("Boss destroyed!");
        Destroy(bossObject);
        Destroy(spawnsObject);
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
