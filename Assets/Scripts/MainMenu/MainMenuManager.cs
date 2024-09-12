using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Unity.Services.Authentication;

public class MainMenuManager : MonoBehaviour
{
    private bool initialized = false;
    private static MainMenuManager singleton = null;
    private FirebaseAuth auth;
    private FirebaseUser user;

    public static MainMenuManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<MainMenuManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("MainMenuManager not found in the scene!");
                }
            }
            return singleton;
        }
    }

    private void Initialize()
    {
        if (initialized) return;
        initialized = true;
    }

    private void Awake()
    {
        Application.runInBackground = true;
        StartClientService();
    }

    private void StartClientService()
    {
        PanelManager.CloseAll();
        PanelManager.GetSingleton("loading").Open();
        try
        {
            var firebaseService = FirebaseService.Singleton;
            auth = firebaseService.Auth;
            FirebaseUser user = auth.CurrentUser;
            if (user != null)
            {
                Debug.Log("User is already signed in: " + user.UserId);
                Debug.Log($"User is already signed in: {user.Email}");
            } else
            {
                Debug.Log("User is not signed in.");
            }
            if (auth.CurrentUser == null)
            {
                Debug.Log("User is not signed in.");
                PanelManager.CloseAll();
                SceneManager.LoadScene("Login");
            }
            else
            {
                PanelManager.CloseAll();
                PanelManager.GetSingleton("main").Open();
            }
        }
        catch (Exception exception)
        {
            Debug.LogError($"Failed to start client service: {exception.Message}");
            ShowPopUp(PopUpMenu.Action.None, "Failed to start client service.", "Retry");
            SceneManager.LoadScene("Login");
        }
    }

    public void SignOut()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            Debug.Log("User signed out from Firebase.");
        }
        else
        {
            Debug.Log("No user is signed in to Firebase.");
        }

        if (AuthenticationService.Instance.IsSignedIn)
        {
            AuthenticationService.Instance.SignOut();
            Debug.Log("User signed out from Unity Services.");
        }
        else
        {
            Debug.Log("No user is signed in to Unity Services.");
        }

        PanelManager.CloseAll();
        SceneManager.LoadScene("Login");
    }

    public void LoadGame()
    {
        try
        {
            CloudSaveManager.Singleton.LoadPlayerData();
            Debug.Log("Player data loaded successfully.");
            // PanelManager.GetSingleton("loading").Open();
            // SceneManager.LoadScene("GameScene");
        }
        catch (Exception e)
        {
            Debug.Log("Error loading player data: " + e.Message);
            ShowPopUp(PopUpMenu.Action.None, "No save data found. Please start a new game.", "Ok");
        }
    }
    public void NewGame()
    {
        FirebaseUser user = auth.CurrentUser;
        CloudSaveManager.Singleton.DeletePlayerData();
        CloudSaveManager.Singleton.SavePlayerData(1, user.UserId);
        // PanelManager.CloseAll();
        // PanelManager.GetSingleton("loading").Open();
        // SceneManager.LoadScene("GameScene");
    }

    public void ShowPopUp(PopUpMenu.Action action = PopUpMenu.Action.None, string text = "", string button = "")
    {
        PanelManager.Close("loading");
        PopUpMenu panel = (PopUpMenu)PanelManager.GetSingleton("popup");
        panel.Open(action, text, button);
    }
}
