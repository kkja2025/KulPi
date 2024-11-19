using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManagerSakim : BattleManager
{
    public override void StartBattle()
    {
        PanelManager.GetSingleton("hud").Open();
        PanelManager.GetSingleton("quiz").Open();
        isTimerRunning = true;
    }

    public override void Defeated()
    {

        VictoryAnimation();
    }
}
