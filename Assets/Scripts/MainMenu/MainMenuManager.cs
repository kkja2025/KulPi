using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class MainMenuManager : MonoBehaviour
{
    private bool initialized = false;
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
        DontDestroyOnLoad(gameObject);
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
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                var auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase initialized successfully.");

                if (auth.CurrentUser != null)
                {
                    Debug.Log($"User is signed in: {auth.CurrentUser.Email}");
                    PanelManager.CloseAll();
                    PanelManager.GetSingleton("main").Open();
                }
                else
                {
                    SceneManager.LoadScene("Login");
                }
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.StartService, "Failed to connect to the network.", "Retry");
            }
        }
        catch (Exception exception)
        {
            Debug.LogError(exception);
            LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.StartService, "Failed to connect to the network.", "Retry");
        }
    }

        public void SignOut()
        {
            var auth = FirebaseAuth.DefaultInstance;
            if (auth.CurrentUser != null)
            {
                auth.SignOut();
                Debug.Log("User signed out.");
                SceneManager.LoadScene("Login");
            }
        }
}