using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Extensions;

public class LoginManager : MonoBehaviour
{
    private bool initialized = false;
    private bool eventsInitialized = false;
    
    private static LoginManager singleton = null;
    private FirebaseApp app;

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

           var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                if (FirebaseApp.DefaultInstance != null)
                {
                    app = FirebaseApp.DefaultInstance;
                    Debug.Log("Firebase initialized successfully.");
                }
            }
            else
            {
                Debug.Log($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }

            if (!eventsInitialized)
            {
                SetupEvents();
            }

            if (AuthenticationService.Instance.IsSignedIn)
            {
                Debug.Log("User is signed in.");
                SignInAnonymouslyAsync();
            }
            else
            {
                Debug.Log("User is not signed in.");
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

    public async void SignInWithUsernameAndPasswordAsync(string username, string password)
    {
        PanelManager.GetSingleton("loading").Open();
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
        }
        catch (AuthenticationException exception)
        {
            Debug.Log(exception);
            ShowPopUp(PopUpMenu.Action.OpenAuthMenu, "Username or password is wrong.", "OK");
        }
        catch (RequestFailedException exception)
        {
            Debug.Log(exception);
            ShowPopUp(PopUpMenu.Action.OpenAuthMenu, "Username or password is wrong.", "OK");
        }
    }

    public async void SignUpWithUsernameAndPasswordAsync(string username, string password)
    {
        PanelManager.GetSingleton("loading").Open();
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
        }
        catch (AuthenticationException exception)
        {
            Debug.Log(exception);
            ShowPopUp(PopUpMenu.Action.None, "Username already exists.", "OK");
        }
        catch (RequestFailedException exception)
        {
            Debug.Log(exception);
            ShowPopUp(PopUpMenu.Action.None, "Username already taken. Please try again.", "Retry");
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
            SceneManager.LoadScene("MainMenu");
    }
    
    public void ShowPopUp(PopUpMenu.Action action = PopUpMenu.Action.None, string text = "", string button = "")
    {
        PanelManager.Close("loading");
        PopUpMenu panel = (PopUpMenu)PanelManager.GetSingleton("popup");
        panel.Open(action, text, button);
    }
}