using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine.UI;

public class RegisterMenu : Panel
{
    [SerializeField] private TMP_InputField emailInput = null;
    [SerializeField] private TMP_InputField passwordInput = null;
    [SerializeField] private TMP_InputField confirmPasswordInput = null;
    [SerializeField] private Button SignUpButton = null;
    [SerializeField] private Button BackButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        SignUpButton.onClick.AddListener(SignUp);
        BackButton.onClick.AddListener(Back);
        base.Initialize();
    }

    public override void Open()
    {
        emailInput.text = "";
        passwordInput.text = "";
        confirmPasswordInput.text = "";
        base.Open();
    }

    private void SignUp()
    {
        string email = emailInput.text.Trim();
        string pass = passwordInput.text.Trim();
        string passConfirm = confirmPasswordInput.text.Trim();
        if (string.IsNullOrEmpty(email) == false && string.IsNullOrEmpty(pass) == false && string.IsNullOrEmpty(passConfirm) == false)
        {
            if (IsEmailValid(email) == false)
            {
                LoginManager.Singleton.ShowError(ErrorMenu.Action.None, "Invalid email address", "OK");
            }
            else if (IsPasswordValid(pass) == false)
            {
                LoginManager.Singleton.ShowError(ErrorMenu.Action.None, "Password must be between 8 and 30 characters and contain at least one uppercase letter, one lowercase letter, one digit, and one symbol", "OK");
            }
            else if (pass != passConfirm)
            {
                LoginManager.Singleton.ShowError(ErrorMenu.Action.None, "Passwords do not match", "OK");
            }
            else
            {
                LoginManager.Singleton.SignUpWithEmailAndPasswordAsync(email, pass);
            }
        }
    }

    private void Back()
    {
        PanelManager.GetSingleton("register").Close();
        PanelManager.GetSingleton("auth").Open();
    }

    private bool IsPasswordValid(string password)
    {
        if (password.Length < 8 || password.Length > 30)
        {
            return false;
        }
        
        bool hasUppercase = false;
        bool hasLowercase = false;
        bool hasDigit = false;
        bool hasSymbol = false;

        foreach (char c in password)
        {
            if (char.IsUpper(c))
            {
                hasUppercase = true;
            }
            else if (char.IsLower(c))
            {
                hasLowercase = true;
            }
            else if (char.IsDigit(c))
            {
                hasDigit = true;
            }
            else if (!char.IsLetterOrDigit(c))
            {
                hasSymbol = true;
            }
        }
        return hasUppercase && hasLowercase && hasDigit && hasSymbol;
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