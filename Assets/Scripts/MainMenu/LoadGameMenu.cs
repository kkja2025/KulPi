using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoadGameMenu : Panel
{
    [SerializeField] private Button BackButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        BackButton.onClick.AddListener(Back);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    private void Back()
    {
        PanelManager.GetSingleton("load").Close();
        PanelManager.GetSingleton("main").Open();
    }
}
