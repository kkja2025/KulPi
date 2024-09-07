using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopUpMenu : Panel
{

    [SerializeField] private TextMeshProUGUI MenuText = null;
    [SerializeField] private TextMeshProUGUI ButtonText = null;
    [SerializeField] private Button ActionButton = null;

    public enum Action
    {
        None = 0, StartService = 1, OpenAuthMenu = 2
    }
    
    private Action action = Action.None;
    
    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        ActionButton.onClick.AddListener(ButtonAction);
        base.Initialize();
    }

    public override void Open()
    {
        action = Action.None;
        base.Open();
    }
    
    public void Open(Action action, string menu, string button)
    {
        Open();
        this.action = action;
        if (string.IsNullOrEmpty(menu) == false)
        {
            MenuText.text = menu;
        }
        if (string.IsNullOrEmpty(button) == false)
        {
            ButtonText.text = button;
        }
    }
    
    private void ButtonAction()
    {
        Close();
        switch (action)
        {
            case Action.StartService:
                LoginManager.Singleton.StartClientService();
                break;
            case Action.OpenAuthMenu:
                PanelManager.CloseAll();
                PanelManager.Open("auth");
                break;
        }
    } 
}