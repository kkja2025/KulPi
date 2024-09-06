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
    [SerializeField] private Button registerButton = null;
    [SerializeField] private Button backButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        registerButton.onClick.AddListener(Register);
        backButton.onClick.AddListener(Back);
        base.Initialize();
    }

    private void Register()
    {
        if (string.IsNullOrEmpty(usernameInput.text) || string.IsNullOrEmpty(passwordInput.text) || string.IsNullOrEmpty(confirmPasswordInput.text))
        {
            PanelManager.GetSingleton("error").Open(ErrorMenu.Action.None, "Please fill in all fields.", "OK");
            return;
        }
        if (passwordInput.text != confirmPasswordInput.text)
        {
            PanelManager.GetSingleton("error").Open(ErrorMenu.Action.None, "Passwords do not match.", "OK");
            return;
        }
        SignUpWithUsernamePasswordAsync(usernameInput.text, passwordInput.text);
    }

    private void Back()
    {
        PanelManager.GetSingleton("register").Close();
        PanelManager.GetSingleton("login").Open();
    }

    private async void SignUpWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            PanelManager.GetSingleton("error").Open(ErrorMenu.Action.None, "SignUp is successful.", "OK");
        }
        catch (AuthenticationException ex)
        {
            PanelManager.GetSingleton("error").Open(ErrorMenu.Action.None, ex.Message, "OK");
        }
        catch (RequestFailedException ex)
        {
            PanelManager.GetSingleton("error").Open(ErrorMenu.Action.None, ex.Message, "OK");
        }
    }
    
}