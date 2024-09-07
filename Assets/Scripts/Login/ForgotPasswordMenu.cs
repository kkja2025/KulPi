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

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        RequestButton.onClick.AddListener(RequestPasswordReset);
        BackButton.onClick.AddListener(Back);
        base.Initialize();
    }

    public override void Open()
    {
        emailInput.text = "";
        base.Open();
    }

    private void RequestPasswordReset()
    {
        string email = emailInput.text;
        if (string.IsNullOrEmpty(email) == false)
        {
            // Request password reset logic
            // LoginManager.Singleton.RequestPasswordResetAsync(email);
        }
    }

    private void Back()
    {
        PanelManager.GetSingleton("forget").Close();
        PanelManager.GetSingleton("auth").Open();
    }
}
