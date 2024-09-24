using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopUpMenu_0 : Panel
{

    [SerializeField] private TextMeshProUGUI MenuText = null;

    public enum Action
    {
        None = 0
    }
    
    private Action action = Action.None;
    
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
        action = Action.None;
        base.Open();
    }
    
    public void Open(Action action, string menu)
    {
        Open();
        this.action = action;
        if (string.IsNullOrEmpty(menu) == false)
        {
            MenuText.text = menu;
        }
    }
}