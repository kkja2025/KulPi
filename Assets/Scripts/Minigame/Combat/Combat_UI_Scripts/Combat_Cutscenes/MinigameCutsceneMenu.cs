using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameCutsceneMenu : Panel
{
    [SerializeField] private Button actionButton = null;
    [SerializeField] private string actionButtonNavigation = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        if (actionButton != null)
        {
        actionButton.onClick.AddListener(ActionButton);
        }
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
        var cutscene1 = PanelManager.GetSingleton(id + "1");
        if (cutscene1 != null)
        {
            cutscene1.Open();
        } else {
            return;
        }
    }
    
    protected void ActionButton()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        switch (actionButtonNavigation)
        {
            case "victory":
                Close();
                PanelManager.GetSingleton(actionButtonNavigation).Open();
                break;
            default:
                PanelManager.LoadSceneAsync(actionButtonNavigation);
                break;
        }
    }
}
