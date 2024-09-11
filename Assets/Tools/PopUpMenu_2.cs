using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopUpMenu_2 : Panel
{
    [SerializeField] private TextMeshProUGUI MenuText = null;
    [SerializeField] private TextMeshProUGUI Button1Text = null;
    [SerializeField] private TextMeshProUGUI Button2Text = null;
    [SerializeField] private Button ActionButton_1 = null;
    [SerializeField] private Button ActionButton_2 = null;

    public enum Action
    {
        None = 0,
        NewGame = 1
    }
    
    private Action action = Action.None;
    
    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        ActionButton_1.onClick.AddListener(ButtonAction_1);
        ActionButton_2.onClick.AddListener(ButtonAction_2);
        base.Initialize();
    }

    public override void Open()
    {
        action = Action.None;
        base.Open();
    }
    
    public void Open(Action action, string menu, string button1, string button2)
    {
        Open();
        this.action = action;
        if (string.IsNullOrEmpty(menu) == false)
        {
            MenuText.text = menu;
        }
        if (string.IsNullOrEmpty(button1) == false)
        {
            Button1Text.text = button1;
        }
        if (string.IsNullOrEmpty(button2) == false)
        {
            Button2Text.text = button2;
        }
    }
    
    private void ButtonAction_1()
    {
        Close();
        switch (action)
        {
            case Action.NewGame:
                break;
        }
    }

    private void ButtonAction_2()
    {
        Close();
        switch (action)
        {
            case Action.NewGame:
                MainMenuManager.Singleton.NewGame();
                break;
        }
    }
}