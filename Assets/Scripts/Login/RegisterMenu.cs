using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine.UI;

public class RegisterMenu : Panel
{
    [SerializeField] private TMP_InputField usernameInput = null;
    [SerializeField] private TMP_InputField passwordInput = null;
    [SerializeField] private TMP_InputField confirmPasswordInput = null;
    [SerializeField] private Button SignUpButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        SignUpButton.onClick.AddListener(SignUp);
        base.Initialize();
    }

    public override void Open()
    {
        usernameInput.text = "";
        passwordInput.text = "";
        confirmPasswordInput.text = "";
        base.Open();
    }

    private void SignUp()
    {
        string username = usernameInput.text.Trim();
        string pass = passwordInput.text.Trim();
        string passConfirm = confirmPasswordInput.text.Trim();
        if (string.IsNullOrEmpty(pass) == false && string.IsNullOrEmpty(passConfirm) == false && string.IsNullOrEmpty(username) == false)
        {
            if (IsUsernameValid(username) == false)
            {
                LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Username must be between 3 and 20 characters and only supports letters, numbers and symbols like ., -, @ or _.", "OK");
            }             
            else if (IsPasswordValid(pass) == false)
            {
                LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Password must be between 8 and 30 characters and contain at least one uppercase letter, one lowercase letter, one digit, and one symbol", "OK");
            }
            else if (pass != passConfirm)
            {
                LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Password do not match", "OK");
            }
            else
            {
                Debug.Log("Registering user with username: " + username + " and password: " + pass);
                LoginManager.Singleton.SignUpWithUsernameAndPasswordAsync(username, pass);
            }
        }
        else
        {
            LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "All fields must be filled", "OK");
        }
    }

// Username must be between 3 and 20 characters and only supports letters, numbers and symbols like ., -, @ or _.
    private bool IsUsernameValid(string username)
    {
        if (username.Length < 3 || username.Length > 20)
        {
            return false;
        }
        foreach (char c in username)
        {
            if (!char.IsLetterOrDigit(c) && c != '.' && c != '-' && c != '@' && c != '_')
            {
                return false;
            }
        }
        return true;
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
}