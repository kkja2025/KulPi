using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    private bool initialized = false;
    private bool eventsInitialized = false;
    
    private static LoginManager singleton = null;

    public static LoginManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindFirstObjectByType<LoginManager>();
                singleton.Initialize();
            }
            return singleton; 
        }
    }

    private void Initialize()
    {
        if (initialized) { return; }
        initialized = true;
    }
    
    private void OnDestroy()
    {
        if (singleton == this)
        {
            singleton = null;
        }
    }
    
    private void Awake()
    {
        Application.runInBackground = true;
        StartClientService();
    }

    public async void StartClientService()
    {
        PanelManager.CloseAll();
        PanelManager.GetSingleton("loading").Open();
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                var options = new InitializationOptions();
                options.SetProfile("default_profile");
                await UnityServices.InitializeAsync();
            }

            if (!eventsInitialized)
            {
                SetupEvents();
            }

            if (AuthenticationService.Instance.IsSignedIn)
            {
                SignInAnonymouslyAsync();
            }
            else
            {
                PanelManager.CloseAll();
                PanelManager.GetSingleton("auth").Open();
            }
        }
        catch (Exception exception)
        {
            Debug.Log(exception);
            ShowPopUp(PopUpMenu.Action.StartService, "Failed to start client.", "Retry");
        }
    }

    public async void SignInAnonymouslyAsync()
    {
        PanelManager.GetSingleton("loading").Open();
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (AuthenticationException exception)
        {
            Debug.Log(exception);
            ShowPopUp(PopUpMenu.Action.OpenAuthMenu, "Logging in failed.", "OK");
        }
        catch (RequestFailedException exception)
        {
            Debug.Log(exception);
            ShowPopUp(PopUpMenu.Action.OpenAuthMenu, "Failed to connect. Please try again.", "Retry");
        }
    }

    public async void SignInWithEmailAndPasswordAsync(string email, string password)
    {
        PanelManager.GetSingleton("loading").Open();
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(email, password);
        }
        catch (AuthenticationException exception)
        {
            Debug.Log(exception);
            ShowPopUp(PopUpMenu.Action.OpenAuthMenu, "Email or password is wrong.", "OK");
        }
        catch (RequestFailedException exception)
        {
            Debug.Log(exception);
            ShowPopUp(PopUpMenu.Action.OpenAuthMenu, "Failed to connect. Please try again.", "Retry");
        }
    }

    public async void SignUpWithEmailAndPasswordAsync(string email, string password)
    {
        PanelManager.GetSingleton("loading").Open();
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(email, password);
        }
        catch (AuthenticationException exception)
        {
            Debug.Log(exception);
            ShowPopUp(PopUpMenu.Action.None, "Failed to register.", "OK");
        }
        catch (RequestFailedException exception)
        {
            Debug.Log(exception);
            ShowPopUp(PopUpMenu.Action.None, "Failed to connect. Please try again.", "Retry");
        }
    }

    private void SetupEvents()
    {
        eventsInitialized = true;
        AuthenticationService.Instance.SignedIn += () =>
        {
            SignInConfirmAsync();
        };
        AuthenticationService.Instance.Expired += () =>
        {
            PanelManager.GetSingleton("auth").Open();
        };
    }

    private void SignInConfirmAsync()
    {
            PanelManager.CloseAll();
            SceneManager.LoadScene("Main Menu");
    }
    
    public void ShowPopUp(PopUpMenu.Action action = PopUpMenu.Action.None, string error = "", string button = "")
    {
        PanelManager.Close("loading");
        PopUpMenu panel = (PopUpMenu)PanelManager.GetSingleton("error");
        panel.Open(action, error, button);
    }

    public void RequestPasswordResetAsync(string email)
    {
        PanelManager.GetSingleton("loading").Open();
        try
        {

        }
        catch 
        {

        }
    }

    public void ResetPasswordAsync(string newPassword, string confirmPassword)
    {
        PanelManager.GetSingleton("loading").Open();
        try
        {

        }
        catch 
        {

        }
    }
}