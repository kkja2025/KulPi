using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialPanel : Panel
{
    [SerializeField] private Button backButton = null;
    [SerializeField] private Button nextButton = null;
    [SerializeField] private string nextTutorialID = null;
    [SerializeField] private string previousTutorialID = null;

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
        PanelManager.GetSingleton(id).Close();
        PanelManager.GetSingleton(nextTutorialID).Open();
    }

    public void PreviousPanel()
    {
        PanelManager.GetSingleton(id).Close();
        PanelManager.GetSingleton(previousTutorialID).Open();
    }
}
