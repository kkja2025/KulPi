using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutscenePanel : Panel
{
    [SerializeField] private Button backButton = null;
    [SerializeField] private Button nextButton = null;
    [SerializeField] private string nextCutsceneID = null;
    [SerializeField] private string previousCutsceneID = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
         if (nextButton != null)
        {
            nextButton.onClick.AddListener(NextPanel);
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(PreviousPanel);
        }
        base.Initialize();
    }

    public void NextPanel()
    {
        if(nextCutsceneID != "")
        {
            PanelManager.GetSingleton(id).Close();
            PanelManager.GetSingleton(nextCutsceneID).Open();
        }
    }

    public void PreviousPanel()
    {
        if(previousCutsceneID != "")
        {
            PanelManager.GetSingleton(id).Close();
            PanelManager.GetSingleton(previousCutsceneID).Open();
        }
    }
}
