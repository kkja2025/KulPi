using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiwataEntry : EncyclopediaUnlock
{
    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        encyclopediaItem = EncyclopediaItem.Figures_Chapter1_Diwata();
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    public override void ActionButton()
    {
        PanelManager.GetSingleton("unlock").Close();
        PanelManager.GetSingleton("win").Open();
    }
}