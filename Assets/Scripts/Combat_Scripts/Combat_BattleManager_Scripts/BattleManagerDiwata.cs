using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine.Utility;

public class BattleManagerDiwata : BattleManagerSigbinTikbalang
{
    [SerializeField] private GameObject ultimateButton;
    [SerializeField] private GameObject bossObject;
    private Boss boss;
    private SpawnerBossDiwata spawner;

    public override void StartBattle()
    {
        base.StartBattle();
        bossObject.SetActive(true);
        boss = bossObject.GetComponent<Boss>();
        spawner = spawnsObject.GetComponent<SpawnerBossDiwata>();
    }

    public void ShowUltimateButton()
    {
        ultimateButton.SetActive(true);
    }

    public void UseUltimate()
    {
        ultimateButton.SetActive(false);
        spawner.ResetCounters();
        boss.TakeUltimateDamage();
        StartCoroutine(WaitForSkillAnimationThenDefeat());
    }

    public override void Defeated()
    {
        spawnsObject.SetActive(false);
        bossObject.SetActive(false);
        PanelManager.GetSingleton("hud").Close();
        LeaderboardManager.Singleton.SubmitTimeBossChapter1((long)(elapsedTime * 1000));
        VictoryMenu diwataVictoryMenu = PanelManager.GetSingleton("victory") as DiwataVictoryMenu;
        if (diwataVictoryMenu != null)
        {
            diwataVictoryMenu.SetTimerText($"Time: {timerText.text}");
            diwataVictoryMenu.Open();
        }
    }

    private IEnumerator WaitForSkillAnimationThenDefeat()
    {
        SkillAnimation skillAnimation = GetComponent<SkillAnimation>();
        skillAnimation.StartMoveAnimation();

        yield return new WaitForSeconds(skillAnimation.animationDuration + 1.5f);

        int health = boss.GetHealth();
        if (health <= 0)
        {
            Defeated();
        }
    }
}
