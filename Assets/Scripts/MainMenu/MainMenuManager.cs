using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private bool initialized = false;
    private bool eventsInitialized = false;
    private static MainMenuManager singleton = null;

    public static MainMenuManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindFirstObjectByType<MainMenuManager>();
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
                    PanelManager.CloseAll();
                    PanelManager.GetSingleton("main").Open();
                } else
                {
                    SceneManager.LoadScene("Login");
                }
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.StartService, "Failed to connect to the network.", "Retry");
            }
        }

        private void SetupEvents()
        {
            eventsInitialized = true;
            AuthenticationService.Instance.Expired += () =>
            {
                SceneManager.LoadScene("Login");
            };
        }

        public void SignOut()
        {
            AuthenticationService.Instance.SignOut();
            SceneManager.LoadScene("Login");
        }
    }