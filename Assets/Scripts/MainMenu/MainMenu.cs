using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenu : Panel
{
    [SerializeField] private Button LogoutButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        LogoutButton.onClick.AddListener(SignOut);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    private void SignOut()
    {
        MainMenuManager.Singleton.SignOut();
    }
}
