using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            if (AuthenticationService.Instance.SessionTokenExists)
            {
                SignInAnonymouslyAsync();
            }
            else
            {
                PanelManager.GetSingleton("auth").Open();
            }
        }
        catch (Exception exception)
        {
            ErrorMenu panel = (ErrorMenu)PanelManager.GetSingleton("error");
            panel.Open("Failed to start the client service.", "Retry");
            Debug.LogException(exception);
        }
        finally
        {
            PanelManager.GetSingleton("loading").Close();
        }
    }

    public async void SignInAnonymouslyAsync()
    {
        PanelManager.GetSingleton("loading").Open();
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (Exception exception)
        {
            ErrorMenu panel = (ErrorMenu)PanelManager.GetSingleton("error");
            panel.Open("Failed to sign in anonymously.", "Retry");
            Debug.LogException(exception);
        }
        finally
        {
            PanelManager.GetSingleton("loading").Close();
        }
    }

    private void SetupEvents()
    {
        eventsInitialized = true;
        AuthenticationService.Instance.SignedIn += () =>
        {
            PanelManager.GetSingleton("main").Open();
        };
        AuthenticationService.Instance.SignedOut += () =>
        {
            PanelManager.GetSingleton("auth").Open();
        };
        AuthenticationService.Expired += () =>
        {
            PanelManager.GetSingleton("auth").Open();
        };
    }
}