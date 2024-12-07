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
        StartCoroutine(WaitForSkillAnimationThenContinue());
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

    private IEnumerator WaitForSkillAnimationThenContinue()
    {
        spawnsObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        spawnsObject.SetActive(false);

        // Check if the boss has been defeated
        if (boss.GetHealth() <= 0)
        {
            Defeated();
        }
    }
}
