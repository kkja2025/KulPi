using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatMenu : Panel
{
    [SerializeField] private Button startButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        startButton.onClick.AddListener(StartCombat);
        base.Initialize();
    }
    
    private void StartCombat()
    {
        Time.timeScale = 1;
        startButton.gameObject.SetActive(false);
    }
}
