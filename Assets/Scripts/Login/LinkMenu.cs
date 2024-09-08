using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LinkMenu : Panel
{
    [SerializeField] private TMP_InputField emailInput = null;
    [SerializeField] private Button NextButton = null;
     [SerializeField] private Button BackButton = null;


    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        NextButton.onClick.AddListener(Next);
        BackButton.onClick.AddListener(Back);
        base.Initialize();
    }

    public override void Open()
    {
        emailInput.text = "";
        base.Open();
    }

    private void Next()
    {
        string email = emailInput.text.Trim();
        if (string.IsNullOrEmpty(email) == false)
        {
            if (IsEmailValid(email) == false)
            {
                LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Invalid email address", "OK");
            }
            else
            {
                FirebaseAuthManager.Singleton.RegisterUser(email, "tempPassword");
            }
        }
        else
        {
            LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Email cannot be empty", "OK");
        }
    }

      private bool IsEmailValid(string email) 
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    private void Back()
    {
        PanelManager.GetSingleton("link").Close();
        PanelManager.GetSingleton("auth").Open();
    }
}