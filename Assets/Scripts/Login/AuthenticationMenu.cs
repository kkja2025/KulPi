using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AuthenticationMenu : Panel
{
    [SerializeField] private TMP_InputField usernameInput = null;
    [SerializeField] private TMP_InputField passwordInput = null;
    [SerializeField] private Button SignInButton = null;
    [SerializeField] private Button SignUpButton = null;
    [SerializeField] private Button ForgotPasswordButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        ForgotPasswordButton.onClick.AddListener(ForgotPassword);
        SignInButton.onClick.AddListener(SignIn);
        SignUpButton.onClick.AddListener(SignUp);
        base.Initialize();
    }

    public override void Open()
    {
        usernameInput.text = "";
        passwordInput.text = "";
        base.Open();
    }

    private void ForgotPassword()
    {
        PanelManager.GetSingleton("auth").Close();
        PanelManager.GetSingleton("forgot").Open();
    }

    private void SignIn()
    {
        string username = usernameInput.text.Trim();
        string pass = passwordInput.text.Trim();
        if (string.IsNullOrEmpty(username) == false && string.IsNullOrEmpty(pass) == false)
        {
            LoginManager.Singleton.SignInWithUsernameAndPasswordAsync(username, pass);
        }
    }

    private void SignUp()
    {
        PanelManager.GetSingleton("auth").Close();
        PanelManager.GetSingleton("link").Open();

    }
}