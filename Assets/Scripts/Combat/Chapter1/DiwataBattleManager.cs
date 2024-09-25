using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DiwataBattleManager : BattleManager
{
    [SerializeField] public GameObject ultimateButton;
    private int ultimateUses = 0;
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
        boss = bossObject.GetComponent<BossDiwata>();
        isTimerRunning = true;
    }

    public void UseUltimate()
    {
        ultimateButton.SetActive(false);
        ultimateUses++;

        boss.TakeUltimateDamage();
        Debug.Log("Ultimate used! Ultimate uses: " + ultimateUses);

        if (ultimateUses >= 5)
        {
            Debug.Log("Boss defeated!");
        }
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
