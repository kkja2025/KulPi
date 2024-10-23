using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MiniGameManager
{
    [SerializeField] protected GameObject spawnsObject;
    private static BattleManager singleton = null;

    public static BattleManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<BattleManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("BattleManager not found in the scene!");
                }
            }
            return singleton;
        }
    }

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
        InitializeScene();
    }
    
    public virtual void StartBattle()
    {
        PanelManager.GetSingleton("hud").Open();
        spawnsObject.SetActive(true);
        isTimerRunning = true;
    }

    public virtual void Defeated()
    {
        spawnsObject.SetActive(false);
        VictoryAnimation();
    }

    public void VictoryAnimation()
    {
        AudioManager.Singleton.PlayVictoryMusic();
        // Play victory animation
    }
}
