using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDDiwataCombatMenu : HUDCombatMenu
{
    [SerializeField] private Button ultimateButton = null;
    private BattleManagerDiwata battleManagerDiwata;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        ultimateButton.onClick.AddListener(UseUltimate);
        battleManagerDiwata = FindObjectOfType<BattleManagerDiwata>();
        base.Initialize();
    }

    private void UseUltimate()
    {
        battleManagerDiwata.UseUltimate();
    }
}
