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
            LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Please enter a valid email address", "OK");
        } 
        else
        {
        //     auth.SendPasswordResetEmailAsync(email).ContinueWith(task => 
        // {
        //     if (task.IsCanceled)
        //     {
        //         Debug.LogError("Password reset was canceled.");
        //     }
        //     if (task.IsFaulted)
        //     {
        //         Debug.LogError("Error encountered during password reset: " + task.Exception);
        //     }

        //     Debug.Log("Password reset email sent successfully.");
        // });
            FirebaseAuthManager.Singleton.ResetPassword(email);
        }
    }
}