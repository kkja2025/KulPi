using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ForgotPasswordMenu : Panel
{
    [SerializeField] private TMP_InputField emailInput = null;
    [SerializeField] private Button RequestButton = null;
    [SerializeField] private Button BackButton = null;
    [SerializeField] private RectTransform panelRectTransform;

    private Vector2 originalPosition; 

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        RequestButton.onClick.AddListener(RequestPasswordReset);
        BackButton.onClick.AddListener(Back);
        originalPosition = panelRectTransform.anchoredPosition;
        base.Initialize();
    }

    public override void Open()
    {
        panelRectTransform.anchoredPosition = originalPosition;
        emailInput.text = "";
        base.Open();
    }

    private void Back()
    {
        PanelManager.GetSingleton("forgot").Close();
        PanelManager.GetSingleton("auth").Open();
    }

    public void RequestPasswordReset()
    {
        string email = emailInput.text;
        if (string.IsNullOrEmpty(email))
        {
            Debug.Log("Email field is empty.");
            LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Enter a valid email", "OK");
        } else if (IsEmailValid(email) == false)
        {
            Debug.Log("Invalid email address.");
            LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Invalid email address", "OK");
        }
        else
        {
            LoginManager.Singleton.RequestResetPasswordAsync(email);
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
}