using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManagerSakim : BattleManager
{
    [SerializeField] private GameObject bossObject;
    [SerializeField] private GameObject ultimateButton; 
    [SerializeField] private GridPlayer player;
    private Boss boss;

    public override void StartBattle()
    {
        boss = bossObject.GetComponent<Boss>();
        ultimateButton.GetComponent<Button>().onClick.AddListener(UseUltimate);
        PanelManager.GetSingleton("hud").Open();
        PanelManager.GetSingleton("quiz").Open();
        isTimerRunning = true;
    }

    public void ShowUltimateButton()
    {
        ultimateButton.SetActive(true);
    }

    public void UseUltimate()
    {
        ultimateButton.SetActive(false);
        player.ResetCharge();
        boss.TakeUltimateDamage();
        // StartCoroutine(WaitForSkillAnimationThenContinue());
        if (boss.GetHealth() <= 0)
        {
            Defeated();
        }
    }

    public override void Defeated()
    {
        VictoryAnimation();
        PanelManager.GetSingleton("hud").Close();
        // PanelManager.GetSingleton("cutscene").Open();
        VictoryMenu sakimVictoryMenu = PanelManager.GetSingleton("victory") as SakimVictoryMenu;
        if (sakimVictoryMenu != null)
        {
            sakimVictoryMenu.SetTimerText($"Time: {timerText.text}");
            sakimVictoryMenu.Open();
        }
    }

    // This coroutine waits for skill animation to complete and checks if the boss is defeated
    private IEnumerator WaitForSkillAnimationThenContinue()
    {
        SkillAnimation skillAnimation = GetComponent<SkillAnimation>();
        skillAnimation.StartMoveAnimation();  // Trigger skill animation

        // Wait for the animation duration to finish
        yield return new WaitForSeconds(skillAnimation.animationDuration + 1.5f);

        // Check if the boss has been defeated
        if (boss.GetHealth() <= 0)
        {
            Defeated();
        }
    }
}
