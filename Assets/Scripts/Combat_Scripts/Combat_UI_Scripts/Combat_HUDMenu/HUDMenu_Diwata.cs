using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDBossCombatMenu : HUDCombatMenu
{
    [SerializeField] private Button ultimateButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        ultimateButton.onClick.AddListener(UseUltimate);
        pauseButton.onClick.AddListener(OpenPause);
        tutorialButton.onClick.AddListener(OpenTutorial);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }
    public void UseUltimate()
    {
        BattleManagerDiwata.Singleton.UseUltimate();
    }
}
