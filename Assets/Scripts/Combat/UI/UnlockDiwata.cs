using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiwataUnlock : EncyclopediaUnlock
{
    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Back()
    {
        Debug.Log("CloseUnlock");
        PanelManager.GetSingleton("unlock").Close();
        PanelManager.GetSingleton("win").Open();
        GameManager.Singleton.UnlockEncyclopediaItem("Diwata");
    }
}