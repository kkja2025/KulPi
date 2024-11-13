using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManagerLapuLapu : BattleManager
{
    [SerializeField] private TMP_Text enemyCountText;
    [SerializeField] private GameObject ultimateButton;
    [SerializeField] private GameObject bossObject;
    private Boss boss;
    private SpawnerBossLapuLapu spawner;

    public void UpdateEnemyCount(int count)
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "Enemy: " + count.ToString();
        }
    }

    public override void StartBattle()
    {
        base.StartBattle();
        bossObject.SetActive(true);
        boss = bossObject.GetComponent<Boss>();
        spawner = spawnsObject.GetComponent<SpawnerBossLapuLapu>();
        UpdateEnemyCount(0);
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

    public override async void Defeated()
    {
        spawnsObject.SetActive(false);
        bossObject.SetActive(false);
        VictoryAnimation();
        PanelManager.GetSingleton("hud").Close();
        await LeaderboardManager.Singleton.SubmitTimeBossChapter2((long)(elapsedTime * 1000));
        VictoryMenu lapuLapuVictoryMenu = PanelManager.GetSingleton("victory") as LapuLapuVictoryMenu;
        if (lapuLapuVictoryMenu != null)
        {
            lapuLapuVictoryMenu.SetTimerText($"Time: {timerText.text}");
            lapuLapuVictoryMenu.Open();
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
