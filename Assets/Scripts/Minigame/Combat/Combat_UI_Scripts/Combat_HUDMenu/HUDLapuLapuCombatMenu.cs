using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDLapuLapuCombatMenu : HUDCombatMenu
{
    [SerializeField] private Button ultimateButton = null;
    private BattleManagerLapuLapu battleManagerLapuLapu;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        ultimateButton.onClick.AddListener(UseUltimate);
        battleManagerLapuLapu = FindObjectOfType<BattleManagerLapuLapu>();
        base.Initialize();
    }

    private void UseUltimate()
    {
        battleManagerLapuLapu.UseUltimate();
    }
}
